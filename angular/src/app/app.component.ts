import { Component } from '@angular/core';
import { AppAccessTokenService } from './app-access-token/app-access-token.service';
import { AppNotification } from './notification-list/app-notification';
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
  }

  ChangeUsername() {
    console.log('username :' + this.username);
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
    console.log('works');
  }
}
