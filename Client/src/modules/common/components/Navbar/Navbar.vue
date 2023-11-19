<template>
    <div
        v-if="user"
        class="surface-overlay py-3 px-4 shadow-2 flex align-items-center justify-content-between relative lg:static"
        style="min-height: 80px">
        <img
            src="@/assets/logo.svg"
            alt="Logo"
            height="40"
            class="mr-0 lg:mr-6" />
        <a
            class="cursor-pointer block lg:hidden text-700 mt-1 p-ripple">
            <i class="pi pi-bars text-4xl"></i>
            <span
                class="p-ink"
                role="presentation"
                aria-hidden="true">
            </span
        ></a>
        <RouterLink
            class="text-4xl"
            to="/game/abc">
            Join Game
        </RouterLink>
        <div
            class="align-items-center flex-grow-1 justify-content-end hidden lg:flex absolute lg:static w-full surface-overlay left-0 top-100 z-1 shadow-2 lg:shadow-none">
            <ul
                class="list-none p-0 m-0 flex lg:align-items-center select-none flex-column lg:flex-row border-top-1 surface-border lg:border-top-none">
                <li
                    class="border-top-1 surface-border lg:border-top-none">
                    <Avatar
                        @click="toggleMenu"
                        aria-haspopup="true"
                        aria-controls="overlay_menu"
                        :label="user.firstName[0]"
                        class="mr-2 cursor-pointer"
                        size="large"
                        style="
                            background-color: #2196f3;
                            color: #ffffff;
                        "
                        shape="circle" />
                    <Menu
                        :model="items"
                        :popup="true"
                        ref="menu"
                        id="overlay_menu"
                        class="absolute right-0">
                        <template #end>
                            <button
                                @click="logout()"
                                class="w-full p-link flex align-items-center p-2 pl-4 text-color hover:surface-200 border-noround">
                                <i class="pi pi-sign-out" />
                                <span class="ml-2">Log Out</span>
                            </button>
                        </template>
                    </Menu>
                    <Toast />
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    import Auth from "@/modules/auth/services/Auth";
    import { routes } from "@/router/router";
    import { RouterLink } from "vue-router";
    import { defineComponent } from "vue";
    import Button from "primevue/button";
    import Avatar from "primevue/avatar";
    import Toast from "primevue/toast";
    import Menu from "primevue/menu";

    export default defineComponent({
        username: "NavBar",
        components: { RouterLink, Button, Avatar, Menu, Toast },
        data: () => {
            return {
                routes: routes,
                items: [
                    {
                        label: "Profile",
                        icon: "pi pi-fw pi-user",
                        to: "/profile",
                    },
                ],
            };
        },
        computed: {
            user() {
                return Auth.user.value;
            },
        },
        methods: {
            logout() {
                Auth.logout();
            },
            toggleMenu(event) {
                this.$refs.menu.toggle(event);
            },
        },
    });
</script>

<style scoped>
    .navbar {
        width: 100%;
        min-height: 3rem;
        display: flex;
        align-items: center;
    }

    .navbar .link {
        padding: 1rem;
    }
</style>
