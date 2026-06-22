import { Component, OnInit, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { SkillService } from '../../../core/services/skill.service';
import { Skill } from '../../../core/models/skill.model';

@Component({
  selector: 'app-skill-search',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './skill-search.component.html'
})
export class SkillSearchComponent implements OnInit {
  private skillService = inject(SkillService);

  query = signal('');
  skills = signal<Skill[]>([]);
  loading = signal(false);

  ngOnInit() { this.loadAll(); }

  loadAll() {
    this.loading.set(true);
    this.skillService.getAll().subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  search() {
    if (!this.query()) { this.loadAll(); return; }
    this.loading.set(true);
    this.skillService.search(this.query()).subscribe({
      next: s => { this.skills.set(s); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }
}