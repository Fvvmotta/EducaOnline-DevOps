import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({providedIn: 'root'})
export class ErrorHandler {

    private erros: Subject<string[]> = new Subject<string[]>();
    erros$ = this.erros.asObservable();

    private notifyComponent = new Subject<void>();
    errorStopLoading$ = this.notifyComponent.asObservable();


    handleError(erros: string[]): void {
        this.erros.next(erros ?? ["Erro desconhecido, por favor contate o administrador do sistema!"]);
        this.notifyComponent.next();
    }
}
