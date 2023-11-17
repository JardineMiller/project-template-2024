import ModelProperty from "@/modules/forms/models/ModelProperty";
import type { IModel } from "@/modules/common/models/IModel";

export default abstract class BaseModel implements IModel {
    private readonly properties: Array<ModelProperty<any>>;

    protected constructor(properties: Array<ModelProperty<any>>) {
        this.properties = properties;
    }

    get<T>(propertyName: string): ModelProperty<T> {
        const property = this.properties.find(
            (x) =>
                x.propertyName === propertyName &&
                x instanceof ModelProperty<T>
        );

        if (!property) {
            throw new Error(
                `Cannot find property with name ${propertyName}`
            );
        }

        return property as ModelProperty<T>;
    }

    get isValid(): boolean {
        return this.properties.every((x) => x.isValid);
    }
}
