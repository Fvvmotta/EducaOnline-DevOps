export interface BffResponseModel {
    title: string;
    status: number;
    errors: ResponseErrorMessages;
}

export interface ResponseErrorMessages {
    messages: string[]
}