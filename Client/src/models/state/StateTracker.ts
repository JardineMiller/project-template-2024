import type ModelProperty from "@/models/state/ModelProperty";
import { Change, Changes } from "@/models/state/Change";
import type { IModel } from "@/models/state/IModel";

export default class StateTracker<T extends IModel> {
    private readonly changes = new Changes();

    trackChanges: boolean;
    model: T;

    constructor(model: T, trackChanges: boolean = false) {
        this.model = model;
        this.trackChanges = trackChanges;
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

    getProperty<T>(propertyName: string): ModelProperty<T> {
        return this.model.get(propertyName);
    }

    setProperty<T>(propertyName: string, newValue: T): void {
        const property = this.getProperty<T>(propertyName);

        if (!property) {
            return;
        }

        const change = new Change(
            propertyName,
            property.value,
            newValue
        );

        property.value = change.newVal;
        property.touched = true;

        this.changes.add(change);
    }

    touchProperty(propertyName: string): void {
        const property = this.getProperty<T>(propertyName);

        if (!property) {
            return;
        }

        property.touched = true;
    }
}
