import { Component, OnInit, signal, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { BookingService } from '../../../core/services/booking.service';
import { UserService } from '../../../core/services/user.service';
import { SkillService } from '../../../core/services/skill.service';
import { User } from '../../../core/models/user.model';
import { Skill } from '../../../core/models/skill.model';

@Component({
  selector: 'app-booking-form',
  standalone: true,
  imports: [FormsModule, RouterLink, NgClass],
  templateUrl: './booking-form.component.html'
})
export class BookingFormComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private auth = inject(AuthService);
  private bookingService = inject(BookingService);
  private userService = inject(UserService);
  private skillService = inject(SkillService);

  provider = signal<User | null>(null);
  skill = signal<Skill | null>(null);
  loading = signal(false);
  submitting = signal(false);
  error = signal<string | null>(null);

  readonly durations = [15, 30, 45, 60];

  scheduledAt = signal('');
  selectedDuration = signal(30);
  message = signal('');

  private requesterId = '';

  async ngOnInit() {
    const providerId = this.route.snapshot.queryParamMap.get('providerId') ?? '';
    const skillId = this.route.snapshot.queryParamMap.get('skillId') ?? '';

    this.loading.set(true);

    const authUser = await this.auth.getUser();
    if (!authUser) { this.error.set('Not signed in.'); this.loading.set(false); return; }
    this.requesterId = authUser.id;

    // Load provider and skill in parallel
    let pending = 2;
    const done = () => { if (--pending === 0) this.loading.set(false); };

    if (providerId) {
      this.userService.getById(providerId).subscribe({
        next: u => { this.provider.set(u); done(); },
        error: () => { this.error.set('Could not load provider details.'); done(); }
      });
    } else { done(); }

    if (skillId) {
      this.skillService.getById(skillId).subscribe({
        next: s => { this.skill.set(s); done(); },
        error: () => { this.error.set('Could not load skill details.'); done(); }
      });
    } else { done(); }
  }

  submit() {
    const providerId = this.provider()?.id;
    const skillId = this.skill()?.id;
    if (!providerId || !skillId || !this.scheduledAt()) return;

    this.submitting.set(true);
    this.bookingService.create({
      requesterId: this.requesterId,
      providerId,
      skillId,
      scheduledAt: new Date(this.scheduledAt()).toISOString(),
      durationMinutes: this.selectedDuration(),
      message: this.message() || undefined
    }).subscribe({
      next: () => { this.submitting.set(false); this.router.navigate(['/bookings']); },
      error: () => { this.submitting.set(false); this.error.set('Failed to create booking. Please try again.'); }
    });
  }
}
