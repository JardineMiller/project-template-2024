import { createRouter, createWebHistory } from "vue-router";
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

export { routes };
export default router;
