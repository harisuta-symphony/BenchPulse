import { Component, OnInit, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SkillService } from '../../../core/services/skill.service';
import { Skill } from '../../../core/models/skill.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorAlertComponent } from '../../../shared/components/error-alert/error-alert.component';

@Component({
  selector: 'app-skill-search',
  standalone: true,
  imports: [FormsModule, RouterLink, LoadingSpinnerComponent, ErrorAlertComponent],
  templateUrl: './skill-search.component.html'
})
export class SkillSearchComponent implements OnInit {
  private skillService = inject(SkillService);

  query = signal('');
  skills = signal<Skill[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit() { this.loadAll(); }

  loadAll() {
    this.loading.set(true);
    this.skillService.getAll().subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => { this.error.set('Could not load skills.'); this.loading.set(false); }
    });
  }

  search() {
    if (!this.query()) { this.loadAll(); return; }
    this.loading.set(true);
    this.skillService.search(this.query()).subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => { this.error.set('Search failed. Please try again.'); this.loading.set(false); }
    });
  }
}