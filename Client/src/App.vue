<script lang="ts">
    import Navbar from "@/components/Navbar/Navbar.vue";
    export default {
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
