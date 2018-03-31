import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Component({
	selector: 'fetchdata',
	templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
	public forecasts: WeatherForecast[];

	constructor(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		httpClient.get<WeatherForecast[]>(`${baseUrl}api/SampleData/WeatherForecasts`)
			.subscribe(
				data => this.forecasts = data,
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
