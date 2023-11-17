<script lang="ts">
    import type User from "@/modules/auth/models/common/User";
    import Auth from "@/modules/auth/services/Auth";
    import { defineComponent } from "vue";
    export default defineComponent({
        head: {
            title: "Home",
        },
        computed: {
            user(): User | null {
                return Auth.user.value;
            },
        },
    });
</script>

<template>
    <div class="p-4">
        <h4 v-if="user">
            Welcome {{ user.firstName }} {{ user.lastName }}
        </h4>
    </div>
</template>

<style scoped></style>
