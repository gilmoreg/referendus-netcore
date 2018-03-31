import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Http, RequestOptions } from '@angular/http';
import { JwtModule, JwtModuleOptions } from '@auth0/angular-jwt';

// Modules
import { AppModuleShared } from './app.shared.module';

// Components
import { AppComponent } from './components/app/app.component';

// Services
import { AuthService } from './auth/auth.service';

export function tokenGetter() {
	return localStorage.getItem('access_token');
};

export function getBaseUrl() {
	return document.getElementsByTagName('base')[0].href;
}

@NgModule({
	bootstrap: [AppComponent],
	imports: [
		BrowserModule,
		AppModuleShared,
		JwtModule.forRoot({ config: { tokenGetter }} as JwtModuleOptions)
	],
	providers: [
		AuthService,
		{ provide: 'BASE_URL', useFactory: getBaseUrl }
	]
})
export class AppModule {
}
