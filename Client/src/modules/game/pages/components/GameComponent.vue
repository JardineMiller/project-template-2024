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
        head: {
            title: "Chat",
        },
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
                    timestamp: Date;
                    messageId: string;
                    likes: number;
                }>(),
                likedMessages: new Set<string>(),
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
                        Validators.required(),
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

            window.addEventListener("beforeunload", this.unload);
        },
        async mounted() {
            GameHub.registerReceiveMessageHandler(
                this.receiveMessage
            );
            GameHub.registerReceiveLikeMessageHandler(
                this.receiveLike
            );
            GameHub.registerReceiveUnlikeMessageHandler(
                this.receiveUnlike
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
            this.unload();
        },
        methods: {
            async unload() {
                GameHub.leaveGame(
                    this.gameId,
                    this.user?.firstName ?? "Anonymous"
                );

                await GameHub.disconnect();
            },
            receiveMessage(
                user: string,
                message: string,
                timestamp: Date,
                messageId: string
            ) {
                this.messages.push({
                    user: user,
                    message: message,
                    timestamp: timestamp,
                    messageId: messageId,
                    likes: 0,
                });
            },
            receiveLike(messageId: string) {
                const message = this.messages.find(
                    (x) => x.messageId === messageId
                );

                if (message) {
                    message.likes++;
                }
            },
            receiveUnlike(messageId: string) {
                const message = this.messages.find(
                    (x) => x.messageId === messageId
                );

                if (message) {
                    message.likes--;
                }
            },
            playerConnected(playerName: string, playerId: string) {
                this.players.push(playerName);
            },
            playerDisconnected(playerName: string, playerId: string) {
                this.players.filter((x) => x !== playerName);
            },
            toggleLike(messageId: string) {
                if (!this.likedMessages.has(messageId)) {
                    this.likeMessage(messageId);
                    return;
                }

                this.unlikeMessage(messageId);
            },
            likeMessage(messageId: string) {
                const message = this.messages.find(
                    (x) => x.messageId === messageId
                );

                if (
                    !message ||
                    message.user == this.user?.firstName ||
                    this.likedMessages.has(messageId)
                ) {
                    return;
                }

                GameHub.likeMessage(this.gameId, messageId);
                this.likedMessages.add(messageId);
            },
            unlikeMessage(messageId: string) {
                const message = this.messages.find(
                    (x) => x.messageId === messageId
                );

                if (
                    !message ||
                    message.user == this.user?.firstName ||
                    !this.likedMessages.has(messageId)
                ) {
                    return;
                }

                GameHub.unlikeMessage(this.gameId, messageId);
                this.likedMessages.delete(messageId);
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

                this.state.model.message.clear();
            },
        },
        watch: {},
    });
</script>

<template>
    <div
        class="block-content justify-content-center flex w-full max-h-full">
        <div
            class="p-4 sm:w-8 md:w-6 lg:w-4 max-h-full flex flex-column">
            <div class="overflow-y-auto max-h-full">
                <!-- Messages -->
                <div
                    class="mx-3 flex"
                    :class="{ 'mt-5': index > 0 }"
                    v-for="(message, index) in messages"
                    v-bind:key="index">
                    <div
                        class="flex flex-column align-items-center"></div>
                    <div
                        class="surface-card shadow-2 border-round p-3 flex-auto">
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
                                {{
                                    message.timestamp.toLocaleDateString()
                                }}
                                {{
                                    message.timestamp.toLocaleTimeString()
                                }}
                            </span>
                        </div>
                        <div class="line-height-3 text-700 mb-3">
                            {{ message.message }}
                        </div>
                        <div
                            class="text-500 flex align-items-center gap-4">
                            <div
                                class="flex align-items-center gap-1 cursor-pointer hover:text-red-400"
                                @click="
                                    toggleLike(message.messageId)
                                ">
                                <i
                                    :class="{
                                        'pi-heart-fill text-red-400':
                                            likedMessages.has(
                                                message.messageId
                                            ),
                                        'pi-heart':
                                            !likedMessages.has(
                                                message.messageId
                                            ),
                                    }"
                                    class="pi">
                                </i>
                                <span class="mr-3">{{
                                    message.likes
                                }}</span>
                            </div>
                            <div
                                class="flex align-items-center gap-1">
                                <i class="pi pi-comment"></i>
                                <span class="mr-3">1</span>
                            </div>
                            <div
                                class="flex align-items-center gap-1">
                                <i class="pi pi-eye"></i>
                                <span>24</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Message -->
            <form
                @submit.prevent="sendMessage()"
                class="mt-5 border-round w-full">
                <div class="p-inputgroup">
                    <InputText
                        :id="message.propertyName.toLowerCase()"
                        :name="message.propertyName.toLowerCase()"
                        :model-value="message.value"
                        :placeholder="
                            message.propertyName.toTitleCase()
                        "
                        :autocomplete="
                            message.propertyName.toLowerCase()
                        "
                        class="w-full"
                        @input="
                            this.state.setProperty<string>(
                                message.propertyName,
                                $event.target.value
                            )
                        "
                        @blur="message.touch()" />

                    <Button
                        type="submit"
                        icon="pi pi-send"
                        :disabled="!state.model.isValid">
                    </Button>
                </div>
            </form>
        </div>
    </div>
</template>

<style scoped></style>
