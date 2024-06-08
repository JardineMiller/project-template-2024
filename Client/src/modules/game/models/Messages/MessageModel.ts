import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type IRequestModel from "@/modules/http/models/IRequestModel";
import BaseModel from "@/modules/common/models/BaseModel";

export default class MessageModel
    extends BaseModel
    implements
        IRequestModel<
            MessageModel,
            { user: string; message: string }
        >
{
    constructor(properties: Array<ModelProperty<any>>) {
        super(properties);
    }

    get message(): ModelProperty<string> {
        return this.get<string>("message");
    }

    get user(): ModelProperty<string> {
        return this.get<string>("user");
    }

    get gameCode(): ModelProperty<string> {
        return this.get<string>("gameCode");
    }

    toRequest(): { gameCode: string; user: string; message: string } {
        return {
            gameCode: this.gameCode.value!,
            user: this.user.value!,
            message: this.message.value!,
        };
    }
}
