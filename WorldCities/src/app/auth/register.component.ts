import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseFormComponent } from './../base-form.component';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountInfo } from './account-info';
import { environment } from '../../environments/environment.prod';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends BaseFormComponent implements OnInit {


  constructor(
    private router: Router,
    private http: HttpClient
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/)]),
      password: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/)]),
      confirmPassword: new FormControl('', [Validators.required, this.isIdenticalField()])
    }, null, this.isRegisteredAccount());
  }

  isRegisteredAccount(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      var email = this.form.controls['email'].value;
      var url = environment.baseUrl + 'api/account/isregisteredaccount';
      var params = new HttpParams().set("email", email)
      return this.http.post<boolean>(url, null, { params }).pipe(map(result => {
        return (result ? { isRegisteredAccount: true } : null);
      }))
    }
  }

  isIdenticalField(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) return null;
      let passField = control.parent.get('password')!.value;
      console.log(control.errors?.['duplicated']);
      let confirmPassField = control.parent.get('confirmPassword')!.value;
      return passField !== confirmPassField ? { duplicated: true } : null;
    };
  }


  updateValue() {
    this.form.controls['password'].updateValueAndValidity();
  }

  onSubmit() { }
}
