export default class AuthenticationResponse {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    token?: string;
    playerId?: string;

    constructor(
        id: string,
        firstName: string,
        lastName: string,
        email: string,
        token?: string,
        playerId?: string
    ) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.token = token;
        this.playerId = playerId;
    }
}
