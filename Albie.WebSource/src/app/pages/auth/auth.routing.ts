import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        loadChildren: 'app/pages/auth/+login/login.module#LoginModule'
    }
];

export const routing = RouterModule.forChild(routes);
