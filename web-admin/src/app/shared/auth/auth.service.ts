import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable, catchError, map, of } from 'rxjs';
import { SseService } from 'src/app/main/user/sse/sse.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isLoggedIn = false;
  baseUrl = environment.apiUrl;
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  constructor(private httpClient: HttpClient, private sseService: SseService) { }

  isAuthenticated(): boolean {
    return this.isLoggedIn;
  }



  login(user: any): Observable<any> {
    return this.httpClient.get<string>(`${this.baseUrl}/user/login`, { params: { userName: user.name } })
      .pipe(
        map((userDto) => {
          console.log("DEBUGGING USER LOGGED");
          console.log(userDto);

          this.registerUserSSE(userDto);
          this.isLoggedIn = true;
          return true;          
        }),
        catchError(error => {
          this.isLoggedIn = false;
          return error;
        })
      );
  }

  registerUserSSE(user: any) {
    console.log("registering user SSE");
    this.sseService.connectToHub(user.encryptedId);

    // this.sseService.connectToSse(user.encryptedId).subscribe({
    //   next : data => {
    //     console.log("CONECTED");
    //     console.log(data);

    //   },
    //   error: error => {
    //     console.log("CONECTED ERROR");
    //     console.error('Error in SSE connection:', error);
    //   }
    // });
  }



}
