import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, inject, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';

const ANIMATION_TIMINGS = "400ms cubic-bezier(0.25, 0.8, 0.25, 1)";

@Component({
  selector: 'lib-alert',
  templateUrl: 'alert.component.html',
  animations: [
    trigger("fade", [
      state("fadeOut", style({ opacity: 0 })),
      state("fadeIn", style({ opacity: 1 })),
      transition("* => fadeIn", animate(ANIMATION_TIMINGS)),
      transition("* => fadeOut", animate(ANIMATION_TIMINGS)),
    ]),
    trigger("slideContent", [
      state(
        "void",
        style({ transform: "translate3d(0, 25%, 0) scale(0.9)", opacity: 0 })
      ),
      state("enter", style({ transform: "none", opacity: 1 })),
      state(
        "leave",
        style({ transform: "translate3d(0, 25%, 0)", opacity: 0 })
      ),
      transition("* => *", animate(ANIMATION_TIMINGS)),
    ]),
  ],
  imports: [MatIconModule]
})

export class AlertComponent implements OnInit {
  animationState: "void" | "enter" | "leave" = "enter";

  data = inject(MAT_SNACK_BAR_DATA);
  ref = inject(MatSnackBarRef);

  constructor() {
  }

  ngOnInit() { }
}
