import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AuthenticatedRouting } from "./authenticated.routing";
import { AuthenticatedComponent } from "./authenticated.component";
import { HeaderModule, MenuModule } from "@educa-online/components";
import { AlunoBffService, ConteudoService } from "@educa-online/services";
import { MAT_DATE_LOCALE } from "@angular/material/core";

@NgModule({
    imports: [
      CommonModule,
      RouterModule,
      MenuModule,
      AuthenticatedRouting,
      HeaderModule,
    ],
    exports: [],
    declarations: [AuthenticatedComponent,],
    providers: [{provide: MAT_DATE_LOCALE, useValue: 'pt'}],
})

export class AuthenticatedModule {}