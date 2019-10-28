import { Component, OnInit, OnChanges } from '@angular/core';
import { DataService } from './data.service';
import { Rate } from './rate';
import { Currency } from './currency';
import { timer } from 'rxjs';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    providers: [DataService]
})

export class AppComponent implements OnInit {

    rate: Rate = new Rate();   // changes rate
    rates: Rate[];                // array of currency
    currencies: Currency[];
    tableMode: boolean = true;          // table mode
    sumToGive: number;
    firstPair: string;
    secondPair: string;
    receivedAsk: number;
    valid: boolean;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.loadRates();    // load all rates from db  
        this.loadCurrencies(); // load currencies from db
        this.oberserableTimer(); // timer to get the latest rate
        this.receivedAsk = 0; // sum to get
    }

    //currency to give
    setPair(val: any) {
        this.firstPair = val;
               
        if (this.validSet())
            this.loadAsk(); // get sum to give ask * sumToGive
        
        else this.receivedAsk = 0;
    }

    //currency to get
    setPair2(val: any) {
        this.secondPair = val;

        if (this.validSet())
            this.loadAsk();

        else this.receivedAsk = 0;
    }

    //sum to exchange
    setSum(val: any) {
        this.sumToGive = val;

        if (this.validSet()) 
            this.loadAsk();        
        else this.receivedAsk = 0;
    }

    //update data in table of rate
    oberserableTimer() {
        const source = timer(0, 300000);
        source.subscribe(val => {
            console.log("I load rates by timer");
            this.loadRates();
        });
    }

    // get data
    loadRates() {
        this.dataService.getRates()
            .subscribe((data: Rate[]) => { console.log(data); this.rates = data });
    }

    // load currencies from db
    loadCurrencies() {
        this.dataService.getCurrencies()
            .subscribe((data: Currency[]) => { console.log(data); this.currencies = data });
    }

    // get ask
    loadAsk() {
        this.dataService.getAsk(this.firstPair + this.secondPair, this.sumToGive).subscribe((data: number) => { this.receivedAsk = data; console.log(data) });
    }

    // check values to auto update sum to get
    validSet() {
        if (this.sumToGive != null && this.firstPair != null && this.secondPair != null)
            return true;
        else return false;
    }
}