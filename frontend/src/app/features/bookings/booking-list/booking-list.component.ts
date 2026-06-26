import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorAlertComponent } from '../../../shared/components/error-alert/error-alert.component';
import { bookingStatusClass, bookingStatusLabel } from '../../../shared/utils/badge.utils';
import { AuthService } from '../../../core/services/auth.service';
import { BookingService } from '../../../core/services/booking.service';
import { Booking } from '../../../core/models/booking.model';

@Component({
  selector: 'app-booking-list',
  standalone: true,
  imports: [DatePipe, NgClass, RouterLink, LoadingSpinnerComponent, ErrorAlertComponent],
  templateUrl: './booking-list.component.html'
})
export class BookingListComponent implements OnInit {
  private auth = inject(AuthService);
  private bookingService = inject(BookingService);

  activeTab = signal<'sent' | 'incoming'>('sent');
  sentBookings = signal<Booking[]>([]);
  incomingBookings = signal<Booking[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  displayedBookings = computed(() =>
    this.activeTab() === 'sent' ? this.sentBookings() : this.incomingBookings()
  );

  async ngOnInit() {
    this.loading.set(true);
    const authUser = await this.auth.getUser();
    if (!authUser) { this.error.set('Not signed in.'); this.loading.set(false); return; }

    let pending = 2;
    const done = () => { if (--pending === 0) this.loading.set(false); };

    this.bookingService.getByRequesterId(authUser.id).subscribe({
      next: b => { this.sentBookings.set(b); done(); },
      error: () => { this.error.set('Could not load bookings.'); done(); }
    });

    this.bookingService.getByProviderId(authUser.id).subscribe({
      next: b => { this.incomingBookings.set(b); done(); },
      error: () => { this.error.set('Could not load bookings.'); done(); }
    });
  }

  confirm(id: string) {
    this.bookingService.updateStatus(id, { status: '1' }).subscribe({
      next: updated => this.replaceIncoming(updated)
    });
  }

  cancel(id: string) {
    this.bookingService.updateStatus(id, { status: '2' }).subscribe({
      next: updated => {
        this.replaceIncoming(updated);
        this.replaceSent(updated);
      }
    });
  }

  private replaceIncoming(updated: Booking) {
    this.incomingBookings.update(list => list.map(b => b.id === updated.id ? updated : b));
  }

  private replaceSent(updated: Booking) {
    this.sentBookings.update(list => list.map(b => b.id === updated.id ? updated : b));
  }

  isPending(status: string): boolean {
    return Number(status) === 0;
  }

  readonly bookingStatusClass = bookingStatusClass;
  readonly bookingStatusLabel = bookingStatusLabel;
}
