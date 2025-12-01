import { MatriculaResponseModel } from "./matricula.model";

export interface AlunoResponseModel {
    id: string;
    nome: string;
    email: string;
    dataCadastro: string;
}

export interface AlunoMatriculaResponseModel{
    matriculas: MatriculaResponseModel[];
}