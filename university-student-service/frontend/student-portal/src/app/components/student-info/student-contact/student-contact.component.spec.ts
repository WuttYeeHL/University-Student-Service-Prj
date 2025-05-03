import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentContactComponent } from './student-contact.component';

describe('StudentInfoComponent', () => {
  let component: StudentContactComponent;
  let fixture: ComponentFixture<StudentContactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentContactComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentContactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
