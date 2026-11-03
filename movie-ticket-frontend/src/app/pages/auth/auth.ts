
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [CommonModule, FormsModule],
  templateUrl: './auth.html',
  styleUrl: './auth.css'
})
export class Auth {

  isLogin = true;
  showPassword = false;
  isLoading = false;

  fullName = '';
  email = '';
  password = '';

  message = '';
  errorMessage = '';

 constructor(
  private authService: AuthService,
  private router: Router
) { }

  switchMode(): void {
    this.isLogin = !this.isLogin;
    this.clearMessages();
    this.password = '';
  }

  clearMessages(): void {
    this.message = '';
    this.errorMessage = '';
  }

  submitForm(): void {
    this.clearMessages();
    this.isLoading = true;

    if (this.isLogin) {
      const loginData = {
        email: this.email,
        password: this.password
      };

      this.authService.login(loginData).subscribe({
       next: (response) => {
  localStorage.setItem('token', response.token);
  localStorage.setItem('customerEmail', this.email);

  this.router.navigate(['/home']);
  this.isLoading = false;
},
        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Login failed. Please check your details.';
          this.isLoading = false;
        }
      });

    } else {
      const registerData = {
        fullName: this.fullName,
        email: this.email,
        password: this.password,
        role: 'Customer'
      };

      this.authService.register(registerData).subscribe({
        next: (response) => {
          this.message =
            response.message || 'Registration successful! Please login.';
          this.isLoading = false;
          this.isLogin = true;
          this.password = '';
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Registration failed. Please try again.';
          this.isLoading = false;
        }
      });
    }
  }
}
