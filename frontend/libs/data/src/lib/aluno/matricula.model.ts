export interface MatriculaModel {
    cursoId: any;
}

export interface MatriculaResponseModel {
    alunoId: string;
    cursoId: string;
    cursoNome: string;
    totalAulas: number;
    cargaHorariaTotal: number;
    dataMatricula: Date;
    status: string;
    aulasConcluidas: number;
}