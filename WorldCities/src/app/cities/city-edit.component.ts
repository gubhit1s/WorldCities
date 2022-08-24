import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';

import { environment } from './../../environments/environment';
import { City } from './city';
import { Country } from './../countries/country';
import { BaseFormComponent } from '../base-form.component';
import { CityService } from './city.service';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss']
})

export class CityEditComponent extends BaseFormComponent implements OnInit, OnDestroy {

  title?: string;
  city?: City;
  id?: number;
  countries?: Observable<Country[]>;
  activityLog: string = '';
  private destroySubject = new Subject();

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private cityService: CityService
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      lat: new FormControl('', [Validators.required, Validators.pattern(/^[-]?\d+(\.\d{1,4})?$/)]),
      lon: new FormControl('', [Validators.required, Validators.pattern(/^[-]?\d+(\.\d{1,4})?$/)]),
      countryId: new FormControl('', Validators.required)
    }, null, this.isDupeCity());

    this.form.valueChanges.pipe(takeUntil(this.destroySubject)).subscribe(() => {
      if (!this.form.dirty) {
        this.log("Form model has been loaded");
      } else {
        this.log("Form was updated by the user");
      }
    });

    this.form.get("name")!.valueChanges.pipe(takeUntil(this.destroySubject)).subscribe(() => {
      if (!this.form.dirty) {
        this.log("Name has been loaded with initial values.");
      } else {
        this.log("Name was updated by the user.");
      }
    });

    this.loadData();
  }

  ngOnDestroy() {
    this.destroySubject.next(true);
    this.destroySubject.complete();
  }

  log(str: string) {
    this.activityLog += "[" + new Date().toLocaleString() + "] " + str + "<br />";
  }

  isDupeCity(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      var city = <City>{};
      city.id = (this.id) ? this.id : 0;
      console.log(city.id);
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;
      city.countryId = +this.form.controls['countryId'].value;

      var url = environment.baseUrl + 'api/cities/isdupecity';
      return this.cityService.isDupeCity(city).pipe(map(result => {
        return (result ? { isDupeCity: true } : null);
      }));
    }
  }

  loadData() {
    this.loadCountries(); //This method call is asynchronous.

    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0; //The preceding + sign converts the string variable to number, or returns NAN.

    if (this.id) {
      this.cityService.get(this.id).subscribe(result => {
        this.city = result;
        this.title = "Edit - " + this.city.name;

        this.form.patchValue(this.city);
      }, error => console.error(error));
    } else {
      this.title = "Create a new City";
    }
  }

  loadCountries() {
    this.cityService.getCountries(0, 9999, "name", "asc", null, null).pipe(map(
      x => x.data
    ));
  }

  onSubmit() {
    var city = (this.id) ? this.city : <City>{};
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;
      city.countryId = +this.form.controls['countryId'].value;

      if (this.id) {
        this.cityService.put(city).subscribe(result => {
          console.log("City " + city!.id + " has been updated.");
          this.router.navigate(['/cities']);
        }, error => console.error(error));
      } else {
        this.cityService.post(city).subscribe(result => {
          console.log("City " + result.id + " has been updated.");
          this.router.navigate(['/cities']);
        }, error => console.log(error));
      }
    }
  }


}
