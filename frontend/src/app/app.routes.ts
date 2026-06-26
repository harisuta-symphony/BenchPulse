import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/skills/skill-search/skill-search.component').then(m => m.SkillSearchComponent),
    canActivate: [authGuard]
  },
  {
    path: 'skills/:id',
    loadComponent: () => import('./features/skills/skill-detail/skill-detail.component').then(m => m.SkillDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'profile/edit',
    loadComponent: () => import('./features/profile/profile-edit/profile-edit.component').then(m => m.ProfileEditComponent),
    canActivate: [authGuard]
  },
  {
    path: 'profile',
    loadComponent: () => import('./features/profile/profile-view/profile-view.component').then(m => m.ProfileViewComponent),
    canActivate: [authGuard]
  },
  {
    path: 'bookings/new',
    loadComponent: () => import('./features/bookings/booking-form/booking-form.component').then(m => m.BookingFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'bookings',
    loadComponent: () => import('./features/bookings/booking-list/booking-list.component').then(m => m.BookingListComponent),
    canActivate: [authGuard]
  },
  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'auth/callback', loadComponent: () => import('./features/auth/callback/auth-callback.component').then(m => m.AuthCallbackComponent) },
  { path: '**', redirectTo: '' }
];
