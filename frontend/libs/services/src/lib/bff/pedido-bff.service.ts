import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../configuration';
import { BffResponseModel, PagamentoCartaoModel } from '@educa-online/data';

@Injectable()
export class PedidoBffService {

    constructor(private httpClient: HttpClient) { }


    realizarPedido(model: PagamentoCartaoModel): Observable<BffResponseModel> {
        return this.httpClient.post<BffResponseModel>(`${environment.alunoBff}/pedido-bff/compras/pedido`, model)
    }
    
}