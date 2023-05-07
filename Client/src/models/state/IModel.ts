import type ModelProperty from "@/models/state/ModelProperty";

export interface IModel {
    get(propertyName: string): ModelProperty<any>;
    get properties(): ModelProperty<any>[];
    get isValid(): boolean;
}
