using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NotificationCenter.Business.Commands;
using NotificationCenter.Business.Events;
using NotificationCenter.DataAccessLayer.DatabaseContext;
using NotificationCenter.DataAccessLayer.DatabaseContext.Repositories;
using NotificationCenter.Model.Entities;
using NotificationCenter.Model.Exceptions;
using NotificationCenter.Model.Exceptions.Imp;

namespace NotificationCenter.Business.Services.Imp
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IBusControl _busControl;

        public NotificationService(IRepositoryFactory repositoryFactory, IBusControl busControl)
        {
            _repositoryFactory = repositoryFactory;
            _busControl = busControl;
        }

        public async Task CreateNotificationAsync(CreateNotificationCommand createNotificationCommand, CancellationToken cancellationToken)
        {
            Notification notification = new Notification(createNotificationCommand.NotificationCorrelationId,
                                                         createNotificationCommand.Username,
                                                         createNotificationCommand.NotificationTitle,
                                                         createNotificationCommand.NotificationContent,
                                                         createNotificationCommand.OperationDate);

            var notificationRepository = _repositoryFactory.Generate<INotificationRepository>();

            try
            {
                await notificationRepository.GetNotificationAsync(createNotificationCommand.Username, createNotificationCommand.NotificationCorrelationId, cancellationToken);
                throw new NotificationAlreadyExistException(createNotificationCommand.NotificationCorrelationId);
            }
            catch (NotFoundException<Notification>)
            {
                // continue
            }

            await notificationRepository.AddAsync(notification, cancellationToken);

            try
            {
                await _busControl.Publish(new NotificationCreatedEvent(notification.CorrelationId.ToString(), notification.Username.ToString(), notification.Title.ToString(), notification.Content.ToString(), notification.OperationDate, notification.IsSeen), cancellationToken);
            }
            catch (Exception)
            {
                // Exception is swallowed
            }
        }

        public async Task ChangeNotificationSeenStatusAsync(ChangeNotificationSeenStatusCommand changeNotificationSeenStatusCommand, CancellationToken cancellationToken)
        {
            var notificationRepository = _repositoryFactory.Generate<INotificationRepository>();
            Notification notification = await notificationRepository.GetNotificationAsync(changeNotificationSeenStatusCommand.Username, changeNotificationSeenStatusCommand.CorrelationId, cancellationToken);
            notification.ChangeSeenStatus(changeNotificationSeenStatusCommand.Seen);
            await notificationRepository.UpdateAsync(notification, cancellationToken);

            try
            {
                await _busControl.Publish(new NotificationSeenStatusChangedEvent(notification.CorrelationId.ToString(), notification.Username.ToString(), notification.Title.ToString(), notification.Content.ToString(), notification.OperationDate, notification.IsSeen), cancellationToken);
            }
            catch (Exception)
            {
                // Exception is swallowed
            }
        }

        public async Task<List<NotificationResponse>> QueryNotificationAsync(QueryNotificationCommand queryNotificationCommand, CancellationToken cancellationToken)
        {
            var notificationRepository = _repositoryFactory.Generate<INotificationRepository>();
            IEnumerable<Notification> notifications = await notificationRepository.GetNotificationAsync(queryNotificationCommand.Username, queryNotificationCommand.IsSeen, queryNotificationCommand.Skip, queryNotificationCommand.Take, cancellationToken);
            List<NotificationResponse> notificationResponses = notifications.Select(x => new NotificationResponse(x.CorrelationId.ToString(), x.Title.ToString(), x.Content.ToString(), x.IsSeen, x.OperationDate))
                                                                            .ToList();

            return notificationResponses;
        }
    }
}