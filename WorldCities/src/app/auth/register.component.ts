import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseFormComponent } from './../base-form.component';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, tap } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountInfo } from './account-info';
import { environment } from '../../environments/environment';
import { RegisterResult } from './register-result';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends BaseFormComponent implements OnInit {

  constructor(
    private router: Router,
    private http: HttpClient,
    private authService: AuthService
  ) {
    super();
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/)], this.isRegisteredAccount()),
      password: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/)]),
      confirmPassword: new FormControl('', [Validators.required, this.isIdenticalField()])
    });
  }

  isRegisteredAccount(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } | null> => {
      var registerRequest = <AccountInfo>{};
      registerRequest.email = this.form.controls['email'].value;
      registerRequest.password = 'registerRequest.email';
      var url = environment.baseUrl + 'api/account/isregisteredaccount';
      return this.http.post<boolean>(url, registerRequest).pipe(map(result => {
        return (result ? { isRegisteredAccount: true } : null);
      }))
    };
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

  onSubmit() {
    var registerRequest = <AccountInfo>{};
    registerRequest.email = this.form.controls['email'].value;
    registerRequest.password = this.form.controls['password'].value;
    var url = environment.baseUrl + 'api/Account/Register';
    return this.authService.register(registerRequest).subscribe(result => {
      if (result.success) {
        this.router.navigate(["/registrationsuccess"]);
      }
    }, error => {
      console.log(error);
    });
  }
}
