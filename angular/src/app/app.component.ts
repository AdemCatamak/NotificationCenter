import { Component } from '@angular/core';
import { AppAccessTokenService } from './app-access-token/app-access-token.service';
import { AppNotification } from './notification-list/app-notification';
import { AppNotificationSeenStatusChangedInfo } from './notification-list/app-notification-seen-status-changed-response';
import { AppNotificationService } from './notification-list/app-notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'angular';

  username = '';
  notificationCollection: AppNotification[] = [];

  constructor(
    private notificationService: AppNotificationService,
    private accessTokenService: AppAccessTokenService
  ) {
    this.notificationCollection = new Array<AppNotification>();

    this.notificationService.NotificationReceived.subscribe(
      (data: AppNotification) => {
        this.notificationCollection.push(data);
        this.notificationCollection.sort((a, b) => {
          let date1 = new Date(a.operationDate);
          let date2 = new Date(b.operationDate);
          return date2.getTime() - date1.getTime();
        });
        this.notificationCollection = this.notificationCollection.splice(0, 5);
      }
    );

    this.notificationService.NotificationReceived.subscribe(
      (data: AppNotificationSeenStatusChangedInfo) => {

        console.log(data);

        this.notificationCollection.forEach((x) => {
          if (x.correlationId == data.correlationId) {
            x.isSeen = data.isSeen;
          }
        });
      }
    );
  }

  ChangeUsername() {
    if (this.username == null || this.username == undefined) {
      this.username = '';
    }

    if (this.username != '') {
      this.accessTokenService.GetAccessToken(this.username).subscribe(
        (res) => {
          this.GetNotifications(res.username, res.value);
        },
        (er) => {
          console.log(er);
        }
      );
    }
  }

  private GetNotifications(username: string, accessToken: string) {
    this.notificationService.GetNotifications(username, accessToken).subscribe(
      (res) => {
        this.notificationCollection = res;
        this.StartObserveNotifications(accessToken);
      },
      (er) => {
        if (er.status == 404) {
          this.notificationCollection = [];
          this.StartObserveNotifications(accessToken);
        } else {
          console.error(er);
        }
      }
    );
  }

  private StartObserveNotifications(accessToken: string) {
    this.notificationService.ObserveNotifications(accessToken);
  }
}
