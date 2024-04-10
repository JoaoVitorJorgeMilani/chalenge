import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable, catchError, map, of } from 'rxjs';
import { SignalRService } from 'src/app/main/user/stream/signalr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  loggedUser: any;
  isLoggedIn = false;
  baseUrl = environment.apiUrl;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  constructor(private httpClient: HttpClient, private signalRService: SignalRService) { }

  isAuthenticated(): boolean {
    return this.isLoggedIn;
  }



  login(user: any): Observable<any> {
    return this.httpClient.get<string>(`${this.baseUrl}/user/login`, { params: { userName: user.name } })
      .pipe(
        map((userDto) => {
          this.registerUserSignalR(userDto);
          this.isLoggedIn = true;
          this.loggedUser = userDto;
          return true;          
        }),
        catchError(error => {
          this.isLoggedIn = false;
          return error;
        })
      );
  }

  registerUserSignalR(user: any) {
    this.signalRService.connectToHub(user.encryptedId);
  }

  getLogedUser(){
    return this.loggedUser;
  }
}
