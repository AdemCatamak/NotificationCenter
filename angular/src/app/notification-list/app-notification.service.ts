import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { AppNotification } from './app-notification';
import * as signalR from '@aspnet/signalr';
import { AppNotificationSeenStatusChangedInfo } from './app-notification-seen-status-changed-response';
import { GlobalVariables } from '../global-variables';
import { HttpTransportType } from '@aspnet/signalr';

@Injectable()
export class AppNotificationService {
  @Output()
  NotificationReceived: EventEmitter<AppNotification> = new EventEmitter<AppNotification>();
  NotificationSeenStatusChanged: EventEmitter<AppNotificationSeenStatusChangedInfo> = new EventEmitter<AppNotificationSeenStatusChangedInfo>();

  private baseUrl = 'http://localhost:5000';

  private hubConnection: signalR.HubConnection | null = null;

  constructor(private httpClient: HttpClient,
    private globalVariables: GlobalVariables) { }

  public GetNotifications(): Observable<AppNotification[]> {
    var reqHeader = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + this.globalVariables.AccessToken,
    });

    return this.httpClient.get<AppNotification[]>(
      this.baseUrl + '/users/' + this.globalVariables.Username + '/notifications?take=5',
      { headers: reqHeader }
    );
  }

  public ChangeNotificationSeenStatus(correlationId: string, isSeen: boolean): Observable<any> {
    var reqHeader = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + this.globalVariables.AccessToken,
    });

    if (isSeen) {
      return this.httpClient.put(
        this.baseUrl + '/users/' + this.globalVariables.Username + '/notifications/' + correlationId + '/seen',
        {},
        { headers: reqHeader }
      );
    }
    else {
      return this.httpClient.delete(
        this.baseUrl + '/users/' + this.globalVariables.Username + '/notifications/' + correlationId + '/seen',
        { headers: reqHeader }
      );
    }
  }

  public ObserveNotifications() {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/auth-hubs/notification-hub', {
        accessTokenFactory: () => this.globalVariables.AccessToken,
        transport: HttpTransportType.LongPolling
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

    this.hubConnection.on('NotificationReceived', (data: AppNotification) => {
      this.NotificationReceived.emit(data);
    });

    this.hubConnection.on(
      'NotificationSeenStatusChanged',
      (data: AppNotificationSeenStatusChangedInfo) => {
        this.NotificationSeenStatusChanged.emit(data);
      }
    );
  }
}
