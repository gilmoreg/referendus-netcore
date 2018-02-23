import { Component, Inject } from '@angular/core';
import { AuthHttp } from 'angular2-jwt';
import 'rxjs/add/operator/map';

@Component({
	selector: 'fetchdata',
	templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
	public forecasts: WeatherForecast[];

	constructor(public authHttp: AuthHttp, @Inject('BASE_URL') baseUrl: string) {
		this.authHttp.get(`${baseUrl}api/SampleData/WeatherForecasts`)
			.map(res => res.json())
			.subscribe(
				data => this.forecasts = data as WeatherForecast[],
				error => console.error(error)
			);
	}
}

interface WeatherForecast {
	dateFormatted: string;
	temperatureC: number;
	temperatureF: number;
	summary: string;
}
