import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/shared/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  getAvailableOrders(): Observable<any> {
    return this.httpClient.get(`${environment.apiUrl}/order/available`, this.httpOptions);
  }

  acceptDelivery(order: any): Observable<any> {
    const params = new HttpParams()
      .set('orderId', order.encryptedId)
      .set('userId', this.authService.getLogedUser().encryptedId);

    return this.httpClient.put(`${environment.apiUrl}/order/accept`, null, { params });
  }

  declineDelivery(order: any): Observable<any> {
    const params = new HttpParams()
      .set('orderId', order.encryptedId)
      .set('userId', this.authService.getLogedUser().encryptedId);

    return this.httpClient.put(`${environment.apiUrl}/order/decline`, null, { params });
  }

  loadDelivering(orderId: string): Observable<any> {
    const params = new HttpParams()
      .set('encryptedOrderId', orderId);

    return this.httpClient.get(`${environment.apiUrl}/order/delivering`, { params });
  }
}
