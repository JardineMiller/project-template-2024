import type IErrorResponseHandler from "@/modules/http/models/IErrorResponseHandler";
import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
import UpdateUserRequest from "@/modules/user/models/UpdateUserRequest";
import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type IRequestModel from "@/modules/http/models/IRequestModel";
import BaseModel from "@/modules/common/models/BaseModel";

export default class UpdateUserModel
    extends BaseModel
    implements
        IRequestModel<UpdateUserModel, UpdateUserRequest>,
        IErrorResponseHandler
{
    constructor(properties: Array<ModelProperty<any>>) {
        super(properties);
    }

    get email(): ModelProperty<string> {
        return this.get<string>("email");
    }

    get bio(): ModelProperty<string> {
        return this.get<string>("bio");
    }

    get displayName(): ModelProperty<string> {
        return this.get<string>("displayName");
    }

    handleErrorResponse(error: HttpErrorResponse): void {}

    get responseErrors(): { [p: string]: boolean } {
        return {};
    }

    toRequest(): UpdateUserRequest {
        return new UpdateUserRequest(
            this.email.value!,
            this.displayName.value!,
            this.bio.value!
        );
    }
}
