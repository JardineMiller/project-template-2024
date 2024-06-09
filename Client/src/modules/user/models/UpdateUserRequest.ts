export default class UpdateUserRequest {
    email: string;
    displayName: string;
    bio: string;

    constructor(email: string, displayName: string, bio: string) {
        this.email = email;
        this.bio = bio;
        this.displayName = displayName;
    }
}
