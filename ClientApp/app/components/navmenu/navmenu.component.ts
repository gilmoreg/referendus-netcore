import { Component } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
	selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
	private _authService: AuthService;

	constructor(authService: AuthService) {
		this._authService = authService;
	}

	login() {
		 this._authService.login();
	}
}
