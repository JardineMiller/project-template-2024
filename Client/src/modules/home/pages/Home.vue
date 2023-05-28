<script lang="ts">
    import type User from "@/modules/auth/models/User";
    import Auth from "@/modules/auth/services/Auth";
    import { defineComponent } from "vue";
    export default defineComponent({
        head: {
            title: "Home",
        },
        computed: {
            user(): User | null {
                return Auth.user();
            },
        },
    });
</script>

<template>
    <h1>Welcome {{ user.firstName }} {{ user.lastName }}</h1>
</template>

<style scoped></style>
