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
    <div v-if="user"><GameComponent></GameComponent></div>
    <div v-else>
        <CreatePlayer> </CreatePlayer>
    </div>
</template>

<style scoped></style>
