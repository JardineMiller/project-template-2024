<script lang="ts">
    import CreatePlayer from "@/modules/game/pages/components/CreatePlayer.vue";
    import Auth from "@/modules/auth/services/Auth";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "Game",
        components: { CreatePlayer },
        computed: {
            user() {
                return Auth.user.value;
            },
        },
    });
</script>

<template>
    <div v-if="user?.playerId">Allo poppet</div>
    <div v-else>
        <CreatePlayer> </CreatePlayer>
    </div>
</template>

<style scoped></style>
