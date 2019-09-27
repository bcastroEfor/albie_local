import { Component } from '@angular/core';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { SettingsService } from 'actio-components-mdbs/dist';
import { utc } from 'moment';

declare var WOW;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public project = {
    copyright: {
      year: utc(new Date()).year(),
      owner: 'Actio Procesos & Tecnología',
      website: 'https://www.actiobp.com/',
      websiteName: 'actiobp.com'
    },
    name: 'Actio Procesos & Tecnología',
  };

  constructor(
    public translate: TranslateService,
    public settings: SettingsService
  ) {
    this.initApp();

    this.initTranslate();
  }
  private initApp() {
    new WOW().init();
    const loader = document.getElementById('apploader');
    loader.classList.add('wow', 'fadeOut');
    setTimeout(() => {
      loader.style.visibility = 'hidden';
    }, 1000);
  }
  private initTranslate() {
    const lang = this.settings.get('lang');
    if (lang == null || lang === 'es') {
      // Revisar. No puede ser que si tengo desde settings "en"
      // No sea capaz de trabajar con 2 traducciones.
      this.translate.addLangs(['es']);
      this.translate.setDefaultLang('eu');
      this.translate.use('es');
    } else {
      this.translate.addLangs(['eu']);
      this.translate.setDefaultLang('es');
      this.translate.use('eu');
    }
    this.translate.onLangChange.subscribe((v: LangChangeEvent) => {
      this.settings.set('lang', v.lang);
    });
  }

}
