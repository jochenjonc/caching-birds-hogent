import { Component } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import {
  exhaustMap,
  filter,
  iif,
  map,
  merge,
  mergeMap,
  Observable,
  startWith,
  Subject,
  switchMap,
} from 'rxjs';

import { Observation } from '../observation';
import {
  IObservationDialogData,
  IObservationDialogResultData,
  ObservationDialogComponent,
} from '../observation-dialog/observation-dialog.component';
import { ObservationService } from '../observation.service';

@Component({
  selector: 'app-observations',
  templateUrl: './observation-overview.component.html',
  styleUrls: ['./observation-overview.component.scss'],
})
export class ObservationOverviewComponent {
  public displayedColumns: string[] = ['id', 'bird', 'observationDate', 'location', 'remark', 'photo', 'actions'];
  public observations$: Observable<Observation[]>;

  private readonly addRequestedSubject = new Subject<void>();
  private readonly updateRequestedSubject = new Subject<Observation>();
  private readonly deleteRequestedSubject = new Subject<Observation>();

  constructor(
    public dialog: MatDialog,
    private observationService: ObservationService
  ) {
    const addObservation$ = this.addRequestedSubject.pipe(
      exhaustMap(() => {
        const dialogRef = this.openObservationDialog();
        return dialogRef.afterClosed().pipe(map(result => result?.observation));
      }),
      filter(observation => observation !== undefined),
      map(observation => observation as Observation),
      switchMap(observation =>
        this.observationService.addObservation(observation).pipe(
          map(newObservation => ({ ...observation, ...newObservation } as Observation))
        )
      ),
      filter(observation => observation.photo !== undefined || observation.photo !== null),
      switchMap(observation => this.observationService.updateObservationPhoto(observation.id, observation.photo!))
    );

    const updateObservation$ = this.updateRequestedSubject.pipe(
      exhaustMap(observation => {
        const dialogRef = this.openObservationDialog(observation);
        return dialogRef.afterClosed().pipe(map(result => result?.observation));
      }),
      filter(observation => observation !== undefined),
      map(observation => observation as Observation),
      switchMap(observation =>
        this.observationService
          .updateObservation(observation)
          .pipe(map(() => observation))
      ),
      mergeMap(observation => iif(() => observation.photo !== undefined,
        this.observationService.updateObservationPhoto(observation.id, observation.photo!),
        this.observationService.deleteObservationPhoto(observation.id))
      )
    );

    const deleteObservation$ = this.deleteRequestedSubject.pipe(
      switchMap((observation) =>
        this.observationService
          .deleteObservation(observation.id)
          .pipe(map(() => observation))
      )
    );

    const observationChanges$ = merge(
      addObservation$,
      updateObservation$,
      deleteObservation$
    );

    this.observations$ = merge(observationChanges$).pipe(
      startWith(null),
      switchMap(() => this.observationService.getObservations())
    );
  }

  public addObservation() {
    this.addRequestedSubject.next();
  }

  public editObservation(observation: Observation) {
    this.updateRequestedSubject.next(observation);
  }

  public removeObservation(observation: Observation) {
    this.deleteRequestedSubject.next(observation);
  }

  public handleMissingImage(event: Event) {
    (event.target as HTMLImageElement).style.display = 'none';
  }

  private openObservationDialog(
    observation?: Observation
  ): MatDialogRef<ObservationDialogComponent, IObservationDialogResultData> {
    return this.dialog.open<ObservationDialogComponent, IObservationDialogData, IObservationDialogResultData>(ObservationDialogComponent, {
      data: { observation },
      autoFocus: false,
    });
  }
}
