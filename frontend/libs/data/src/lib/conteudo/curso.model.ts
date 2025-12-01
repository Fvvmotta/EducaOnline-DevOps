import { AulaResponseModel } from "./aula.model";

export interface CursoResponseModel {
  id: string;
  nome: string;
  ativo: boolean;
  valor: number;
  conteudoProgramatico: ConteudoProgramaticoResponseModel;
  aulas: AulaResponseModel[];
}

export interface CursoModel {
  id: string;
  nome: string;
  ativo: boolean;
  valor: number;
  conteudoProgramatico: ConteudoProgramaticoResponseModel;
}

export interface ConteudoProgramaticoResponseModel {
  titulo: string;
  descricao: string;
  objetivos: string;
  cargaHoraria: number;
}
