export interface ModalInfoModel {
  modalSucesso?: boolean;
  btnSucessoText?: string;
  btnSucessoIcon?: string;
  titulo: string;
  texto: string;
  btnOk: string;
  btnCancel: string;
  btn: BtnType;
}

export type BtnType = 'danger' | 'sucess' | 'warning' | 'secondary';
