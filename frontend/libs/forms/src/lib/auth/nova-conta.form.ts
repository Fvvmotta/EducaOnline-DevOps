import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { FormGroupBase } from "../form.base";
import { passwordMatchValidator } from "../validators";

const fb = new FormBuilder();

export class NovaContaFormGroup extends FormGroupBase {

  get nome(): FormControl {
    return this.get('nome') as FormControl;
  }

  get email(): FormControl {
    return this.get('email') as FormControl;
  }

  get senha(): FormControl {
    return this.get('senha') as FormControl;
  }

  get senhaConfirmacao(): FormControl {
    return this.get('senhaConfirmacao') as FormControl;
  }

  constructor() {
    super({
      nome: fb.control('', [Validators.required]),
      email: fb.control('', [Validators.required, Validators.email]),
      senha: fb.control('', [Validators.required]),
      senhaConfirmacao: fb.control('', [Validators.required])
    }, {
      validators: passwordMatchValidator('senha', 'senhaConfirmacao')
    });
  }
}
