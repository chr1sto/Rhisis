import { NgModule } from '@angular/core';
import { Http } from '@angular/http';
import { HttpService } from './services/http.service';

@NgModule({
    imports: [
        Http,
    ],
    exports: [
        Http
    ],
    providers: [
        HttpService
    ]
})
export class CoreModule {
}