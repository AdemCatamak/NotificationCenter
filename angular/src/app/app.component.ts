import { Component } from '@angular/core';
import { AppAccessTokenService } from './app-access-token/app-access-token.service';
import { GlobalVariables } from './global-variables';
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

  notificationCollection: AppNotification[] = [];

  constructor(
    private notificationService: AppNotificationService,
    private accessTokenService: AppAccessTokenService,
    public globalVariables : GlobalVariables
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

    this.notificationService.NotificationSeenStatusChanged.subscribe(
      (data: AppNotificationSeenStatusChangedInfo) => {
        this.notificationCollection.forEach((x) => {
          if (x.correlationId == data.correlationId) {
            x.isSeen = data.isSeen;
          }
        });
      }
    );
  }

  ChangeUsername() {
    if (this.globalVariables.Username == null || this.globalVariables.Username  == undefined) {
      this.globalVariables.Username  = '';
    }

    if (this.globalVariables.Username != '') {
      this.accessTokenService.GetAccessToken(this.globalVariables.Username ).subscribe(
        (res) => {
          this.globalVariables.AccessToken = res.value; 
          this.GetNotifications();
        },
        (er) => {
          console.log(er);
        }
      );
    }
  }

  private GetNotifications() {
    this.notificationService.GetNotifications().subscribe(
      (res) => {
        this.notificationCollection = res;
        this.StartObserveNotifications();
      },
      (er) => {
        if (er.status == 404) {
          this.notificationCollection = [];
          this.StartObserveNotifications();
        } else {
          console.error(er);
        }
      }
    );
  }

  private StartObserveNotifications() {
    this.notificationService.ObserveNotifications();
  }
}
