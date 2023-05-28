import { createRouter, createWebHistory } from "vue-router";
import Home from "@/modules/common/pages/Home.vue";
import Auth from "@/modules/auth/services/Auth";

const routes = [
    {
        path: "/",
        name: "home",
        component: Home,
    },
    {
        path: "/login",
        name: "login",
        component: () =>
            import("@/modules/auth/pages/login/Login.vue"),
    },
    {
        path: "/register",
        name: "register",
        component: () =>
            import("@/modules/auth/pages/register/Register.vue"),
    },
    {
        path: "/confirm",
        name: "confirm",
        component: () =>
            import("@/modules/auth/pages/confirm/Confirm.vue"),
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routes,
});

const anonymousRoutes = ["login", "register", "confirm"];

router.beforeEach(async (to, from) => {
    if (
        !Auth.isAuthenticated() &&
        !anonymousRoutes.includes(to.name?.toString() ?? "")
    ) {
        return { name: "login" };
    }
});

export { routes };
export default router;
