<script lang="ts">
    import MessageModel from "@/modules/game/models/Messages/MessageModel";
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import { Validators } from "@/modules/forms/validation/Validators";
    import ModelProperty from "@/modules/forms/models/ModelProperty";
    import { HubConnectionState } from "@microsoft/signalr";
    import GameHub from "@/modules/game/signalr/GameHub";
    import Auth from "@/modules/auth/services/Auth";
    import InputText from "primevue/inputtext";
    import { defineComponent } from "vue";
    import Button from "primevue/button";
    import Avatar from "primevue/avatar";

    export default defineComponent({
        name: "GameComponent",
        components: {
            InputText,
            Button,
            Avatar,
        },
        data() {
            return {
                messages: new Array<{
                    user: string;
                    message: string;
                }>(),
                players: new Array<string>(),
                input: "",
                gameId: "abcdef",
                state: {} as StateTracker<MessageModel>,
            };
        },
        computed: {
            user() {
                return Auth.user.value;
            },
            message(): ModelProperty<string> {
                return this.state.model.message;
            },
        },
        created() {
            this.state = new StateTracker<MessageModel>(
                new MessageModel([
                    new ModelProperty<string>("message", "", [
                        Validators.minLength(2),
                        Validators.maxLength(100),
                    ]),
                    new ModelProperty<string>(
                        "user",
                        this.user?.firstName
                    ),
                    new ModelProperty<string>(
                        "gameCode",
                        this.gameId
                    ),
                ])
            );
        },
        async mounted() {
            GameHub.registerReceiveMessageHandler(
                this.receiveMessage
            );
            GameHub.registerPlayerConnectedHandler(
                this.playerConnected
            );
            GameHub.registerPlayerDisconnectedHandler(
                this.playerDisconnected
            );

            if (
                GameHub.connectionState() !==
                HubConnectionState.Connected
            ) {
                await GameHub.connect();
            }

            GameHub.joinGame(
                this.gameId,
                this.user?.firstName ?? "Anonymous"
            );
        },
        async unmounted() {
            GameHub.leaveGame(
                this.gameId,
                this.user?.firstName ?? "Anonymous"
            );

            await GameHub.disconnect();
        },
        methods: {
            receiveMessage(user: string, message: string) {
                this.messages.push({ user: user, message: message });
            },
            playerConnected(playerName: string, playerId: string) {
                this.players.push(playerName);
            },
            playerDisconnected(playerName: string, playerId: string) {
                this.players.filter((x) => x !== playerName);
            },
            sendMessage() {
                if (
                    GameHub.connectionState() !==
                    HubConnectionState.Connected
                ) {
                    return;
                }

                const message = this.state.model.toRequest();

                GameHub.sendMessage(
                    message.gameCode,
                    message.user,
                    message.message
                );
            },
        },
        watch: {},
    });
</script>

<template>
    <div class="block-content">
        <div class="px-4 pt-8 md:px-6 lg:px-8">
            <div
                class="mb-5 flex"
                v-for="(message, index) in messages"
                v-bind:key="index">
                <div
                    class="flex flex-column align-items-center"></div>
                <div
                    class="ml-5 surface-card shadow-2 border-round p-3 flex-auto">
                    <div class="mb-3">
                        <Avatar
                            :label="message.user[0]"
                            class="mr-2 cursor-pointer"
                            size="medium"
                            style="
                                background-color: #2196f3;
                                color: #ffffff;
                            "
                            shape="circle" />

                        <span
                            class="text-900 font-medium inline-block mr-3">
                            {{ message.user }}
                        </span>
                        <span class="text-500 text-sm">
                            1 minute ago
                        </span>
                    </div>
                    <div class="line-height-3 text-700 mb-3">
                        {{ message.message }}
                    </div>
                    <div
                        class="text-500 flex align-items-center gap-4">
                        <div class="flex align-items-center gap-1">
                            <i class="pi pi-heart"></i>
                            <span class="mr-3">0</span>
                        </div>
                        <div class="flex align-items-center gap-1">
                            <i class="pi pi-comment"></i>
                            <span class="mr-3">1</span>
                        </div>
                        <div class="flex align-items-center gap-1">
                            <i class="pi pi-eye"></i>
                            <span>24</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Message -->
    <div
        class="px-4 py-8 md:px-6 lg:px-6 flex align-items-center justify-content-center">
        <div
            class="surface-card p-4 shadow-2 border-round w-full lg:w-6 md:w-8">
            <div class="surface-card"></div>

            <div class="field mt-5">
                <span class="p-float-label">
                    <InputText
                        :id="message.propertyName.toLowerCase()"
                        :name="message.propertyName.toLowerCase()"
                        :model-value="message.value"
                        :autocomplete="
                            message.propertyName.toLowerCase()
                        "
                        class="w-full"
                        :class="{
                            'p-invalid':
                                message.touched && !message.isValid,
                        }"
                        @input="
                            this.state.setProperty<string>(
                                message.propertyName,
                                $event.target.value
                            )
                        "
                        @blur="message.touch()" />
                    <label :for="message.propertyName.toLowerCase()">
                        {{ message.propertyName.toTitleCase() }}
                        <span
                            v-if="message.isRequired"
                            class="p-error">
                            *
                        </span>
                    </label>
                </span>
                <div v-if="message.touched && !message.isValid">
                    <small
                        v-for="error in message.errors"
                        :key="error"
                        class="p-error">
                        {{ error }} <br />
                    </small>
                </div>
            </div>

            <!-- Submit -->
            <Button
                class="w-full"
                type="button"
                label="Send"
                @click="sendMessage()"
                :disabled="!state.model.isValid">
            </Button>
        </div>
    </div>
</template>

<style scoped></style>
