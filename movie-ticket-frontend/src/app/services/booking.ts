
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  private apiUrl = 'https://localhost:7162/api';

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getSeats(showId: number) {
    return this.http.get<any[]>(
      `${this.apiUrl}/showseats/${showId}`
    );
  }

  createBooking(bookingData: any) {
    return this.http.post(
      `${this.apiUrl}/bookings`,
      bookingData,
      { headers: this.getHeaders() }
    );
  }

  getMyBookings() {
    const email = localStorage.getItem('customerEmail');

    return this.http.get<any[]>(
      `${this.apiUrl}/bookings/customer/${encodeURIComponent(email || '')}`,
      { headers: this.getHeaders() }
    );
  }

  cancelBooking(bookingId: number) {
    return this.http.put(
      `${this.apiUrl}/bookings/${bookingId}/cancel`,
      {},
      { headers: this.getHeaders() }
    );
  }
}
