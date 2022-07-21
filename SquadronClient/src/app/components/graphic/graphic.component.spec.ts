import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraficComponent } from './graphic.component';

describe('GraphicComponent', () => {
  let component: GraficComponent;
  let fixture: ComponentFixture<GraficComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GraficComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(GraficComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
