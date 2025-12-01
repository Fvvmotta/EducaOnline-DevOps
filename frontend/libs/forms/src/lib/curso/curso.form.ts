import { FormBuilder, FormControl, Validators } from "@angular/forms";
import { FormGroupBase } from "../form.base";

const fb = new FormBuilder();

export class CursoFormGroup extends FormGroupBase {

  get id(): FormControl {
    return this.get('id') as FormControl;
  }

  get nome(): FormControl {
    return this.get('nome') as FormControl;
  }

  get valor(): FormControl {
    return this.get('valor') as FormControl;
  }

  get ativo(): FormControl {
    return this.get('ativo') as FormControl;
  }

  get conteudoProgramatico(): ConteudoProgramaticoFormGroup {
    return this.get('conteudoProgramatico') as ConteudoProgramaticoFormGroup;
  }

  constructor() {
    super({
      id: fb.control(null),
      nome: fb.control('', Validators.required),
      ativo: fb.control(true, Validators.required),
      valor: fb.control(null, Validators.required),
      conteudoProgramatico: new ConteudoProgramaticoFormGroup,
    });
  }
}

export class ConteudoProgramaticoFormGroup extends FormGroupBase {

  get titulo(): FormControl {
    return this.get('titulo') as FormControl;
  }

  get descricao(): FormControl {
    return this.get('descricao') as FormControl;
  }

  get cargaHoraria(): FormControl {
    return this.get('cargaHoraria') as FormControl;
  }

  get objetivos(): FormControl {
    return this.get('objetivos') as FormControl;
  }

  constructor() {
    super({
      titulo: fb.control('', Validators.required),
      descricao: fb.control('', Validators.required),
      cargaHoraria: fb.control(null, Validators.required),
      objetivos: fb.control('', Validators.required),
    });
  }
}