import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import { UserSkillService } from '../../../core/services/user-skill.service';
import { User } from '../../../core/models/user.model';
import { UserSkill } from '../../../core/models/user-skill.model';

@Component({
  selector: 'app-profile-view',
  standalone: true,
  imports: [RouterLink, NgClass],
  templateUrl: './profile-view.component.html'
})
export class ProfileViewComponent implements OnInit {
  private auth = inject(AuthService);
  private userService = inject(UserService);
  private userSkillService = inject(UserSkillService);

  user = signal<User | null>(null);
  userSkills = signal<UserSkill[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  // Group skills by status value (0=Learning, 1=Proficient, 2=Expert)
  skillGroups = computed(() => [
    { label: 'Learning',   badgeClass: 'badge-learning',   skills: this.userSkills().filter(s => Number(s.status) === 0) },
    { label: 'Proficient', badgeClass: 'badge-proficient', skills: this.userSkills().filter(s => Number(s.status) === 1) },
    { label: 'Expert',     badgeClass: 'badge-expert',     skills: this.userSkills().filter(s => Number(s.status) === 2) },
  ].filter(g => g.skills.length > 0));

  initials = computed(() => {
    const name = this.user()?.fullName ?? '';
    return name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2);
  });

  async ngOnInit() {
    this.loading.set(true);
    try {
      const authUser = await this.auth.getUser();
      if (!authUser) { this.error.set('Not signed in.'); this.loading.set(false); return; }

      this.userService.getById(authUser.id).subscribe({
        next: u => { this.user.set(u); this.loading.set(false); },
        error: () => { this.error.set('Could not load profile.'); this.loading.set(false); }
      });

      this.userSkillService.getByUserId(authUser.id).subscribe({
        next: us => this.userSkills.set(us)
      });
    } catch {
      this.error.set('Unexpected error loading profile.');
      this.loading.set(false);
    }
  }
}
