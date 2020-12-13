import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NotificationListComponent } from './notification-list/notification-list.component';
import { AppNotificationService } from './notification-list/app-notification.service';

@NgModule({
  declarations: [AppComponent, NotificationListComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule, AppRoutingModule],
  providers: [AppNotificationService],
  bootstrap: [AppComponent],
})
export class AppModule {}
