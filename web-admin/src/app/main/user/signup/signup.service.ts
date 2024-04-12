import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignupService {

  baseUrl = environment.apiUrl;
  
  constructor(private httpClient: HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  add(user: any): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/user/add`, user, this.httpOptions);   
  }

  
}
