import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/skills/skill-search/skill-search.component').then(m => m.SkillSearchComponent)
  },
  {
    path: 'skills/:id',
    loadComponent: () => import('./features/skills/skill-detail/skill-detail.component').then(m => m.SkillDetailComponent)
  },
  {
    path: 'profile/edit',
    loadComponent: () => import('./features/profile/profile-edit/profile-edit.component').then(m => m.ProfileEditComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./features/profile/profile-view/profile-view.component').then(m => m.ProfileViewComponent)
  },
  {
    path: 'bookings/new',
    loadComponent: () => import('./features/bookings/booking-form/booking-form.component').then(m => m.BookingFormComponent)
  },
  {
    path: 'bookings',
    loadComponent: () => import('./features/bookings/booking-list/booking-list.component').then(m => m.BookingListComponent)
  },
  { path: '**', redirectTo: '' }
];
