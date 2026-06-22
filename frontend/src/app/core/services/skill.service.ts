import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Skill, CreateSkill } from '../models/skill.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root'})
export class SkillService {
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}/skills`;

    getAll(): Observable<Skill[]> {return this.http.get<Skill[]>(this.base);}
    search(query: string): Observable<Skill[]> { return this.http.get<Skill[]>(`${this.base}/search?q=${query}`) }
    getById(id: string): Observable<Skill> {return this.http.get<Skill>(`${this.base}/${id}`)}
    create(dto: CreateSkill): Observable<Skill> {return this.http.post<Skill>(this.base, dto)}
    delete(id: string): Observable<void> { return this.http.delete<void>(`${this.base}/${id}`) }
}