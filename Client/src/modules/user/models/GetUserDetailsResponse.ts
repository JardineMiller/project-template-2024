export default class GetUserDetailsResponse {
    id: string;
    displayName: string;
    email: string;
    bio?: string;
    avatarUrl?: string;

    constructor(
        id: string,
        displayName: string,
        email: string,
        bio?: string,
        avatarUrl?: string
    ) {
        this.id = id;
        this.displayName = displayName;
        this.email = email;
        this.bio = bio;
        this.avatarUrl = avatarUrl;
    }
}
