import { Component, ViewChild, OnInit } from '@angular/core';
import { ColumnItem, ActDatatableComponent, ActDatatableCellButton, ActDataselectComponent, AutocompleteComponent, NotificationService } from 'actio-components-mdbs/dist';
import { IDatatableRowClickEvent } from 'actio-components-mdbs/dist/app/components/datatable/common/models/events';
import { CompraService } from '../../../shared/services/webapi/compra/compra.service';
import { ProductService } from '../../../shared/services/angular/product/product.service';
import { Subscription } from 'rxjs/Subscription';
import { ModalDirective } from 'angular-bootstrap-md';

@Component({
    selector: 'app-compra',
    templateUrl: './compra.component.html',
    styleUrls: ['./compra.component.scss']
})
export class CompraComponent {
    @ViewChild('productsCategory') public dtProductsCategory: ActDatatableComponent;
    @ViewChild('providerRate') public dtProvidersRate: ActDatatableComponent;
    @ViewChild('category') dsCategory: ActDataselectComponent;
    @ViewChild('subcategory') dsSubcategory: ActDataselectComponent;
    @ViewChild('corte') dsCorte: ActDataselectComponent;
    @ViewChild('providersModal') public providersModal: ModalDirective;
    @ViewChild('autoComplete') public productos: AutocompleteComponent;
    columnsProductCategory: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Nombre',
            field: 'description',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    columnsProduct: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Nombre',
            field: 'description',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    columnsProviderRate: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Proveedor',
            field: 'provider.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Precio unidad',
            field: 'directUnitCost',
            sorted: true,
            filter: true,
            fieldDisplayType: 'currency',
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Udad',
            field: 'product.unitMeasureProduct.code',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    parentCategory = '';
    identation = -1;
    providers = false;
    title = 'por productos';
    showSubcategory = false;
    showSubsubcategory = false;
    products: any[];
    cart: any[] = [];
    private subscriptions: Subscription[] = [];
    productoId: any;
    modo = false;
    modeProduct = true;
    constructor(private cSV: CompraService, private pSV: ProductService, public nSV: NotificationService) {
        this.getProducts('');
    }

    onProductsCategoryRowClick(event: IDatatableRowClickEvent) {
        this.identation = event.row.identation;
        if (event.row.parentCategory = '') {
            this.parentCategory = event.row.parentCategory;
        } else {
            this.parentCategory = event.row.code;
        }
        this.dtProductsCategory.refreshData();
    }

    onProductsRowClick(event: IDatatableRowClickEvent) {
        this.identation = event.row.identation;
    }

    onProvidersRowClick(event: IDatatableRowClickEvent) {
        this.identation = event.row.identation;
    }

    changeList() {
        this.providers = !this.providers;
    }

    categorySelect(event: any) {
        this.parentCategory = event;
        this.showSubsubcategory = false;
        if (event) {
            this.getProducts(event.substring(0, 3));
            setTimeout(() => {
                if (this.products.length > 0) {
                    this.showSubcategory = true;
                    this.dsSubcategory.refreshItems();
                } else {
                    this.showSubcategory = false;
                }
            }, 500);
        }
    }

    subCategorySelect(event: any) {
        this.parentCategory = event;
        if (event) {
            this.getProducts(event);
            setTimeout(() => {
                if (this.products.length > 0) {
                    this.showSubsubcategory = true;
                    this.dsCorte.refreshItems();
                } else {
                    this.showSubsubcategory = false;
                }
            }, 500);
        }
    }

    corteSelect(event: any) {
        this.getProducts(event);
    }

    getProducts(parentCategory: string) {
        this.cSV.getProductos(parentCategory).subscribe(
            result => {
                this.products = result;
            }
        );
    }

    addProduct(product: any) {
        this.getProductsCart();
        const carro = this.cart.filter(o => o.productNo === product.productNo)[0];
        if (carro) {
            this.cart.filter(o => o.productNo === product.productNo)[0].totalUnits += carro.unitMeasureProduct.quantity;
            this.cart.filter(o => o.productNo === product.productNo)[0].totalPrice = carro.totalUnits * carro.unitPrice;
        } else {
            product.totalUnits = product.unitMeasureProduct.quantity;
            product.totalPrice = product.totalUnits * product.unitPrice;
            this.cart.push(product);
        }
        this.pSV.getProducts(this.cart);
        this.nSV.smallToast('Carro', 'Producto añadido correctamente', 'success');
    }

    getProductsCart() {
        const s = this.pSV.products.subscribe((product) => {
            if (Object.keys(product).length > 0) {
                this.cart = product;
            }
        });
        this.subscriptions.push(s);
    }

    openModalProviders(event: any) {
        this.productoId = event.productNo;
        this.dtProvidersRate.refreshData();
        this.providersModal.show();
    }

    selectProduct(event: any) {
        const product = this.productos.options.filter(o => o.value === event)[0].data;
        this.products = [product];
    }

    updateProducts(index, item) {
        return item.productNo;
    }

    onProviderRateRowClick(event: IDatatableRowClickEvent) {
        const carro = this.cart.filter(o => o.productNo === this.productoId)[0];
        if (carro) {
            this.cart.filter(o => o.productNo === this.productoId)[0].providerRateId = event.row.id;
            this.cart.filter(o => o.productNo === this.productoId)[0].unitPrice = event.row.directUnitCost;
            this.cart.filter(o => o.productNo === this.productoId)[0].totalPrice = carro.totalUnits * carro.unitPrice;
            this.cart.filter(o => o.productNo === this.productoId)[0].providerId = event.row.vendorNo;
            this.pSV.getProducts(this.cart);
        }
        this.products.filter(o => o.productNo === this.productoId)[0].providerRateId = event.row.id;
        this.products.filter(o => o.productNo === this.productoId)[0].unitPrice = event.row.directUnitCost;
        this.products.filter(o => o.productNo === this.productoId)[0].providerId = event.row.vendorNo;
        this.nSV.smallToast('Proveedor', 'Proveedor seleccionado correctamente', 'success');
    }

    trackByFn(index, item) {
        return (item.productNo);
    }

    providerMode() {
        this.modeProduct = false;
    }

    providerSelect(event: any) {
        this.modeProduct = true;
        this.getProducts(event);
    }

    productMode() {
        this.modeProduct = !this.modeProduct;
        this.getProducts('');
    }
}
