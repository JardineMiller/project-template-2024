export default class AuthenticationResponse {
    id: string;
    displayName: string;
    email: string;
    token?: string;

    constructor(
        id: string,
        displayName: string,
        email: string,
        token?: string
    ) {
        this.id = id;
        this.displayName = displayName;
        this.email = email;
        this.token = token;
    }
}
