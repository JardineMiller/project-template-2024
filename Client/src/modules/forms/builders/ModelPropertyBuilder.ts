import type { IValidator } from "@/modules/forms/validation/IValidator";
import type ModelBuilder from "@/modules/forms/builders/ModelBuilder";
import { Validators } from "@/modules/forms/validation/Validators";
import ModelProperty from "@/modules/forms/models/ModelProperty";
import type {IModel} from "@/modules/common/models/IModel";

export class ModelPropertyBuilder<T extends IModel, K> {
    private readonly _propertyName: string;
    private _value: K | undefined = undefined;
    private _validators: Array<IValidator<K>> = [];
    private _isRequired: boolean = false;
    private readonly _modelBuilder: ModelBuilder<T>;

    constructor(modelBuilder: ModelBuilder<T>, propertyName: string) {
        this._modelBuilder = modelBuilder;
        this._propertyName = propertyName;
    }

    public value(value: K): ModelPropertyBuilder<T, K> {
        this._value = value;
        return this;
    }

    public required(): ModelPropertyBuilder<T, K> {
        this._validators.push(Validators.required());
        return this;
    }

    public validators(
        validators: Array<IValidator<K>>
    ): ModelPropertyBuilder<T, K> {
        this._validators = this._validators.concat(validators);
        return this;
    }

    public buildProperty(): ModelBuilder<T> {
        this._modelBuilder.add(this.build());
        return this._modelBuilder;
    }

    private build(): ModelProperty<K> {
        return new ModelProperty<K>(
            this._propertyName,
            this._value,
            this._validators
        );
    }
}
