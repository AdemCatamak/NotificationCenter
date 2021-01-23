using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotificationCenter.Business.Services;
using NotificationCenter.Business.Services.Imp;
using NotificationCenter.Controllers;
using NotificationCenter.Controllers.WebMiddleware;
using NotificationCenter.DataAccessLayer.DatabaseContext;
using NotificationCenter.DataAccessLayer.DatabaseContext.ConfigModels;
using NotificationCenter.DataAccessLayer.DatabaseContext.MongoDbSection;
using NotificationCenter.DataAccessLayer.IntegrationMessageBroker.ConfigModels;
using NotificationCenter.DataAccessLayer.IntegrationMessageBroker.MassTransitSection.MassTransitConverters;
using NotificationCenter.DataAccessLayer.IntegrationMessageBroker.MassTransitSection.MassTransitObservers;
using NotificationCenter.Hubs;

namespace NotificationCenter
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                                       {
                                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                           options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                                           options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.Default;
                                           options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
                                           options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                                           options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                                           options.SerializerSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                                       })
                    .AddApplicationPart(typeof(HomeController).Assembly);

            #region Swagger

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new OpenApiInfo());
                                       c.CustomSchemaIds(type => type.ToString());

                                       c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                         {
                                                                             Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                                                             Name = "Authorization",
                                                                             In = ParameterLocation.Header,
                                                                             Type = SecuritySchemeType.ApiKey
                                                                         });
                                       c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                {
                                                                    {
                                                                        new OpenApiSecurityScheme
                                                                        {
                                                                            Reference = new OpenApiReference
                                                                                        {
                                                                                            Type = ReferenceType.SecurityScheme,
                                                                                            Id = "Bearer"
                                                                                        },
                                                                        },
                                                                        new List<string>()
                                                                    }
                                                                });
                                   });

            #endregion

            #region MessageBroker

            var messageBrokerConfig = _configuration.GetSection("MessageBrokerConfig")
                                                    .Get<MessageBrokerConfig>();
            var messageBrokerOption = messageBrokerConfig.SelectedMessageBrokerOption();
            services.AddSingleton(messageBrokerConfig);
            services.AddSingleton(messageBrokerOption);
            services.AddMassTransitHostedService();

            services.AddSingleton<IConsumeObserver, BasicConsumeObserver>();
            services.AddSingleton<ISendObserver, BasicSendObserver>();
            services.AddSingleton<IPublishObserver, BasicPublishObserver>();

            void ConfigureBusFactory(IBusFactoryConfigurator busFactoryConfigurator, IBusRegistrationContext busRegistrationContext)
            {
                foreach (var observer in busRegistrationContext.GetServices<IConsumeObserver>()) busFactoryConfigurator.ConnectConsumeObserver(observer);
                foreach (var observer in busRegistrationContext.GetServices<ISendObserver>()) busFactoryConfigurator.ConnectSendObserver(observer);
                foreach (var observer in busRegistrationContext.GetServices<IPublishObserver>()) busFactoryConfigurator.ConnectPublishObserver(observer);

                busFactoryConfigurator.ConfigureJsonSerializer(settings =>
                                                               {
                                                                   settings.Converters.Add(new MassTransitTypeNameHandlingConverter(TypeNameHandling.Auto));
                                                                   return settings;
                                                               });
                busFactoryConfigurator.ConfigureJsonDeserializer(settings =>
                                                                 {
                                                                     settings.Converters.Add(new MassTransitTypeNameHandlingConverter(TypeNameHandling.Auto));
                                                                     return settings;
                                                                 });
            }

            services.AddMassTransit(serviceCollectionBusConfigurator =>
                                    {
                                        serviceCollectionBusConfigurator.AddConsumers(typeof(Startup).Assembly);
                                        serviceCollectionBusConfigurator.AddBus(provider =>
                                                                                {
                                                                                    IBusControl busControl = messageBrokerOption.BrokerType switch
                                                                                                             {
                                                                                                                 MessageBrokerTypes.RabbitMq => Bus.Factory.CreateUsingRabbitMq(cfg =>
                                                                                                                                                                                {
                                                                                                                                                                                    cfg.Host($"{messageBrokerOption.HostName}",
                                                                                                                                                                                             messageBrokerOption.VirtualHost,
                                                                                                                                                                                             hst =>
                                                                                                                                                                                             {
                                                                                                                                                                                                 hst.Username(messageBrokerOption.UserName);
                                                                                                                                                                                                 hst.Password(messageBrokerOption.Password);
                                                                                                                                                                                                 hst.UseCluster(clusterConfigurator => { clusterConfigurator.Node($"{messageBrokerOption.HostName}:{messageBrokerOption.Port}"); });
                                                                                                                                                                                             });

                                                                                                                                                                                    ConfigureBusFactory(cfg, provider);
                                                                                                                                                                                    cfg.ConfigureEndpoints(provider);
                                                                                                                                                                                }),
                                                                                                                 _ => throw new ArgumentOutOfRangeException()
                                                                                                             };


                                                                                    return busControl;
                                                                                });
                                    });

            #endregion

            #region Db

            var noSqlDbConfig = _configuration.GetSection("NoSqlDbConfig")
                                              .Get<NoSqlDbConfig>();
            var noSqlDbOption = noSqlDbConfig.SelectedDbOption();
            services.AddSingleton(noSqlDbOption);
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            switch (noSqlDbOption.NoSqlDbType)
            {
                case NoSqlDbTypes.MongoDb:
                    AddAs<IRepository>(services, typeof(IMongoRepository), ServiceLifetime.Scoped, typeof(IRepository).Assembly);
                    AddAs<IMongoDbScript>(services, typeof(IMongoDbScript), ServiceLifetime.Scoped, typeof(IRepository).Assembly);
                    services.AddScoped<IMongoClient>(provider =>
                                                     {
                                                         MongoUrlBuilder mongoUrlBuilder = new MongoUrlBuilder
                                                                                           {
                                                                                               Username = noSqlDbOption.Username,
                                                                                               Password = noSqlDbOption.Password,
                                                                                               DatabaseName = noSqlDbOption.DbName,
                                                                                               Servers = noSqlDbOption.Nodes.Select(x => new MongoServerAddress(x.Host, x.Port))
                                                                                           };
                                                         var mongoUrl = mongoUrlBuilder.ToMongoUrl();
                                                         MongoClient client = new MongoClient(mongoUrl);
                                                         return client;
                                                     });
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion

            #region AddAuth

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                                  options =>
                                  {
                                      options.RequireHttpsMetadata = false;
                                      options.SaveToken = true;
                                      options.TokenValidationParameters = new TokenValidationParameters
                                                                          {
                                                                              ValidateIssuer = true,
                                                                              ValidateAudience = true,
                                                                              ValidateLifetime = true,
                                                                              ValidateIssuerSigningKey = true,
                                                                              ValidIssuer = JwtAccessTokenGenerator.JWT_ISSUER,
                                                                              ValidAudience = JwtAccessTokenGenerator.JWT_AUDIENCE,
                                                                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtAccessTokenGenerator.JWT_KEY))
                                                                          };
                                      options.Events = new JwtBearerEvents
                                                       {
                                                           OnMessageReceived = context =>
                                                                               {
                                                                                   StringValues accessToken = context.Request.Query["access_token"];
                                                                                   PathString path = context.HttpContext.Request.Path;
                                                                                   if (!string.IsNullOrEmpty(accessToken))
                                                                                   {
                                                                                       if (path.StartsWithSegments("/auth-hubs"))

                                                                                       {
                                                                                           context.Token = $"Bearer {accessToken}";
                                                                                       }
                                                                                   }

                                                                                   return Task.CompletedTask;
                                                                               }
                                                       };
                                  });

            #endregion

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();

            services.AddSignalR()
                    .AddJsonProtocol(options =>
                                     {
                                         options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                         options.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                                     });
        }

        private void AddAs<TType>(IServiceCollection serviceCollection, Type baseType, ServiceLifetime serviceLifetime, params Assembly[] assemblies)
        {
            IEnumerable<TypeInfo> typesFromAssemblies = assemblies.SelectMany(assembly => assembly.DefinedTypes.Where(x => x.GetInterfaces().Contains(baseType) && x.IsClass && !x.IsAbstract));
            foreach (var type in typesFromAssemblies)
                serviceCollection.Add(new ServiceDescriptor(typeof(TType), type, serviceLifetime));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<GeneralExceptionHandlerMiddleware>();
            app.UseCors(x => x.AllowCredentials()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .WithOrigins(new string[] {"http://localhost:4200"})
                       );
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", ""); });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(builder =>
                             {
                                 builder.MapControllers();
                                 builder.MapHub<NotificationHub>("auth-hubs/notification-hub");
                             });
        }
    }
}