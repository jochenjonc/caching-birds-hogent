import { Point } from "geojson";
import { Bird } from "../birds/bird";

export interface Observation {
  id: number;
  bird: Bird;
  observationDate: Date;
  location: Point | undefined;
  remark: string | undefined;
  photo: File | undefined;
  hasPhoto: boolean;
}
