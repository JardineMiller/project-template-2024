import type { ModelBuilder } from "@/models/builders/ModelBuilder";
import type { IValidator } from "@/models/validation/IValidator";
import ModelProperty from "@/models/state/ModelProperty";
import type { IModel } from "@/models/base/IModel";

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
        this._isRequired = true;
        return this;
    }

    public validators(
        validators: Array<IValidator<K>>
    ): ModelPropertyBuilder<T, K> {
        this._validators = validators;
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
            this._isRequired,
            this._validators
        );
    }
}
