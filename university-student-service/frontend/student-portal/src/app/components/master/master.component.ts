import {
  Component,
  computed,
  HostListener,
  Inject,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { LayoutComponent } from '../layout/layout.component';

@Component({
  selector: 'app-master',
  imports: [RouterOutlet, CommonModule, LayoutComponent],
  templateUrl: './master.component.html',
  styleUrl: './master.component.scss',
})
export class MasterComponent implements OnInit {
  isLeftSidebarCollapsed = signal<boolean>(false);
  screenWidth = signal<number>(0);

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  sizeClass = computed(() => {
    const collapsed = this.isLeftSidebarCollapsed();
    const width = this.screenWidth();

    return collapsed ? '' : width > 768 ? 'body-trimmed' : 'body-md-screen';
  });

  changeIsLeftSidebarCollapsed(isLeftSidebarCollapsed: boolean): void {
    this.isLeftSidebarCollapsed.set(isLeftSidebarCollapsed);
  }

  @HostListener('window:resize')
  onResize() {
    const width = window.innerWidth;
    this.screenWidth.set(width);

    if (width < 768) {
      this.isLeftSidebarCollapsed.set(true); // Collapse on small screens
    } else {
      this.isLeftSidebarCollapsed.set(false); // Expand on larger screens
    }
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.screenWidth.set(window.innerWidth);
      this.isLeftSidebarCollapsed.set(this.screenWidth() < 768);
    }
  }
}
