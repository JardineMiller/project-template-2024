export default class User {
    id: string;
    displayName: string;
    email: string;
    avatarUrl?: string;

    constructor(
        id: string,
        displayName: string,
        email: string,
        avatarUrl?: string
    ) {
        this.id = id;
        this.displayName = displayName;
        this.email = email;
        this.avatarUrl = avatarUrl;
    }
}
