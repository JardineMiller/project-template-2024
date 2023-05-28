<script lang="ts">
    import Navbar from "@/modules/home/components/Navbar/Navbar.vue";

    export default {
        head: {
            titleTemplate: `%s - ${import.meta.env.VITE_APP_TITLE}`,
        },
        components: {
            NavBar: Navbar,
        },
    };
</script>

<template>
    <div class="surface-ground h-screen">
        <NavBar></NavBar>
        <RouterView />
    </div>
</template>

<style></style>
