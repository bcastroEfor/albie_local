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
        loadChildren: 'app/pages/main/+order/+detalle/detalle.module#DetalleModule'
    },
    {
        path: 'lista',
        loadChildren: 'app/pages/main/+order/+lista/lista.module#ListaModule'
    },
    {
        path: 'historico',
        loadChildren: 'app/pages/main/+order/+historico/historico.module#HistoricoModule'
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ]
})
export class OrderModule { }
