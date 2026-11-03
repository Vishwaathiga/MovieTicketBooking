
import { Routes } from '@angular/router';
import { Auth } from './pages/auth/auth';
import { Home } from './pages/home/home';
import { SeatSelection } from './pages/seat-selection/seat-selection';
import { MyBookings } from './pages/my-bookings/my-bookings';

export const routes: Routes = [
  {
    path: 'auth',
    component: Auth
  },
  {
    path: 'home',
    component: Home
  },
  {
    path: 'booking/:showId',
    component: SeatSelection
  },
  {
    path: 'my-bookings',
    component: MyBookings
  },
  {
    path: '',
    redirectTo: 'auth',
    pathMatch: 'full'
  }
];
