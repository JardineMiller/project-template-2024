import { createRouter, createWebHistory } from "vue-router";
import Auth from "@/features/auth/services/Auth";
import HomeView from "../pages/Home/Home.vue";

const routes = [
    {
        path: "/",
        name: "home",
        component: HomeView,
    },
    {
        path: "/login",
        name: "login",
        component: () => import("../pages/Login/Login.vue"),
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
