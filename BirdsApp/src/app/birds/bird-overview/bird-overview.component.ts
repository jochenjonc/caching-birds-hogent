import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { Bird } from '../bird';
import { BirdService } from '../bird.service';

@Component({
  selector: 'app-bird-overview',
  templateUrl: './bird-overview.component.html',
  styleUrls: ['./bird-overview.component.scss']
})
export class BirdOverviewComponent {
  public displayedColumns: string[] = ['id', 'name'];
  public birds$: Observable<Bird[]>;

  constructor(private birdService: BirdService) {
    this.birds$ = this.birdService.getBirds();
  }
}
