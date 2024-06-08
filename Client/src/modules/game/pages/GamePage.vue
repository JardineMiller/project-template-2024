<script lang="ts">
    import GameComponent from "@/modules/game/pages/components/GameComponent.vue";
    import CreatePlayer from "@/modules/game/pages/components/CreatePlayer.vue";
    import Auth from "@/modules/auth/services/Auth";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "Game",
        components: { GameComponent, CreatePlayer },
        data() {
            return {
                gameId: "abcdef",
            };
        },
        computed: {
            user() {
                return Auth.user.value;
            },
        },
        methods: {},
    });
</script>

<template>
    <GameComponent v-if="user"></GameComponent>
    <CreatePlayer v-else> </CreatePlayer>
</template>

<style scoped></style>
