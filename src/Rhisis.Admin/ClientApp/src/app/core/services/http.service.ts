import { Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export class HttpService {
    protected readonly http: HttpClient;
    
    /**
     * Initializes the HttpService.
     */
    constructor(injector: Injector) {
        this.http = injector.get(HttpClient);
    }

    /**
     * Sends a GET request to the URL.
     * @param url API URL
     */
    public get<T>(url: string): Observable<T> {
        return this.http.get<T>(url);
    }
}