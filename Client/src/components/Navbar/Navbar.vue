<template>
    <div class="navbar">
        <RouterLink
            data-test-id="navbar-link-home"
            class="link"
            to="/">
            Home
        </RouterLink>
        <RouterLink
            data-test-id="navbar-link-login"
            class="link"
            to="/login">
            Login
        </RouterLink>
        <Button
            @click="logout()"
            label="Logout"
            plain
            text />
    </div>
</template>

<script>
    import Auth from "@/features/auth/services/Auth";
    import { RouterLink } from "vue-router";
    import { defineComponent } from "vue";
    import Button from "primevue/button";

    export default defineComponent({
        username: "NavBar",
        components: { RouterLink, Button },
        methods: {
            logout() {
                Auth.logout();
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
