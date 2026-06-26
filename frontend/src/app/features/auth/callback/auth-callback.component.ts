import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  templateUrl: './auth-callback.component.html'
})
export class AuthCallbackComponent implements OnInit {
  private auth = inject(AuthService);
  private router = inject(Router);
  private userService = inject(UserService);

  async ngOnInit() {
    const supabaseUser = await this.auth.getUser();
    if (!supabaseUser) {
      this.router.navigate(['/login']);
      return;
    }

    try {
      await firstValueFrom(this.userService.getById(supabaseUser.id));
    } catch {
      // User record doesn't exist yet — create it using the Supabase ID
      const name = supabaseUser.user_metadata?.['full_name']
        ?? supabaseUser.user_metadata?.['name']
        ?? supabaseUser.email?.split('@')[0]
        ?? 'New User';

      await firstValueFrom(this.userService.create({
        id: supabaseUser.id,
        fullName: name,
        email: supabaseUser.email ?? '',
      }));
    }

    this.router.navigate(['/']);
  }
}
