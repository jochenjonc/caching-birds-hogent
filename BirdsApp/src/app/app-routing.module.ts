import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ObservationOverviewComponent } from './observations/observation-overview/observation-overview.component';
import { BirdOverviewComponent } from './birds/bird-overview/bird-overview.component';


const routes: Routes = [
  { path: '', redirectTo: '/observations', pathMatch: 'full' },
  { path: 'observations', component: ObservationOverviewComponent },
  { path: 'birds', component: BirdOverviewComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
