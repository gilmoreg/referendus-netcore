import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Http, RequestOptions } from '@angular/http';
import { AuthHttp, AuthConfig } from 'angular2-jwt';

// Modules
import { AppModuleShared } from './app.shared.module';

// Components
import { AppComponent } from './components/app/app.component';

// Services
import { AuthService } from './auth/auth.service';

export function authHttpServiceFactory(http: Http, options: RequestOptions) {
	return new AuthHttp(new AuthConfig({
		tokenGetter: (async () => await localStorage.getItem('access_token') || '')
	}), http, options);
}

export function getBaseUrl() {
	return document.getElementsByTagName('base')[0].href;
}

@NgModule({
    bootstrap: [ AppComponent ],
    imports: [
        BrowserModule,
        AppModuleShared
    ],
	providers: [
		AuthService,
		{
			provide: AuthHttp,
			useFactory: authHttpServiceFactory,
			deps: [Http, RequestOptions]
		},
        { provide: 'BASE_URL', useFactory: getBaseUrl }
    ]
})
export class AppModule {
}
