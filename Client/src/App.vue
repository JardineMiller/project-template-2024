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
        <div>
            <RouterView />
        </div>
    </div>
</template>

<style></style>
