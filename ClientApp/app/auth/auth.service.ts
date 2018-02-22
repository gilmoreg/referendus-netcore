// src/app/auth/auth.service.ts

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import 'rxjs/add/operator/filter';
import * as auth0 from 'auth0-js';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/timer';

@Injectable()
export class AuthService {
	refreshSubscription: Subscription;

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
		this.scheduleRenewal();
	}

	public logout(): void {
		// Remove tokens and expiry time from localStorage
		localStorage.removeItem('access_token');
		localStorage.removeItem('id_token');
		localStorage.removeItem('expires_at');
		this.unscheduleRenewal();
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

	public renewToken() {
		this.auth0.checkSession({}, (err, result) => {
			if (err) {
				console.log(err);
			} else {
				this.setSession(result);
			}
		});
	}

	public scheduleRenewal() {
		if (!this.isAuthenticated()) return;
		this.unscheduleRenewal();

		const expiresAt = JSON.parse(window.localStorage.getItem('expires_at') || '');

		const source = Observable.of(expiresAt).flatMap(
			expiresAt => {

				const now = Date.now();

				// Use the delay in a timer to
				// run the refresh at the proper time
				return Observable.timer(Math.max(1, expiresAt - now));
			});

		// Once the delay time from above is
		// reached, get a new JWT and schedule
		// additional refreshes
		this.refreshSubscription = source.subscribe(() => {
			this.renewToken();
			this.scheduleRenewal();
		});
	}

	public unscheduleRenewal() {
		if (!this.refreshSubscription) return;
		this.refreshSubscription.unsubscribe();
	}
}