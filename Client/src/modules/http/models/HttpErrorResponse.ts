export default interface HttpErrorResponse {
    errors: { [key: string]: string[] };
    status: number;
    title: string;
    traceId: string;
    type: string;
}
