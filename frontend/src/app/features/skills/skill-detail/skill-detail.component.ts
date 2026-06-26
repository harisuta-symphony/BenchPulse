import { Component, OnInit, signal, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SkillService } from '../../../core/services/skill.service';
import { UserSkillService } from '../../../core/services/user-skill.service';
import { Skill } from '../../../core/models/skill.model';
import { UserSkill } from '../../../core/models/user-skill.model';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorAlertComponent } from '../../../shared/components/error-alert/error-alert.component';
import { skillStatusClass, skillStatusLabel } from '../../../shared/utils/badge.utils';

@Component({
    selector: 'app-skill-detail',
    standalone: true,
    imports: [RouterLink, LoadingSpinnerComponent, ErrorAlertComponent],
    templateUrl: './skill-detail.component.html'
})
export class SkillDetailComponent implements OnInit {
    private route = inject(ActivatedRoute);
    private skillService = inject(SkillService);
    private userSkillService = inject(UserSkillService);

    skill = signal<Skill | null>(null);
    userSkills = signal<UserSkill[]>([]);
    loading = signal(false);
    error = signal<string | null>(null);

    ngOnInit() { this.load(); }

    load() {
        const id = this.route.snapshot.paramMap.get('id')!;
        this.loading.set(true);
        this.skillService.getById(id).subscribe({
            next: s => { this.skill.set(s); this.loading.set(false); },
            error: () => { this.error.set('Could not load skill details.'); this.loading.set(false); }
        });
        this.userSkillService.getBySkillId(id).subscribe({
            next: us => this.userSkills.set(us.filter(u => u.isTeachable)),
            error: () => this.error.set('Could not load peer list.')
        });
    }

    teachable() {
        return this.userSkills();
    }

    readonly skillStatusClass = skillStatusClass;
    readonly skillStatusLabel = skillStatusLabel;
}