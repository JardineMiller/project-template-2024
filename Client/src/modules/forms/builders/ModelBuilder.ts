import { ModelPropertyBuilder } from "@/modules/forms/builders/ModelPropertyBuilder";
import type ModelProperty from "@/modules/forms/models/ModelProperty";
import type {
    IModel,
    IModelConstructor,
} from "@/modules/common/models/IModel";

export default class ModelBuilder<T extends IModel> {
    private readonly _modelConstructor: IModelConstructor<T>;
    private readonly _properties: Array<ModelProperty<any>> = [];

    constructor(modelConstructor: IModelConstructor<T>) {
        this._modelConstructor = modelConstructor;
    }

    public property<K>(
        propertyName: string
    ): ModelPropertyBuilder<T, K> {
        return new ModelPropertyBuilder<T, K>(this, propertyName);
    }

    public add<K>(property: ModelProperty<K>) {
        this._properties.push(property);
    }

    public build(): T {
        return new this._modelConstructor(this._properties);
    }
}
