import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';

import { environment } from './../../environments/environment';
import { City } from './city';
import { Country } from './../countries/country';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss']
})

export class CityEditComponent implements OnInit {

  title?: string;
  form!: FormGroup;
  city?: City;
  id?: number;
  countries?: Country[];

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl(''),
      countryId: new FormControl('');
    });
    this.loadData();
  }

  loadData() {
    this.loadCountries();

    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0; //The preceding + sign converts the variable to number, or returns NAN.

    if (this.id) {
      var url = environment.baseUrl + 'api/Cities/' + this.id;
      this.http.get<City>(url).subscribe(result => {
        this.city = result;
        this.title = "Edit - " + this.city.name;

        this.form.patchValue(this.city);
      }, error => console.error(error));
    } else {
      this.title = "Create a new City";
    }
  }

  loadCountries() {
    var url = environment.baseUrl + 'api/Coutries';
    var params = new HttpParams()
      .set("pageIndex", "0")
      .set("pageSize", "9999")
      .set("sortColumn", "name");

    this.http.get<any>(url, { params }).subscribe(result => {
      this.countries = result.data;
    }, error => console.log(error));
  }

  onSubmit() {
    var city = (this.id) ? this.city : <City>{};
    if (city) {
      city.name = this.form.controls['name'].value;
      city.lat = +this.form.controls['lat'].value;
      city.lon = +this.form.controls['lon'].value;

      if (this.id) {

        var url = environment.baseUrl + 'api/Cities/' + city.id;
        this.http.put<City>(url, city).subscribe(result => {
          console.log("City" + city!.id + "has been updated");

          this.router.navigate(['/cities']);
        }, error => console.log(error));
      } else {
        var url = environment.baseUrl + 'api/Cities';
        this.http.post<City>(url, city).subscribe(result => {
          console.log("City " + result.id + "has beed created.");
        }, error => console.log(error));
      }
    }
  }


}
