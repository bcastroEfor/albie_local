import { Component } from '@angular/core';
import { NotificationService } from 'actio-components-mdbs/dist';

@Component({
    selector: 'app-proveedor-lista',
    templateUrl: './lista.component.html',
    styleUrls: ['./lista.component.scss']
})
export class ListaComponent {
    constructor(public nSV: NotificationService) {
    }
}
