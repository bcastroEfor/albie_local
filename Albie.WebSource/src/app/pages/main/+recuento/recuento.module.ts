import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'lista',
        pathMatch: 'full'
    },
    {
        path: 'detalle',
        loadChildren: 'app/pages/main/+recuento/+detalle/detalle.module#DetalleModule'
    },
    {
        path: 'lista',
        loadChildren: 'app/pages/main/+recuento/+lista/lista.module#ListaModule'
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ]
})
export class RecuentoModule { }
