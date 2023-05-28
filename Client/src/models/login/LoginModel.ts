import LoginRequest from "@/modules/auth/models/LoginRequest";
import { ModelBuilder } from "@/models/builders/ModelBuilder";
import type ModelProperty from "@/models/state/ModelProperty";
import type { IModelConstructor } from "@/models/base/IModel";
import type IRequestModel from "@/models/IRequestModel";
import BaseModel from "@/models/base/BaseModel";

export default class LoginModel
    extends BaseModel
    implements IRequestModel<LoginModel, LoginRequest>
{
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

    toRequest(): LoginRequest {
        return new LoginRequest(
            this.email.value!,
            this.password.value!
        );
    }
}
