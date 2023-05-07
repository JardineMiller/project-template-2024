import type ModelProperty from "@/models/state/ModelProperty";

export interface IModel {
    get(propertyName: string): ModelProperty;
    get properties(): ModelProperty[];
    get isValid(): boolean;
}
