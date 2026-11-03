
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookingService } from '../../services/booking';

@Component({
  selector: 'app-my-bookings',
  imports: [CommonModule],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css',
})
export class MyBookings implements OnInit {

  bookings: any[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  cancellingBookingId: number | null = null;

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.bookingService.getMyBookings().subscribe({
      next: (response) => {
        this.bookings = response;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to load your bookings.';
        this.isLoading = false;
      }
    });
  }

  cancelBooking(bookingId: number): void {
    const confirmed = confirm(
      'Are you sure you want to cancel this booking?'
    );

    if (!confirmed) {
      return;
    }

    this.cancellingBookingId = bookingId;
    this.errorMessage = '';
    this.successMessage = '';

    this.bookingService.cancelBooking(bookingId).subscribe({
      next: () => {
        this.successMessage = 'Booking cancelled successfully.';
        this.cancellingBookingId = null;
        this.loadBookings();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to cancel this booking.';
        this.cancellingBookingId = null;
      }
    });
  }
}
