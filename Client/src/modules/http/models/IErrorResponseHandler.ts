import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";

export default interface IErrorResponseHandler {
    get responseErrors(): { [key: string]: boolean };
    handleErrorResponse(error: HttpErrorResponse): void;
}
