import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'centro',
        pathMatch: 'full'
    },
    {
        path: 'centro',
        loadChildren: () => import('./+centro/centro.module').then(mod => mod.CentroModule)
    },
    {
        path: 'contado',
        loadChildren: () => import('./+contado/contado.module').then(mod => mod.ContadoModule)
    },
    {
        path: 'caja',
        loadChildren: () => import('./+caja/caja.module').then(mod => mod.CajaModule)
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ]
})
export class VentasModule { }
