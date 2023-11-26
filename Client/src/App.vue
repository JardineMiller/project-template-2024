<script lang="ts">
    import Navbar from "@/modules/common/components/Navbar/Navbar.vue";

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
    <div class="min-h-screen surface-ground">
        <NavBar></NavBar>
        <div style="height: calc(100vh - 70px)">
            <RouterView />
        </div>
    </div>
</template>

<style></style>
