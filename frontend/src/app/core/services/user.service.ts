import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, CreateUser } from '../models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/users`;

  getAll(): Observable<User[]> { return this.http.get<User[]>(this.base); }
  getById(id: string): Observable<User> { return this.http.get<User>(`${this.base}/${id}`); }
  create(dto: CreateUser): Observable<User> { return this.http.post<User>(this.base, dto); }
  update(id: string, dto: Partial<User>): Observable<User> { return this.http.put<User>(`${this.base}/${id}`, dto); }
  delete(id: string): Observable<void> { return this.http.delete<void>(`${this.base}/${id}`); }
}