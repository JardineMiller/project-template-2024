export default class AuthenticationResponse {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    token: string | null;

    constructor(
        id: string,
        firstName: string,
        lastName: string,
        email: string,
        token: string
    ) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.token = token;
    }
}
