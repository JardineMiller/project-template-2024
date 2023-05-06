import type FormInput from "@/models/forms/FormInput";

class Change {
    propertyName: string;
    oldVal: any;
    newVal: any;

    constructor(propertyName: string, oldVal: any, newVal: any) {
        this.propertyName = propertyName;
        this.oldVal = oldVal;
        this.newVal = newVal;
    }
}

interface IStateModel {
    get(propertyName: string): FormInput;
}

export class StateModel<T extends IStateModel> {
    private readonly changes: Change[] = [];
    private readonly undoneChanges: Change[] = [];

    allowUndo: boolean;
    model: T;

    constructor(model: T, allowUndo: boolean = false) {
        this.model = model;
        this.allowUndo = allowUndo;
    }

    get hasChanges(): boolean {
        return this.changes.length > 0;
    }

    get hasUndoneChanges(): boolean {
        return this.undoneChanges.length > 0;
    }

    undo(): void {
        if (!this.hasChanges) {
            return;
        }

        const change = this.changes.pop();

        if (!change) {
            return;
        }

        this.model.get(change.propertyName).value = change.oldVal;
        this.undoneChanges.push(change);
    }

    redo(): void {
        if (!this.hasUndoneChanges) {
            return;
        }

        const change = this.undoneChanges.pop();

        if (!change) {
            return;
        }

        this.model.get(change.propertyName).value = change.newVal;
        this.changes.push(change);
    }

    clear(): void {
        while (this.hasUndoneChanges) {
            this.redo();
        }

        while (this.hasChanges) {
            this.undo();
        }

        this.undoneChanges.splice(0);
        this.changes.splice(0);
    }

    makeChange(
        propertyName: string,
        value: any,
        clearUndone: boolean = false
    ) {
        const oldVal = this.model.get(propertyName).value;
        const change = new Change(propertyName, oldVal, value);

        this.model.get(change.propertyName).value = change.newVal;
        this.changes.push(change);

        if (clearUndone) {
            this.undoneChanges.splice(0);
        }
    }
}

export class LoginForm implements IStateModel {
    email: FormInput;
    password: FormInput;
    [key: string]: any;

    constructor(email: FormInput, password: FormInput) {
        this.email = email;
        this.password = password;
    }

    get(propertyName: string): FormInput {
        return this[propertyName] as FormInput;
    }

    get isValid(): boolean {
        return [this.email, this.password].every((x) => x.isValid);
    }
}
