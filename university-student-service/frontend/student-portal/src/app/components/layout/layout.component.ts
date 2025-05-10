import { CommonModule } from '@angular/common';
import { Component, inject, input, output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../model/interface/user';
import { AuthService } from '../../services/guards/auth-guard.service';

@Component({
  selector: 'app-layout',
  imports: [RouterModule, CommonModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {
  user: User = {
    name: 'Florence',
    email: 'florence123@gmail.com',
    imageUrl: 'profilepic.png',
  };

  isLeftSidebarCollapsed = input.required<boolean>();
  changeIsLeftSidebarCollapsed = output<boolean>();
  router = inject(Router);
  items = [
    {
      routeLink: 'home',
      icon: 'fas fa-home',
      label: 'Home',
    },
    {
      routeLink: 'student-info',
      icon: 'fas fa-chalkboard-user',
      label: 'Student Information',
    },
    {
      routeLink: 'enrolment-info',
      icon: 'fas fa-chalkboard',
      label: 'Class Enrolments',
    },
    {
      routeLink: 'student-documents',
      icon: 'fas fa-book-open-reader',
      label: 'Student Documents',
    },
    {
      routeLink: 'course-info',
      icon: 'fas fa-book-open',
      label: 'Course Information',
    },
  ];

  toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed());
  }

  closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }

  constructor(private authService: AuthService) {}

  onLogout() {
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigateByUrl('/login');
      },
      error: (err) => {
        console.error('Logout failed', err);
      },
    });
  }
}
