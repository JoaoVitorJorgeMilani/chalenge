import { EventEmitter, Injectable, Output } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  private hubConnection: any;
  @Output() streaming: EventEmitter<string> = new EventEmitter<string>();

  constructor() {}

  public connectToHub(encryptedId : string) {
    encryptedId = encodeURIComponent(encryptedId);
    
    this.hubConnection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Debug)
    .withUrl(`http://localhost:5000/api/mainhub?encryptedUserId=${encryptedId}`,
    )      
    .build();
    
    this.hubConnection.start()
    .then(() => {
      this.listenForEvents();
    })
    .catch((error: any) => console.log(error));
  }

  public listenForEvents() {
    this.hubConnection.on('Streaming', (message: any) => {
      this.streaming.emit(message);
    });
  }
}
