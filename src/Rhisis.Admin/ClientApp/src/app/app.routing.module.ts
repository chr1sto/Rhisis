import { NgModule } from "@angular/core";
import { RouterModule, Route } from "@angular/router";

/**
 * Defines the app routes
 */
const routes: Route[] = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
    },
    {
        path: 'home',
        component: undefined
    }
]

@NgModule({
    imports: [
        RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}