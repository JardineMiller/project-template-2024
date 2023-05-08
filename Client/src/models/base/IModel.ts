import type ModelProperty from "@/models/state/ModelProperty";

export interface IModelConstructor<T extends IModel> {
    new (properties: Array<ModelProperty<any>>): T;
}

export interface IModel {
    get(propertyName: string): ModelProperty<any>;
    get isValid(): boolean;
}
