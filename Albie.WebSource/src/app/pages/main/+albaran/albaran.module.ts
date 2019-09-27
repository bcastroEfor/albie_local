import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'detalle',
        pathMatch: 'full'
    },
    {
        path: 'detalle',
        loadChildren: 'app/pages/main/+albaran/+detalle/detalle.module#DetalleModule'
    },
    {
        path: 'lista',
        loadChildren: 'app/pages/main/+albaran/+lista/lista.module#ListaModule'
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ]
})
export class AlbaranModule { }
