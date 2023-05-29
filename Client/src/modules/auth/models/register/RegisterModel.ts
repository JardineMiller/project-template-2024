import type IErrorResponseHandler from "@/modules/http/models/IErrorResponseHandler";
import RegisterRequest from "@/modules/auth/models/register/RegisterRequest";
import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type IRequestModel from "@/modules/http/models/IRequestModel";
import BaseModel from "@/modules/common/models/BaseModel";

export default class RegisterModel
    extends BaseModel
    implements
        IRequestModel<RegisterModel, RegisterRequest>,
        IErrorResponseHandler
{
    private _errors: { [key: string]: boolean } = {
        duplicateEmail: false,
        creationFailed: false,
    };

    constructor(properties: Array<ModelProperty<any>>) {
        super(properties);
    }

    get email(): ModelProperty<string> {
        return this.get<string>("email");
    }

    get password(): ModelProperty<string> {
        return this.get<string>("password");
    }

    get firstName(): ModelProperty<string> {
        return this.get<string>("firstName");
    }

    get lastName(): ModelProperty<string> {
        return this.get<string>("lastName");
    }

    toRequest(): RegisterRequest {
        return new RegisterRequest(
            this.email.value!,
            this.password.value!,
            this.firstName.value!,
            this.lastName.value!
        );
    }

    handleErrorResponse(error: HttpErrorResponse): void {
        //
        if (error.status === 409) {
            this._errors.duplicateEmail = true;
            return;
        }

        this._errors.creationFailed = true;
    }

    get responseErrors(): { [p: string]: boolean } {
        return this._errors;
    }
}
