/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-non-null-assertion */
import {
  AbstractControl,
  AsyncValidatorFn,
  FormArray,
  FormControl,
  FormGroup,
  ValidatorFn,
  AbstractControlOptions
} from '@angular/forms';
import { Observable, Subject, Subscription } from 'rxjs';

export class ItemValidator {
  control?: string;
  validators?: ValidatorFn;
}

/**
* An extension of `@angular/forms#FormGroup` class.
*/
export class FormGroupBase extends FormGroup {

  protected destroy$ = new Subject<void>()

  constructor(
      controls: { [key: string]: AbstractControl },
      validatorOrOpts?: ValidatorFn | ValidatorFn[] | AbstractControlOptions,
      asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[]
  ) {
      super(controls, validatorOrOpts, asyncValidator);
  }

  /**
   * Resets a child control given the control's name or path.
   *
   * @param path A dot-delimited string or array of string/number values that define the path to the
   * control.
   *
   * * @param options Configuration options that determine how the control proopagates changes and
   * emits events when the value changes. The configuration options are passed to the `updateValueAndValidity` method.
   * * `onlySelf`: When true, each change only affects this control, and not its parent. Default is false.
   *
   * * `emitEvent`: When true or not supplied (the default), both the `statusChanges` and `valueChanges`
   * observables emit events with the latest status and value when the control value is updated. When false, no events are emitted.
   */
  resetControl(
      path: string | (string | number)[],
      value?: any,
      options?: {
          onlySelf?: boolean;
          emitEvent?: boolean;
      }) {
      this.get(path)!.reset(value, options);
  }


  clearSpecificControls(controls: string[]) {
      Object.keys(this.value).forEach(key => {
          if (controls.indexOf(key) > -1) {
              this.get(key)!.clearValidators();
              this.get(key)!.updateValueAndValidity();
          }
      });
  }

  resetAllControls(controls = this.controls) {
      Object.values(controls).forEach(control => {
          control.clearValidators();
          control.updateValueAndValidity();

          if (control instanceof FormGroup) {
              this.resetAllControls(control.controls);
          }
      });
  }

  resetExceptControl(
      exceptKeys: string[],
      value?: any,
      options?: {
          onlySelf?: boolean;
          emitEvent?: boolean;
      }) {

      Object.keys(this.value).forEach(key => {
          if (exceptKeys.indexOf(key) > -1)
              return;
          this.get(key)!.reset(value, options);
      })
  }

  addValidatorsManyControls(
      controls: string[],
      validators: ValidatorFn[],
      options?: {
          onlySelf?: boolean;
          emitEvent?: boolean;
      }) {

      Object.keys(this.value).forEach(key => {
          if (controls.indexOf(key) == 0) {
              const control = this.get(key);
              control?.setValidators(validators);
              control?.updateValueAndValidity();
          }
      });
  }


  addSpecificValidators(
      itens: ItemValidator[],
      options?: {
          onlySelf?: boolean;
          emitEvent?: boolean;
      }) {

      itens.forEach(item => {
          Object.keys(this.value).forEach(key => {
              if (item?.control?.indexOf(key) == 0) {
                  const control = this.get(item.control);
                  control?.setValidators(item.validators!);
                  control?.updateValueAndValidity();
              }
          });
      })
  }


  /**
   * Retrieves a child control value given the control's name or path.
   *
   * @param path A dot-delimited string or array of string/number values that define the path to the
   * control.
   *
   * @usageNotes
   * ### Retrieve a nested control
   *
   * For example, to get a `name` control nested within a `person` sub-group:
   *
   * * `this.form.getControlValue('person.name');`
   *
   * -OR-
   *
   * * `this.form.getControlValue(['person', 'name']);`
   */
  getControlValue(path: string | (string | number)[]): any {
      return this.get(path)!.value;
  }

  /**
   * Sets the value of the control given the control's name or path.
   *
   * @param path A dot-delimited string or array of string/number values that define the path to the
   * control.
   *
   * @param value The value of the control
   *
   * @param options Configuration options that determine how the control proopagates changes and
   * emits events when the value changes. The configuration options are passed to the `updateValueAndValidity` method.
   * * `onlySelf`: When true, each change only affects this control, and not its parent. Default is false.
   *
   * * `emitEvent`: When true or not supplied (the default), both the `statusChanges` and `valueChanges`
   * observables emit events with the latest status and value when the control value is updated. When false, no events are emitted.
   *
   * * `emitModelToViewChange`: When true or not supplied (the default), each change triggers an `onChange` event to update the view.
   *
   * * `emitViewToModelChange`: When true or not supplied (the default), each change triggers an `ngModelChange` event to update the model.
   */
  setControlValue(
      path: string | (string | number)[],
      value: any,
      options?: {
          onlySelf?: boolean;
          emitEvent?: boolean;
          emitModelToViewChange?: boolean;
          emitViewToModelChange?: boolean
      }) {
      this.get(path)!.setValue(value, options);
  }

