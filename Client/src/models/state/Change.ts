export class Change {
    propertyName: string;
    oldVal: any;
    newVal: any;

    constructor(propertyName: string, oldVal: any, newVal: any) {
        this.propertyName = propertyName;
        this.oldVal = oldVal;
        this.newVal = newVal;
    }
}

export class Changes {
    private index?: number = undefined;
    readonly changes: Change[] = [];

    add(change: Change): void {
        this.changes.push(change);
        this.increment();
    }

    increment(): void {
        if (this.index === undefined) {
            this.index = 0;
        }

        this.index++;

        if (this.index > this.changes.length - 1) {
            this.index = this.changes.length - 1;
        }
    }

    decrement(): void {
        if (this.index === undefined) {
            this.index = 0;
        }

        this.index--;

        if (this.index < 0) {
            this.index = undefined;
        }
    }

    clear(): void {
        this.changes.splice(0);
        this.index = undefined;
    }

    getCurrentChange(): Change | undefined {
        if (this.changes.length === 0) {
            return undefined;
        }

        if (this.index === undefined) {
            this.index = 0;
        }

        return this.changes[this.index];
    }

    get canUndo(): boolean {
        return (
            this.index !== undefined &&
            this.changes.length > 0 &&
            this.index >= 0
        );
    }

    get canRedo(): boolean {
        return (
            this.changes.length > 0 &&
            (this.index === undefined ||
                this.changes.length - 1 > this.index)
        );
    }
}
