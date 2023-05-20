import type ModelProperty from "@/models/state/ModelProperty";

export interface IModelConstructor<T extends IModel> {
    new (properties: Array<ModelProperty<any>>): T;
}

export interface IModel {
    get<T>(propertyName: string): ModelProperty<T>;
    get isValid(): boolean;
}
