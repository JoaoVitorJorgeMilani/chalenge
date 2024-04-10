import { HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SseService {

  private eventSource: EventSource | null = null;
  private hubConnection: any;


  constructor() {
        
  }

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
    this.hubConnection.on('Streaming', (message: string) => {
      console.log('Evento do hub SignalR recebido:', message);
    });
  }
}
