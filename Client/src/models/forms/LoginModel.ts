import type ModelProperty from "@/models/state/ModelProperty";
import type { IModel } from "@/models/state/IModel";

export default class LoginModel implements IModel {
    email: ModelProperty;
    password: ModelProperty;

    constructor(email: ModelProperty, password: ModelProperty) {
        this.email = email;
        this.password = password;
    }

    get(propertyName: string): ModelProperty {
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

    get properties(): ModelProperty[] {
        return [this.email, this.password];
    }
}
