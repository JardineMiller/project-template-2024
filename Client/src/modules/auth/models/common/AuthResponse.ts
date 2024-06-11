export default class AuthenticationResponse {
    id: string;
    displayName: string;
    email: string;
    token?: string;
    avatarUrl?: string;

    constructor(
        id: string,
        displayName: string,
        email: string,
        token?: string,
        avatarUrl?: string
    ) {
        this.id = id;
        this.displayName = displayName;
        this.email = email;
        this.token = token;
        this.avatarUrl = avatarUrl;
    }
}
