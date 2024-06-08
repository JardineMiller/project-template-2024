import type IErrorResponseHandler from "@/modules/http/models/IErrorResponseHandler";
import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
import CreatePlayerRequest from "@/modules/game/models/CreatePlayerRequest";
import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type IRequestModel from "@/modules/http/models/IRequestModel";
import BaseModel from "@/modules/common/models/BaseModel";

export default class CreatePlayerModel
    extends BaseModel
    implements
        IRequestModel<CreatePlayerModel, CreatePlayerRequest>,
        IErrorResponseHandler
{
    private _errors: { [key: string]: boolean } = {
        invalidCredentials: false,
        emailNotConfirmed: false,
    };

    constructor(properties: Array<ModelProperty<any>>) {
        super(properties);
    }

    get displayName(): ModelProperty<string> {
        return this.get<string>("displayName");
    }

    get userId(): ModelProperty<string> {
        return this.get<string>("userId");
    }

    get responseErrors(): { [key: string]: boolean } {
        return this._errors;
    }

    handleErrorResponse(error: HttpErrorResponse): void {
        // TODO: [HTTP-C01] Make use of some universal/global error handling here
        if (!Object.keys(error.errors).length) {
            return;
        }
    }

    toRequest(): CreatePlayerRequest {
        return new CreatePlayerRequest(
            this.userId.value!,
            this.displayName.value!
        );
    }
}
