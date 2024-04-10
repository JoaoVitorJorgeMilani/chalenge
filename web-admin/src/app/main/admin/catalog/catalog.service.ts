import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, retry } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {

  baseUrl = environment.apiUrl;
  
  constructor(private httpClient: HttpClient) { }

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  add(bike: any): Observable<any> {
    return this.httpClient.post<any>(`${this.baseUrl}/catalog/add`, bike, this.httpOptions);   
  }

  getList(): Observable<any> {
    return this.httpClient.get(`${this.baseUrl}/catalog/list`, this.httpOptions);   
  }

  delete(bike: any): Observable<any> {
    var encryptedId = bike.encryptedId;
    return this.httpClient.delete(`${this.baseUrl}/catalog/delete`, { params: { encryptedId } });   
  }

  edit(bike: any): Observable<any> {
    const params = new HttpParams()
      .set('encryptedId', bike.encryptedId)
      .set('licensePlate', bike.licensePlate);

    return this.httpClient.put(`${this.baseUrl}/catalog/edit`, null, { params });

  }


}
