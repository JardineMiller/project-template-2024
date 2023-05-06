/**
 * Must match Validation.cs in the backend.
 * TODO: Should we actually get these values from the backend and save the duplication?
 */

import Auth from "./validation.auth";
import Game from "./validation.game";
import User from "./validation.user";

export default {
    Auth: Auth,
    Game: Game,
    User: User,
};
