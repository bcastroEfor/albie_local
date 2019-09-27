import { Routes, RouterModule } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';
import { LayoutMainComponent } from './layouts/main/main.layout';
import { LayoutAuthComponent } from './layouts/auth/auth.component';

export const routes: Routes = [
    {
        path: '',
        component: LayoutMainComponent,
        loadChildren: 'app/pages/main/main.module#MainPageModule'
    },
    {
        path: 'auth',
        component: LayoutAuthComponent,
        loadChildren: 'app/pages/auth/auth.module#AuthModule'
    },
    { path: '**', redirectTo: '' }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes, { useHash: true });
