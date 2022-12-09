<script lang="ts">
    import Navbar from "@/components/Navbar/Navbar.vue";
    export default {
        components: {
            NavBar: Navbar,
        },
    };
</script>

<template>
    <NavBar></NavBar>
    <RouterView />
</template>

<style></style>
