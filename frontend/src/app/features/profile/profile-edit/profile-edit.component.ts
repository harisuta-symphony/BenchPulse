import { Component, OnInit, signal, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgClass } from '@angular/common';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorAlertComponent } from '../../../shared/components/error-alert/error-alert.component';
import { skillStatusClass, skillStatusLabel } from '../../../shared/utils/badge.utils';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import { SkillService } from '../../../core/services/skill.service';
import { UserSkillService } from '../../../core/services/user-skill.service';
import { User } from '../../../core/models/user.model';
import { Skill } from '../../../core/models/skill.model';
import { UserSkill, UpdateUserSkill } from '../../../core/models/user-skill.model';

@Component({
  selector: 'app-profile-edit',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, NgClass, LoadingSpinnerComponent, ErrorAlertComponent],
  templateUrl: './profile-edit.component.html'
})
export class ProfileEditComponent implements OnInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private userService = inject(UserService);
  private skillService = inject(SkillService);
  private userSkillService = inject(UserSkillService);
  private router = inject(Router);

  userId = signal<string | null>(null);
  loading = signal(false);
  saving = signal(false);
  error = signal<string | null>(null);

  userSkills = signal<UserSkill[]>([]);
  skillSearchQuery = signal('');
  skillSearchResults = signal<Skill[]>([]);
  searchingSkills = signal(false);

  form: FormGroup = this.fb.group({
    fullName: [''],
    department: [''],
    bio: ['']
  });

  async ngOnInit() {
    this.loading.set(true);
    try {
      const authUser = await this.auth.getUser();
      if (!authUser) { this.error.set('Not signed in.'); this.loading.set(false); return; }

      this.userId.set(authUser.id);

      this.userService.getById(authUser.id).subscribe({
        next: (u: User) => {
          this.form.patchValue({
            fullName: u.fullName,
            department: u.department ?? '',
            bio: u.bio ?? ''
          });
          this.loading.set(false);
        },
        error: () => { this.error.set('Could not load profile.'); this.loading.set(false); }
      });

      this.userSkillService.getByUserId(authUser.id).subscribe({
        next: (us: UserSkill[]) => this.userSkills.set(us)
      });
    } catch {
      this.error.set('Unexpected error loading profile.');
      this.loading.set(false);
    }
  }

  save() {
    const id = this.userId();
    if (!id) return;
    this.saving.set(true);
    this.userService.update(id, this.form.value).subscribe({
      next: () => { this.saving.set(false); this.router.navigate(['/profile']); },
      error: () => { this.saving.set(false); this.error.set('Failed to save profile.'); }
    });
  }

  searchSkills() {
    const q = this.skillSearchQuery().trim();
    if (!q) { this.skillSearchResults.set([]); return; }
    this.searchingSkills.set(true);
    this.skillService.search(q).subscribe({
      next: (skills: Skill[]) => {
        const ownedIds = new Set(this.userSkills().map(us => us.skillId));
        this.skillSearchResults.set(skills.filter(s => !ownedIds.has(s.id)));
        this.searchingSkills.set(false);
      },
      error: () => this.searchingSkills.set(false)
    });
  }

  addSkill(skill: Skill) {
    const id = this.userId();
    if (!id) return;
    this.userSkillService.create({ userId: id, skillId: skill.id, status: 0, isTeachable: false }).subscribe({
      next: (us: UserSkill) => {
        this.userSkills.update(list => [...list, us]);
        this.skillSearchResults.update(results => results.filter(s => s.id !== skill.id));
      }
    });
  }

  removeSkill(userSkill: UserSkill) {
    this.userSkillService.delete(userSkill.id).subscribe({
      next: () => this.userSkills.update(list => list.filter(us => us.id !== userSkill.id))
    });
  }

  toggleTeachable(userSkill: UserSkill) {
    const dto: UpdateUserSkill = { isTeachable: !userSkill.isTeachable };
    this.userSkillService.update(userSkill.id, dto).subscribe({
      next: (updated: UserSkill) => {
        this.userSkills.update(list => list.map(us => us.id === updated.id ? updated : us));
      }
    });
  }

  setStatus(userSkill: UserSkill, status: number) {
    const dto: UpdateUserSkill = { status };
    this.userSkillService.update(userSkill.id, dto).subscribe({
      next: (updated: UserSkill) => {
        this.userSkills.update(list => list.map(us => us.id === updated.id ? updated : us));
      }
    });
  }

  toNumber = Number;
  readonly skillStatusClass = skillStatusClass;
  readonly skillStatusLabel = skillStatusLabel;
}