  /**
   * A multicasting observable that emits an event every time the value of the control changes, in
   * the UI or programmatically.
   *
   * @param path A dot-delimited string or array of string/number values that define the path to the
   * control.
   *
   * @returns The `valueChanges` observable of the given control's path.
   */
  controlValueChanges(path: string | (string | number)[]): Observable<any> {
      return this.get(path)!.valueChanges;
  }

  /**
   * Removes all the items given the FormArray.
   * @param array The FormArray to remove the items from.
   */
  clearFormArray(array: FormArray) {
      while (array.length !== 0) {
          array.removeAt(0);
      }
  }

  /**
   * Subscribes to the `valuesChanges` given the source's name or path,
   * resets the control state and toggles disabled given the target's name or path.
   *
   * @param target A dot-delimited string or array of string/number values that define the path to the
   * target control to be disabled/enabled.
   *
   * @param source A dot-delimited string or array of string/number values that define the path which values
   * change will be subscribed to and checked using the predicate.
   *
   * @param predicate The predicate that says whether or not the target control should be disabled.
   *
   * @returns The `controlValueChanges` subscription.
   */
  toggleDisabled(
      target: string | (string | number)[],
      source: string | (string | number)[],
      predicate: (value: any) => boolean): Subscription {
      return this.controlValueChanges(source)
          .subscribe((value) => {
              const control = this.get(target);
              if (predicate(value)) {
                  control!.disable();
                  control!.reset();
              } else {
                  control!.enable();
              }
          });
  }

  /**
   * Subscribes to the `valuesChanges` of the source, resets the control state and toggles disabled
   * given the target's name or path when the predicate returns not true.
   *
   * @param target A dot-delimited string or array of string/number values that define the path to the
   * target control to be disabled.
   *
   * @param source A dot-delimited string or array of string/number values that define the path which values
   * change will be subscribed to and checked using the predicate.
   *
   * @returns The `controlValueChanges` subscription.
   */
  disableWhenFalse(target: string | (string | number)[], source: string | (string | number)[]): Subscription {
      return this.toggleDisabled(target, source, (value) => value !== true);
  }

  /**
   * Subscribes to the `valuesChanges` of the source, resets the control state and toggles disabled
   * given the target's name or path when the predicate returns true.
   *
   * @param target A dot-delimited string or array of string/number values that define the path to the
   * target control to be disabled.
   *
   * @param source A dot-delimited string or array of string/number values that define the path which values
   * change will be subscribed to and checked using the predicate.
   *
   * @returns The `controlValueChanges` subscription.
   */
  disableWhenTrue(target: string | (string | number)[], source: string | (string | number)[]): Subscription {
      return this.toggleDisabled(target, source, (value) => value === true);
  }

  /**
   * Subscribes to the `valuesChanges` of the source, resets the control state and toggles disabled
   * given the target's name or path when the predicate returns false, null, undefined or empty.
   *
   * @param target A dot-delimited string or array of string/number values that define the path to the
   * target control to be disabled.
   *
   * @param source A dot-delimited string or array of string/number values that define the path which values
   * change will be subscribed to and checked using the predicate.
   *
   * @returns The `controlValueChanges` subscription.
   */
  disableWhenFalseOrEmpty(target: string | (string | number)[], source: string | (string | number)[]): Subscription {
      return this.toggleDisabled(target, source, (value) => {
          return value === null || value === undefined || value === '' || value === false;
      });
  }

  /**
   * Subscribes to the valuesChanges of the source, resets the control state and toggles disabled
   * given the target's name or path when the predicate returns less than or equals to zero.
   *
   * @param target A dot-delimited string or array of string/number values that define the path to the
   * target control to be disabled.
   *
   * @param source A dot-delimited string or array of string/number values that define the path which values
   * change will be subscribed to and checked using the predicate.
   *
   * @returns The `controlValueChanges` subscription.
   */
  disableWhenLessThanOne(target: string | (string | number)[], source: string | (string | number)[]): Subscription {
      return this.toggleDisabled(target, source, (value) => {
          return value === null || value === undefined || value <= 0;
      });
  }

  /**
   * Creates a `FormControl` based on the current disabled state of the form.
   *
   * @param value The initial value of the `FormControl`.
   *
   * @param opts A synchronous validator function, or an array of such functions, or an `AbstractControlOptions`
   * object that contains validation functions and a validation trigger.
   *
   * @param asyncValidator A single async validator or array of async validator functions.
   *
   * @returns The `FormControl`.
   */
  createFormControl(
      value?: any,
      opts?: ValidatorFn | ValidatorFn[] | AbstractControlOptions,
      asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[]) {
      return new FormControl({ value, disabled: this.disabled }, opts, asyncValidator);
  }

  /**
   * Patches the `FormArray` removing than invoking a callback function
   * to add the new items given value and control's name.
   *
   * @param key The control's name of the FormArray to be patched.
   * @param value The value that will patch the `FormArray`.
   * @param callback The method that will be invoked when patching the `FormArray`.
   */
  patchFormArray(key: string, value: any[], callback: (item: any) => void) {
      const arr = this.get(key) as FormArray;
      while (arr.length) { arr.removeAt(0); }

      if (value && value.length) {
          value.forEach((item: any) => callback.call(this, item));
      }
  }
}
