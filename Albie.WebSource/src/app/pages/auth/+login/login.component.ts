import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../shared/services/angular/auth/auth.service';

@Component({
    selector: 'app-auth-login',
    templateUrl: './login.component.html',
    styleUrls: [
        './login.component.scss'
    ]
})
export class LoginComponent implements OnInit {
    userData: any = {};

    isLoading = false;

    constructor(
        private auth: AuthService,
        private router: Router
    ) { }

    ngOnInit() { }

    logIn() {
        // this.isLoading = true;
        // this.auth.login(this.userData).subscribe(
        //     v => {
        //         this.router.navigate([''])
        //             .then(() => { this.isLoading = false; }, () => { this.isLoading = false; })
        //             .catch(() => { this.isLoading = false; });
        //     },
        //     err => {
        //         this.isLoading = false;
        //     });
    }
}
