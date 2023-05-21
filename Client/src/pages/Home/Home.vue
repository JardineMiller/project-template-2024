<script lang="ts">
    import type User from "@/features/auth/models/User";
    import Auth from "@/features/auth/services/Auth";
    import { defineComponent } from "vue";
    export default defineComponent({
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
