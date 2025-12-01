export interface AlertOptions {
  title?: string;
  subtitle?: string;
  status?: AlertStatus;
}

export type AlertStatus = 'sucesso' | 'erro' | 'aviso' | 'info';
