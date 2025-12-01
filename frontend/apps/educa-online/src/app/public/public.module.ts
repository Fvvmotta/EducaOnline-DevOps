import { NgModule } from '@angular/core';

import { PublicComponent } from './public.component';
import { PublicRouting } from './public.routing';

@NgModule({
  imports: [PublicRouting],
  declarations: [PublicComponent],
  providers: []
})
export class PublicModule {
}
