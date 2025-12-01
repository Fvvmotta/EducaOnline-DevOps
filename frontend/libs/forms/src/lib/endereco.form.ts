
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { FormGroupBase } from "./form.base";

const fb = new FormBuilder();

export class EnderecoFormGroup extends FormGroupBase {

  get logradouro(): FormControl {
    return this.get('logradouro') as FormControl;
  }

  get numero(): FormControl {
    return this.get('numero') as FormControl;
  }

  get bairro(): FormControl {
    return this.get('bairro') as FormControl;
  }

  get cidade(): FormControl {
    return this.get('cidade') as FormControl;
  }

  get cep(): FormControl {
    return this.get('cep') as FormControl;
  }



  constructor() {
    super({
      logradouro: fb.control('', [Validators.required]),
      numero: fb.control('', [Validators.required]),
      bairro: fb.control('', [Validators.required]),
      cidade: fb.control('', [Validators.required]),
      cep: fb.control('', [Validators.required]),
    });
  }
}
