import { Change, Changes } from "@/models/state/Change";
import type { IModel } from "@/models/base/IModel";

export default class StateTracker<T extends IModel> {
    private readonly changes = new Changes();

    trackChanges: boolean;
    model: T;

    constructor(
        model: T,
        options: { trackChanges: boolean } = { trackChanges: false }
    ) {
        this.model = model;
        this.trackChanges = options.trackChanges ?? false;
    }

    get canUndo(): boolean {
        return this.changes.canUndo;
    }

    get canRedo(): boolean {
        return this.changes.canRedo;
    }

    undo(): void {
        if (!this.canUndo) {
            return;
        }

        const change = this.changes.getCurrentChange();

        if (!change) {
            return;
        }

        this.model.get(change.propertyName).value = change.oldVal;
        this.changes.decrement();
    }

    redo(): void {
        if (!this.canRedo) {
            return;
        }

        this.changes.increment();

        const change = this.changes.getCurrentChange();

        if (!change) {
            return;
        }

        this.model.get(change.propertyName).value = change.newVal;
    }

    clear(): void {
        while (this.canRedo) {
            this.redo();
        }

        while (this.canUndo) {
            this.undo();
        }

        this.changes.clear();
    }

    setProperty<T>(propertyName: string, newValue: T): void {
        const property = this.model.get<T>(propertyName);

        if (!property) {
            return;
        }

        const change = new Change(
            propertyName,
            property.value,
            newValue
        );

        property.value = change.newVal;
        property.touch();

        this.changes.add(change);
    }
}
