import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { AppNotification } from './app-notification';
import * as signalR from '@aspnet/signalr';

@Injectable()
export class AppNotificationService {
  @Output()
  NewNotificationArrived: EventEmitter<AppNotification> = new EventEmitter<AppNotification>();

  @Output()
  NotificationStatusChanged: EventEmitter<AppNotification> = new EventEmitter<AppNotification>();

  private hubConnection: signalR.HubConnection | null = null;

  constructor(private httpClient: HttpClient) {}

  public GetNotifications(
    username: string,
    accessToken: string
  ): Observable<AppNotification[]> {
    var reqHeader = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken,
    });

    return this.httpClient.get<AppNotification[]>(
      'http://localhost:5000/users/' + username + '/notifications?take=5',
      { headers: reqHeader }
    );
  }

  public ObserveNotifications(accessToken: string) {
    console.log('ObserveNotifications' + accessToken);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/auth-hubs/notification-hub', {
        accessTokenFactory: () => accessToken,
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.registerOnServerEvents();
      })
      .catch((err) => {
        console.log(err);
      });
  }

  private registerOnServerEvents(): void {
    if (this.hubConnection == null) {
      return;
    }

    this.hubConnection.on('GetNotification', (data: AppNotification) => {
      console.log('GetNotification');
      console.log(data);

      this.NewNotificationArrived.emit(data);
    });

    this.hubConnection.on(
      'UpdateNotificationSeenStatus',
      (data: AppNotification) => {
        console.log('UpdateNotificationSeenStatus');
        console.log(data);

        this.NotificationStatusChanged.emit(data);
      }
    );
  }
}
