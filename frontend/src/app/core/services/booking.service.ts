import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Booking, CreateBooking, UpdateBookingStatus} from '../models/booking.model';
import {environment} from '../../../environments/environment';

@Injectable({providedIn: 'root'})
export class BookingService {
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}/bookings`;

    getByRequesterId(requesterId: string): Observable <Booking[]> {return this.http.get<Booking[]>(`${this.base}/requester/${requesterId}`)}
    getByProviderId(providerId: string): Observable<Booking[]> {return this.http.get<Booking[]>(`${this.base}/provider/${providerId}`)}
    getById(id: string): Observable<Booking> {return this.http.get<Booking>(`${this.base}/${id}`)}
    create(dto: CreateBooking): Observable<Booking> {return this.http.post<Booking>(`${this.base}`, dto)}
    updateStatus(id: string, dto: UpdateBookingStatus): Observable<Booking> { return this.http.patch<Booking>(`${this.base}/${id}/status`, dto) }
    delete(id: string): Observable<void> { return this.http.delete<void>(`${this.base}/${id}`) }
}