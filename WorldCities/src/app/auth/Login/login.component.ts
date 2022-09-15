import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';

import { BaseFormComponent } from '../base-form.component';
import { AuthService } from './auth.service';
import { LoginRequest } from './login-request';
import { LoginResult } from './login-result';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scsss']
})

export class LoginComponent extends BaseFormComponent implements OnInit {
    title?: string;
    loginResult?: LoginResult;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private authService: AuthService
    ) {
        super();
    }

    ngOnInit() {
        this.form = new FormGroup({
            email: new FormControl('', Validators.required),
            password: new FormControl('', Validators.required)
        });
    }

    onSubmit() {
        var loginRequest = <LoginRequest>{};
        loginRequest.email = this.forms.controls['email'].value;
        loginRequest.password = this.forms.controls['password'].value;

        this.authService.login(loginRequest).subscribe(result => {
            console.log(result);
            this.loginResult = result;
            if (result.success && result.token) {
                localStorage.setItem(this.authService.tokenKey, result.token);
                this.router.navigate(["/"]);
            }
        }, error => {
            console.log(error);
            if (error.status == 401) {
                this.loginResult = error.error;
            }
        });
    }
}
