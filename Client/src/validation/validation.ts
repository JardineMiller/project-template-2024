/**
 * Must match Validation.cs in the backend. TODO: Should we actually get these values from the backend and save the duplication?
 */
export default {
    Auth: {
        Password: {
            MinLength: 6,
            MaxLength: 50,
            Pattern:
                "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,50}$",
        },
    },

    Game: {
        Name: {
            MinLength: 3,
            MaxLength: 100,
        },

        Description: {
            MinLength: 3,
            MaxLength: 250,
        },

        Code: {
            MaxLength: 25,
        },
    },

    User: {
        FirstName: {
            MinLength: 2,
            MaxLength: 50,
        },

        LastName: {
            MinLength: 2,
            MaxLength: 50,
        },
    },
};
