import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { MenuItem } from '../../../commons/models';

const ENDPOINT = 'menu';

/**
 * Menu items provider
 */
@Injectable()
export class MenuService {
  /**
   * Menu items that will be shared thoughout the application.
   * To update it, call "refresh".
   */
  public menuItems = new BehaviorSubject<MenuItem[]>([]);
  constructor(
    public api: ApiService
  ) {
    this.refresh();
  }


  private get(id: string = null) {
      return this.api.get(`${ENDPOINT}/`);
  }

  refresh() {
    this.get().subscribe(items => {
      this.menuItems.next(items);
    });
  }
}
