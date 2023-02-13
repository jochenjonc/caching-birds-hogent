import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';

import { Bird } from '../../birds/bird';
import { BirdService } from '../../birds/bird.service';
import { Observation } from '../observation';

export interface IObservationDialogData {
  observation?: Observation;
}
export type IObservationDialogResultData = Required<IObservationDialogData>;

@Component({
  selector: 'app-observation-dialog',
  templateUrl: './observation-dialog.component.html',
  styleUrls: ['./observation-dialog.component.scss'],
})
export class ObservationDialogComponent implements OnInit {
  public form = new FormGroup({
    bird: new FormControl<Bird | undefined>(undefined, Validators.required),
    observationDate: new FormControl<Date>(new Date(), Validators.required),
    latitude: new FormControl<number | undefined>(undefined, [Validators.min(-90), Validators.max(90)]),
    longitude: new FormControl<number | undefined>(undefined, [Validators.min(-180), Validators.max(180)]),
    remark: new FormControl<string | undefined>(undefined),
    photo: new FormControl<File | undefined>(undefined)
  });

  public photoUrl: string | null | undefined;

  public birds$: Observable<Bird[]>;

  constructor(
    private birdService: BirdService,
    public dialogRef: MatDialogRef<ObservationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IObservationDialogData
  ) {
    this.birds$ = this.birdService.getBirds();
  }

  ngOnInit() {
    if (this.data.observation) {
      this.form.patchValue({
        bird: this.data.observation.bird,
        observationDate: this.data.observation.observationDate,
        latitude: this.data.observation.location?.coordinates[1],
        longitude: this.data.observation.location?.coordinates[0],
        remark: this.data.observation.remark,
        photo: this.data.observation.photo
      });
    }
  }

  public compareBirds(left: Bird | undefined, right: Bird | undefined): boolean {
    return left?.id === right?.id
  }

  public onCancel(): void {
    this.dialogRef.close();
  }

  public onSave() : void {
    if(this.form.invalid) {
      return;
    }

    this.dialogRef.close(<IObservationDialogResultData>{
      observation: {
        id: this.data.observation?.id,
        bird: this.form.value.bird,
        observationDate: this.form.value.observationDate,
        location: this.form.value.longitude && this.form.value.latitude ? {
          type: 'Point',
          coordinates: [this.form.value.longitude, this.form.value.latitude]
        } : undefined,
        remark: this.form.value.remark,
        photo: this.form.value.photo
      }
    });
  }

  public getLocation() {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition((position: GeolocationPosition) => {
        if (position) {
          this.form.patchValue({
            latitude: position.coords.latitude,
            longitude: position.coords.longitude
          });
        }
      },
        (error: GeolocationPositionError) => console.error(error));
    } else {
      alert("Geolocation is not supported by this browser.");
    }
  }

  public fileInputChange(files: FileList | null) {
    if (files?.length) {
      this.form.patchValue({
        photo: files[0]
      });
      this.form.get('photo')?.updateValueAndValidity();

      // File Preview
      const reader = new FileReader();
      reader.onload = () => {
        this.photoUrl = reader.result as string;
      }
      reader.readAsDataURL(files[0])
    } else {
      this.form.patchValue({
        photo: undefined
      });

      this.photoUrl = undefined;
    }
  }
}
