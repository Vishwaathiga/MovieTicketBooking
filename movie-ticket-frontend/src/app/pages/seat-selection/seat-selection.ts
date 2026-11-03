
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../services/booking';
import { ShowService } from '../../services/show';

@Component({
  selector: 'app-seat-selection',
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.css'
})
export class SeatSelection implements OnInit {

  showId = 0;
  seats: any[] = [];
  selectedSeats: any[] = [];

  movieTitle = 'Movie';
  ticketPrice = 0;
  customerName = '';

  isLoading = true;
  isBooking = false;

  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookingService: BookingService,
    private showService: ShowService
  ) { }

  ngOnInit(): void {
    this.showId = Number(this.route.snapshot.paramMap.get('showId'));

    const storedName = localStorage.getItem('customerName');
    this.customerName = storedName || '';

    this.loadShowDetails();
    this.loadSeats();
  }

  loadShowDetails(): void {
    this.showService.getShows().subscribe({
      next: (data: any[]) => {
        const selectedShow = data.find(
          show => show.showId === this.showId
        );

        if (selectedShow) {
          this.movieTitle = selectedShow.movieTitle;
          this.ticketPrice = selectedShow.ticketPrice;
        }
      },
      error: (error) => {
        console.error('Unable to load show details:', error);
      }
    });
  }

  loadSeats(): void {
    this.bookingService.getSeats(this.showId).subscribe({
      next: (data) => {
        this.seats = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load seats. Please try again.';
        this.isLoading = false;
      }
    });
  }

  get rows(): string[] {
    return [...new Set(
      this.seats.map(seat => seat.seat.rowName)
    )];
  }

  getSeatsForRow(rowName: string): any[] {
    return this.seats.filter(
      seat => seat.seat.rowName === rowName
    );
  }

  isSelected(showSeatId: number): boolean {
    return this.selectedSeats.some(
      seat => seat.showSeatId === showSeatId
    );
  }

  toggleSeat(seat: any): void {
    if (seat.status !== 'Available') {
      return;
    }

    if (this.isSelected(seat.showSeatId)) {
      this.selectedSeats = this.selectedSeats.filter(
        selected => selected.showSeatId !== seat.showSeatId
      );
    } else {
      this.selectedSeats.push(seat);
    }

    this.errorMessage = '';
  }

  get totalAmount(): number {
    return this.selectedSeats.length * this.ticketPrice;
  }

  bookTickets(): void {
    if (this.selectedSeats.length === 0) {
      this.errorMessage = 'Please select at least one seat.';
      return;
    }

    if (!this.customerName.trim()) {
      this.errorMessage = 'Please enter your name.';
      return;
    }

    this.isBooking = true;
    this.errorMessage = '';

    const bookingData = {
      showId: this.showId,
      customerName: this.customerName,
      showSeatIds: this.selectedSeats.map(
        seat => seat.showSeatId
      )
    };

    this.bookingService.createBooking(bookingData).subscribe({
      next: (response: any) => {
        this.successMessage = 'Booking completed successfully!';
        this.isBooking = false;

        setTimeout(() => {
          this.router.navigate(['/home']);
        }, 2000);
      },
      error: (error) => {
        console.error(error);

        this.errorMessage =
          error.error?.message ||
          'Booking failed. Please try again.';

        this.isBooking = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }
}
