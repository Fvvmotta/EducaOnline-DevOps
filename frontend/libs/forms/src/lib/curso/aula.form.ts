import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { FormGroupBase } from "../form.base";

const fb = new FormBuilder();

export class AulaFormGroup extends FormGroupBase {

  get id(): FormControl {
    return this.get('id') as FormControl;
  }

  get titulo(): FormControl {
    return this.get('titulo') as FormControl;
  }

  get descricao(): FormControl {
    return this.get('descricao') as FormControl;
  }

  get totalHoras(): FormControl {
    return this.get('totalHoras') as FormControl;
  }

  get cursoId(): FormControl {
    return this.get('cursoId') as FormControl;
  }

  constructor() {
    super({
      id: fb.control(null),
      titulo: fb.control('', Validators.required),
      descricao: fb.control('', Validators.required),
      totalHoras: fb.control(0, Validators.required),
      cursoId: fb.control(null, Validators.required),
    });
  }
}