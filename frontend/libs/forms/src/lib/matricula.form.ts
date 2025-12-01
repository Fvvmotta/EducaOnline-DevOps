
import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { FormGroupBase } from "./form.base";

const fb = new FormBuilder();

export class MatriculaFormGroup extends FormGroupBase {

  get cursoId(): FormControl {
    return this.get('cursoId') as FormControl;
  }

  constructor() {
    super({
      cursoId: fb.control('', [Validators.required]),
    });
  }
}

export class CartaoFormGroup extends FormGroupBase {

  get numeroCartao(): FormControl {
    return this.get('numeroCartao') as FormControl;
  }

  get nomeCartao(): FormControl {
    return this.get('nomeCartao') as FormControl;
  }

  get expiracaoCartao(): FormControl {
    return this.get('expiracaoCartao') as FormControl;
  }

  get ccvCartao(): FormControl {
    return this.get('ccvCartao') as FormControl;
  }

   constructor() {
    super({
      numeroCartao: fb.control('', [
        Validators.required,
        Validators.pattern(/^\d{16}$/) // 16 dígitos numéricos
      ]),
      nomeCartao: fb.control('', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern(/^[A-Za-zÀ-ÿ\s]+$/) // apenas letras e espaços
      ]),
      expiracaoCartao: fb.control('', [
        Validators.required
        // Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/) // formato MM/AA
      ]),
      ccvCartao: fb.control('', [
        Validators.required,
        Validators.pattern(/^\d{3,4}$/) // 3 ou 4 dígitos numéricos
      ]),
    });
  }
}


