import type ModelProperty from "@/models/state/ModelProperty";
import type { IModel } from "@/models/state/IModel";

export default class LoginModel implements IModel {
    email: ModelProperty<string>;
    password: ModelProperty<string>;

    constructor(
        email: ModelProperty<string>,
        password: ModelProperty<string>
    ) {
        this.email = email;
        this.password = password;
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

    get properties(): ModelProperty<any>[] {
        return [this.email, this.password];
    }
}
