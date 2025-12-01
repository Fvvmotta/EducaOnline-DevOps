export interface JwtPayload {
  nome: string;
  aud: string;
  email: string;
  exp: number;
  iat: number;
  iss: string;
  nameid: string;
  nbf: number;
  role: string;
  id: string;
}
