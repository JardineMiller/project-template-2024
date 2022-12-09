import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../pages/Home/Home.vue";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
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
    ],
});

export default router;
