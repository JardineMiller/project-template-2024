import { ModelBuilder } from "@/models/builders/ModelBuilder";
import type ModelProperty from "@/models/state/ModelProperty";
import type { IModelConstructor } from "@/models/base/IModel";
import BaseModel from "@/models/base/BaseModel";

export default class LoginModel extends BaseModel {
    constructor(properties: Array<ModelProperty<any>>) {
        super(properties);
    }

    static Builder(): ModelBuilder<LoginModel> {
        return new ModelBuilder<LoginModel>(
            LoginModel as IModelConstructor<LoginModel>
        );
    }
}
