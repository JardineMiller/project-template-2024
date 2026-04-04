export default class UpdateUserRequest {
    email: string;
    displayName: string;
    bio: string;
    avatarUrl?: string;

    constructor(email: string, displayName: string, bio: string, avatarUrl?: string) {
        this.email = email;
        this.bio = bio;
        this.displayName = displayName;
        this.avatarUrl = avatarUrl;
    }
}
