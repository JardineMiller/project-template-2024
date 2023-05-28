<script lang="ts">
    import ProgressSpinner from "primevue/progressspinner";
    import Auth from "@/modules/auth/services/Auth";
    import { defineComponent } from "vue";

    export default defineComponent({
        head: {
            title: "Confirm",
        },
        name: "Confirm",
        components: {
            ProgressSpinner,
        },
        data: () => {
            return {};
        },
        computed: {
            token() {
                return this.$route.query.token?.toString() ?? "";
            },
            email() {
                return this.$route.query.email?.toString() ?? "";
            },
        },
        created() {
            Auth.confirm(this.token, this.email);
        },
    });
</script>

<template>
    <div
        class="px-4 py-8 md:px-6 lg:px-6 flex h-screen align-items-center justify-content-center">
        <div class="flex flex-column">
            <ProgressSpinner />
            <h5 class="py-4">Confirming email...</h5>
        </div>
    </div>
</template>

<style scoped></style>
