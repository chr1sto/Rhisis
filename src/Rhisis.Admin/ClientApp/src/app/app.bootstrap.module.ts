import { NgModule } from "@angular/core";

import { BsDropdownModule } from 'ngx-bootstrap';

/**
 * Add every bootstrap component definition.
 */
@NgModule({
    imports: [
        BsDropdownModule.forRoot()
    ],
    exports: [
        BsDropdownModule
    ]
})
export class AppBootstrapModule {

}