import { Component } from '@angular/core';

@Component({
  selector: 'app-top-bar',
  imports: [],
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.scss',
})
export class TopBarComponent {
  userImageUrl = 'assets/profile.jpg'; // Replace with real user image
  userEmail = 'user@example.com'; // Replace with dynamic email
}
