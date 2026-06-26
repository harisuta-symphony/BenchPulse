import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserSkill, CreateUserSkill, UpdateUserSkill} from '../models/user-skill.model';
import {environment} from '../../../environments/environment';

@Injectable({ providedIn: 'root'})
export class UserSkillService {
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}/user-skills`;

    getByUserId(userId: string): Observable<UserSkill[]> {return this.http.get<UserSkill[]>(`${this.base}/user/${userId}`)}
    getBySkillId(skillId: string): Observable<UserSkill[]> {return this.http.get<UserSkill[]>(`${this.base}/skill/${skillId}`)}
    getById(id: string): Observable<UserSkill> {return this.http.get<UserSkill>(`${this.base}/${id}`)}
    create(dto: CreateUserSkill): Observable<UserSkill> {return this.http.post<UserSkill>(`${this.base}`, dto)}
    update(id: string, dto: UpdateUserSkill): Observable<UserSkill> { return this.http.put<UserSkill>(`${this.base}/${id}`, dto) }
    delete(id: string): Observable<void> { return this.http.delete<void>(`${this.base}/${id}`) }
}