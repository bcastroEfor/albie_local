import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';
import { ApiService, HttpOptions, SettingsService } from 'actio-components-mdbs/dist';

@Injectable()
export class AuthService {
  public userData: BehaviorSubject<any> = new BehaviorSubject<any>({});

  constructor(
    private api: ApiService,
    private route: Router,
    private settings: SettingsService
  ) {
    const a = this.settings.get('auth') || {};
    this.userData.next(a.extraData);
  }

  login(accountInfo: any) {
    const seq = this.api.post('login', accountInfo)
      .map(r => {
        this.settings.set('auth', r);
        this.userData.next(r.extraData);
        return r;
      });
    return seq;
  }

  logout(): void {
    this.settings.set('auth', {});
    this.route.navigate(['auth', 'login']);
  }
}
