import type IErrorResponseHandler from "@/modules/http/models/IErrorResponseHandler";
import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
import type { IModelConstructor } from "@/modules/common/models/IModel";
import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type IRequestModel from "@/modules/http/models/IRequestModel";
import LoginRequest from "@/modules/auth/models/login/LoginRequest";
import ModelBuilder from "@/modules/forms/builders/ModelBuilder";
import BaseModel from "@/modules/common/models/BaseModel";

export default class LoginModel
    extends BaseModel
    implements
        IRequestModel<LoginModel, LoginRequest>,
        IErrorResponseHandler
{
    private _errors: { [key: string]: boolean } = {
        invalidCredentials: false,
        emailNotConfirmed: false,
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

    static Builder(): ModelBuilder<LoginModel> {
        return new ModelBuilder<LoginModel>(
            LoginModel as IModelConstructor<LoginModel>
        );
    }

    get responseErrors(): { [key: string]: boolean } {
        return this._errors;
    }

    handleErrorResponse(error: HttpErrorResponse): void {
        // TODO: [HTTP-C01] Make use of some universal/global error handling here
        if (!Object.keys(error.errors).length) {
            return;
        }

        if (error.errors["Auth.InvalidCredentials"]) {
            this._errors.invalidCredentials = true;
        }
        if (error.errors["Auth.EmailNotConfirmed"]) {
            this._errors.emailNotConfirmed = true;
        }
    }

    toRequest(): LoginRequest {
        return new LoginRequest(
            this.email.value!,
            this.password.value!
        );
    }
}
