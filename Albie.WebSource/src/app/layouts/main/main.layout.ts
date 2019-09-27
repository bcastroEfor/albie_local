import { Component, AfterViewInit } from '@angular/core';
import { OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { ApiService, SettingsService, NotificationService, AutocompleteComponent } from 'actio-components-mdbs/dist';
import { AuthService } from '../../shared/services/angular/auth/auth.service';
import { MenuItem } from '../../shared/commons/models';
import * as moment from 'moment';
import { MenuService } from '../../shared/services/angular/menu/menu.service';
import { Subscription } from 'rxjs/Subscription';
import { ProductService } from '../../shared/services/angular/product/product.service';
import { CompraService } from '../../shared/services/webapi/compra/compra.service';
import { ModalDirective } from 'angular-bootstrap-md';
@Component({
    selector: 'app-layout-main',
    templateUrl: './main.layout.html',
    styleUrls: ['./main.layout.scss']
})
export class LayoutMainComponent implements OnInit, AfterViewInit {
    public menuItems: MenuItem[] = [];

    @ViewChild('drawer') drawer: MatDrawer;
    @ViewChild('navBar') navBar: any;
    @ViewChild('saveListModal') public listModal: ModalDirective;
    
    public isApiIddle = false;
    public navBarHeight = 0;
    public isDrawerOpened = false;
    products: any[] = [];
    private subscriptions: Subscription[] = [];
    displayedColumns: string[] = ['photo'];
    opened = false;
    total = 0;
    nombreLista = '';
    productName = '';
    constructor(
        api: ApiService,
        public menuSV: MenuService,
        public aSV: AuthService,
        public pSV: ProductService,
        public nSV: NotificationService,
        public cSV: CompraService
    ) {
        api.isWorking.subscribe(v => {
            this.isApiIddle = !v;
        });

        menuSV.menuItems.subscribe(items => {
            this.menuItems = items;
        });
    }

    getProducts() {
        const s = this.pSV.products.subscribe((product) => {
            if (Object.keys(product).length > 0) {
                this.products = product;
                this.calcTotal();
            }
        });
        this.subscriptions.push(s);
    }

    

    calcTotal() {
        this.total = 0;
        for (let i = 0; i < this.products.length; i++) {
            this.total += this.products[i].totalPrice;
        }
    }

    getState(outlet) {
        return outlet.activatedRouteData.state;
    }

    ngOnInit() {
        moment.locale('es');
    }

    ngAfterViewInit() {
        this.calculateNavBarLeftSpace();
    }

    toogleMenu(e: Event) {
        this.drawer.toggle();
        this.calculateNavBarLeftSpace();
    }

    logOut() {
        this.aSV.logout();
    }

    private calculateNavBarLeftSpace() {
        const nBar: HTMLElement = this.navBar.el.nativeElement;
        const drawerContent = <HTMLElement>nBar.closest('mat-drawer-content');
        if (this.drawer.opened) {
            this.isDrawerOpened = true;
            nBar.parentElement.style.paddingLeft = `${this.drawer._width}px`;
            drawerContent.style.paddingLeft = `${this.drawer._width}px`;
        } else {
            this.isDrawerOpened = false;
            nBar.parentElement.style.paddingLeft = `0px`;
            drawerContent.style.paddingLeft = `0px`;
        }
        this.navBarHeight = nBar.parentElement.offsetHeight;
    }
    // IE fix
    private getClosest_IE(element: HTMLElement, cssClass: string) {
        while (element.className !== cssClass) {
            element = element.parentElement;
            if (!element) { return null; }
        }
    }

    openCart() {
        this.getProducts();
        if (this.products.length === 0) {
            this.nSV.smallToast('', 'No hay productos en el carro', 'info');
        } else {
            this.opened = !this.opened;
        }
    }

    clearProduct(product: any) {
        this.products = this.products.filter(item => item !== product);
        this.pSV.getProducts(this.products);
        if (this.products.length === 0) {
            this.opened = false;
        }
        this.nSV.smallToast('Carro', 'Producto borrado correctamente', 'success');
    }

    openModalList() {
        this.listModal.show();
    }

    saveList() {
        this.listModal.hide();
        this.cSV.postList(this.products, this.nombreLista).subscribe(result => {
            this.nSV.smallToast('', 'Lista guardada', 'success');
            this.pSV.getProducts([]);
        });
    }
}
