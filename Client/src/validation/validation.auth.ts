export default {
    Password: {
        MinLength: 6,
        MaxLength: 50,
        Pattern:
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,50}$",
    },
};
