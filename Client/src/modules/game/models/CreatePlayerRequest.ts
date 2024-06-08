export default class CreatePlayerRequest {
    userId: string;
    displayName: string;

    constructor(userId: string, displayName: string) {
        this.userId = userId;
        this.displayName = displayName;
    }
}
