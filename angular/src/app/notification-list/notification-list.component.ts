import { Component, Input, OnInit } from '@angular/core';
import { AppNotification } from './app-notification';
import { AppNotificationService } from './app-notification.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css'],
})
export class NotificationListComponent implements OnInit {
  @Input() notifications: AppNotification[] = [];

  constructor(private notificationService: AppNotificationService) {}

  ngOnInit(): void {
    if (this.notifications == null || this.notifications == undefined) {
      this.notifications = new Array<AppNotification>();
    }
  }

  ChangeIsSeen(notification: AppNotification) {
   this.notificationService.ChangeNotificationSeenStatus(notification.correlationId, !notification.isSeen)
   .subscribe(()=>{
   }, 
   er=>{
     console.log(er);
   })
  }

}
