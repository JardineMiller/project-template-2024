import { createRouter, createWebHistory } from "vue-router";
import Auth from "@/modules/auth/services/Auth";

const routes = {
    home: {
        path: "/",
        name: "home",
        component: () => import("@/modules/common/pages/Home.vue"),
    },
    login: {
        path: "/login",
        name: "login",
        component: () =>
            import("@/modules/auth/pages/login/Login.vue"),
    },
    register: {
        path: "/register",
        name: "register",
        component: () =>
            import("@/modules/auth/pages/register/Register.vue"),
    },
    confirm: {
        path: "/confirm",
        name: "confirm",
        component: () =>
            import("@/modules/auth/pages/confirm/Confirm.vue"),
    },
};

const routesArray = Object.values(routes);

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routesArray,
});

const anonymousRoutes = [
    routes.login.name,
    routes.register.name,
    routes.confirm.name,
];

router.beforeEach(async (to, from) => {
    if (!Auth.user.value) {
        await Auth.refreshToken();
    }

    if (
        !Auth.user.value &&
        !anonymousRoutes.includes(to.name?.toString() ?? "")
    ) {
        return { name: routes.login.name };
    }
});

export { routes };

export default router;
