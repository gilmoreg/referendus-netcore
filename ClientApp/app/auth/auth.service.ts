// src/app/auth/auth.service.ts

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/filter';
import * as auth0 from 'auth0-js';

@Injectable()
export class AuthService {

	auth0 = new auth0.WebAuth({
		clientID: 'vJbkOxpe8KH09MsN2yWihPx5M0Skm2eP',
		domain: 'gilmoreg.auth0.com',
		responseType: 'token id_token',
		audience: 'https://gilmoreg.auth0.com/userinfo',
		redirectUri: 'http://localhost:3000/callback',
		scope: 'openid'
	});

	constructor(public router: Router) { }

	public login(): void {
		this.auth0.authorize();
	}

	public handleAuthentication(): void {
		this.auth0.parseHash((err, authResult) => {
			if (authResult && authResult.accessToken && authResult.idToken) {
				window.location.hash = '';
				this.setSession(authResult);
				this.router.navigate(['/home']);
			} else if (err) {
				this.router.navigate(['/home']);
				console.log(err);
			}
		});
	}

	private setSession(authResult: any): void {
		// Set the time that the Access Token will expire at
		const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
		localStorage.setItem('access_token', authResult.accessToken);
		localStorage.setItem('id_token', authResult.idToken);
		localStorage.setItem('expires_at', expiresAt);
	}

	public logout(): void {
		// Remove tokens and expiry time from localStorage
		localStorage.removeItem('access_token');
		localStorage.removeItem('id_token');
		localStorage.removeItem('expires_at');
		// Go back to the home route
		this.router.navigate(['/']);
	}

	public isAuthenticated(): boolean {
		// Check whether the current time is past the
		// Access Token's expiry time
		const expiresAt = localStorage.getItem('expires_at');
		if (!expiresAt) return false;
		return new Date().getTime() < JSON.parse(expiresAt);
	}
}