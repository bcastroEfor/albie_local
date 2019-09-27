import { NgModule } from '@angular/core';
import { MainPageComponent } from './main.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'compra',
    pathMatch: 'full'
  },
  {
    path: 'compra',
    loadChildren: 'app/pages/main/+compra/compra.module#CompraModule'
  },
  {
    path: 'lista',
    loadChildren: 'app/pages/main/+lista/lista.module#ListaModule'
  },
  {
    path: 'pedidos-habituales',
    loadChildren: 'app/pages/main/+pedidosHabituales/pedidosHabituales.module#PedidosHabitualesModule'
  },
  {
    path: 'proveedor',
    loadChildren: 'app/pages/main/+proveedor/proveedor.module#ProveedorModule'
  },
  {
    path: 'order',
    loadChildren: 'app/pages/main/+order/order.module#OrderModule'
  },
  {
    path: 'albaran',
    loadChildren: 'app/pages/main/+albaran/albaran.module#AlbaranModule'
  },
  {
    path: 'inventario',
    loadChildren: 'app/pages/main/+inventory/inventory.module#InventoryModule'
  },
  {
    path: 'recuento',
    loadChildren: 'app/pages/main/+recuento/recuento.module#RecuentoModule'
  },
  {
    path: 'ventas',
    loadChildren: 'app/pages/main/+ventas/ventas.module#VentasModule'
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  declarations: [
    MainPageComponent
  ],
  exports: [
    MainPageComponent
  ]
})
export class MainPageModule { }
