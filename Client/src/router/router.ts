import { createRouter, createWebHistory } from "vue-router";
import Home from "@/modules/home/pages/Home.vue";
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
            import("@/modules/auth/pages/Login/Login.vue"),
    },
];

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: routes,
});

router.beforeEach(async (to, from) => {
    if (!Auth.isAuthenticated() && to.name !== "login") {
        return { name: "login" };
    }
});

export { routes };
export default router;
