﻿export default class User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    playerId?: string;

    constructor(
        id: string,
        firstName: string,
        lastName: string,
        email: string,
        playerId?: string
    ) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.playerId = playerId;
    }
}
