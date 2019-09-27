import { MenuItemWrapper } from './menu-wrapper';

export class MenuItem {
    label = 'Dashboard';
    iconClass = 'fa fa-lg fa-home';
    routerLink?: string = null;
    routerLinkActive = 'text-white';
    wrapper: MenuItemWrapper = new MenuItemWrapper;
    children: MenuItem[] = [];
    get hasChildren() { return this.children && this.children.length > 0; }

    constructor(
        label: string,
        iconClass: string,
        routerLink: string = null,
        routerLinkActive: string = 'text-white',
        routerLinkActiveWrapper: string = 'primary-color text-white',
        children: MenuItem[] = [],
    ) {
        this.label = label;
        this.iconClass = iconClass;
        this.routerLink = routerLink;
        this.routerLinkActive = routerLinkActive;

        this.wrapper = new MenuItemWrapper(routerLinkActiveWrapper);
        this.children = children;
    }
}
