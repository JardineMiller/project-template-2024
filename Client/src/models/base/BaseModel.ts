import type ModelProperty from "@/models/state/ModelProperty";
import type { IModel } from "@/models/base/IModel";

export default abstract class BaseModel implements IModel {
    private readonly properties: Array<ModelProperty<any>>;

    protected constructor(properties: Array<ModelProperty<any>>) {
        this.properties = properties;
    }

    get(propertyName: string): ModelProperty<any> {
        const property = this.properties.find(
            (x) => x.propertyName === propertyName
        );

        if (!property) {
            throw new Error(
                `Cannot find property with name ${propertyName}`
            );
        }

        return property;
    }

    get isValid(): boolean {
        return this.properties.every((x) => x.isValid);
    }
}
