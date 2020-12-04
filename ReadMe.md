
# NotificationCenter

This project has been prepared as a proof of concept to inform the user of system-wide events as soon as possible.

RabbitMq messaging system was used to monitor the events occurring throughout the system and MassTransit package was used to connect to RabbitMq.

Notifications were created from events that occurred and notifications are stored on MongoDb.

After the notifications are created, this information will be pushed to the client-side application via the SignalR package.

<img src="./Doc/Img/NotificationCenter.png" alt="Notification Center"/>

## __RUN__

__Way 1__

The project could be executed via _docker-compose_. In case of choosing this way to run the project, you can reach swagger screen via _http://localhost:5000_.

Note: Because the rabbit-mq needs more time to be ready compared to NotificationCenter-Api, it might take a while for you to reach the endpoints after `docker-compose up` command execution.

Note: If the `docker-compose up` command cannot be executed due to the port conflict, you should change the port information both from _docker-compose.yml_ and _appsettings.json_ file.

__Way 2__

If you want to execute the project without using docker, it is required that you set the connection strings inside the NotificationCenter/appsettings.dev.json file.

Changes to be made are:
1. NoSqlDbConfig -> NoSqlDbOptions section should be changed with the MongoDb connection information that you have.
2. MessageBrokerConfig -> MessageBrokerOptions -> HostName, VirtualHost, Username, Password values should be changed with the RabbitMq platform information that you have.
