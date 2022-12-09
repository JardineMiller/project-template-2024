<template>
    <div class="navbar">
        <RouterLink
            class="link"
            to="/"
            >Home</RouterLink
        >
        <RouterLink
            class="link"
            to="/login"
            >Login</RouterLink
        >
    </div>
</template>

<script>
    export default {
        username: "NavBar",
        components: {},
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
