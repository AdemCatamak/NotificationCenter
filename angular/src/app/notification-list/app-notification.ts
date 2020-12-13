export class AppNotification {
  correlationId: string = '';
  title: string = '';
  content: string = '';
  isSeen: boolean = false;
  operationDate: Date | null = null;
}
