import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rate } from './rate';
import { Currency } from './currency';

@Injectable()
export class DataService {

    private url = "/api/rates";
    private urlCurrency = "/api/currencies";

    constructor(private http: HttpClient) {
    }

    getRates() {
        return this.http.get(this.url);
    }

    getCurrencies() {
        return this.http.get(this.urlCurrency);
    }

    getAsk(pair: string, sum: number) {
        return this.http.get(this.url + '/' + pair + '/' + sum);
    }
}