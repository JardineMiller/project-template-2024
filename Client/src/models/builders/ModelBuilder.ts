import { ModelPropertyBuilder } from "@/models/builders/ModelPropertyBuilder";
import type { IModel, IModelConstructor } from "@/models/base/IModel";
import type ModelProperty from "@/models/state/ModelProperty";

export class ModelBuilder<T extends IModel> {
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
