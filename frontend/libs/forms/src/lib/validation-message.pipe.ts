import { Pipe, PipeTransform } from '@angular/core';
// import * as moment from 'moment';

const messages = {
  required: 'Campo obrigatório',
  minlength: 'Digite pelo menos %d caracteres',
  maxlength: 'Tamanho máximo de %d caracteres excedido',
  min: 'O valor deve ser mais que %d',
  email: 'Email inválido',
  pattern: 'Formato inválido',
  date: 'Data inválida',
  minDate: 'A data deve ser maior ou igual a %s',
  maxDate: 'A data deve ser menor ou igual a %s',
  passwordMismatch: 'As senhas não conferem',
  cpf: 'Cpf inválido'
};

@Pipe({ name: 'validationMessage' })
export class ValidationMessagePipe implements PipeTransform {

  private messages: { [key: string]: string; };

  constructor() {
    this.messages = { ...messages };
  }

  transform(value: { [key: string]: any }) {
    if (!value) { return null; }
    for (const key in value) {
      if (value.hasOwnProperty(key)) {
        switch (key) {
          case 'maxlength':
            return `${this.messages[key]}, máximo: ${value['maxlength']?.requiredLength}`;
          case 'minlength':
            return `${this.messages[key]}, mínimo: ${value['minlength']?.requiredLength}`;;
          case 'min':
            return`${this.messages[key]}, mínimo: ${value['min'].min - 1}`;
          // case 'bsDate':
          //   return this.getBsDateErrorMessages(value.bsDate);
          default:
            return key in this.messages ? this.messages[key] : JSON.stringify(value);
        }
      }
    }

    return '';
  }

}
