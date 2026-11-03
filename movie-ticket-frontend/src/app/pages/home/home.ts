
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShowService } from '../../services/show';
import { RouterLink, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

  shows: any[] = [];
  errorMessage = '';
  isLoading = true;

  constructor(
    private showService: ShowService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.showService.getShows().subscribe({
      next: (data) => {
        this.shows = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load shows. Please try again.';
        this.isLoading = false;
      }
    });
  }

  signOut(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('customerEmail');

    this.router.navigate(['/auth']);
  }
}
