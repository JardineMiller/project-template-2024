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
    </div>
</template>

<script>
    import { RouterLink } from "vue-router";

    export default {
        username: "NavBar",
        components: { RouterLink },
    };
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
