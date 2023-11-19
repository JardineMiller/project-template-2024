<script lang="ts">
    import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
    import CreatePlayerModel from "@/modules/game/models/CreatePlayerModel";
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import { Validators } from "@/modules/forms/validation/Validators";
    import ModelProperty from "@/modules/forms/models/ModelProperty";
    import type User from "@/modules/auth/models/common/User";
    import Player from "@/modules/game/services/Player";
    import Auth from "@/modules/auth/services/Auth";
    import InputText from "primevue/inputtext";
    import type { AxiosError } from "axios";
    import { defineComponent } from "vue";
    import Button from "primevue/button";

    export default defineComponent({
        name: "CreatePlayer",
        components: {
            InputText,
            Button,
        },
        data: () => {
            return {
                loading: false,
                state: {} as StateTracker<CreatePlayerModel>,
            };
        },
        created() {
            this.state = new StateTracker<CreatePlayerModel>(
                new CreatePlayerModel([
                    new ModelProperty<string>(
                        "displayName",
                        undefined,
                        [
                            Validators.required(),
                            Validators.minLength(2),
                            Validators.maxLength(100),
                        ]
                    ),
                    new ModelProperty<string>(
                        "displayName",
                        this.user?.id
                    ),
                ])
            );
        },
        computed: {
            displayName(): ModelProperty<string> {
                return this.state.model.displayName;
            },
            user(): User | null {
                return Auth.user.value;
            },
        },
        methods: {
            handleSubmit(): void {
                this.loading = true;
                Player.create(this.state.model.toRequest())
                    .then((res) => {
                        Auth.setPlayerId(res.data.playerId);
                    })
                    .catch((err: AxiosError) => {
                        this.state.model.handleErrorResponse(
                            err.response?.data as HttpErrorResponse
                        );
                    })
                    .finally(() => {});
            },
        },
    });
</script>

<template>
    <div
        class="px-4 py-8 md:px-6 lg:px-6 flex align-items-center justify-content-center">
        <div
            class="surface-card p-4 shadow-2 border-round w-full lg:w-4 md:w-8">
            <div class="text-center">
                <div class="text-900 text-3xl font-medium mb-3">
                    Join Game
                </div>
                <span class="text-600 font-medium line-height-3">
                    Please enter your display name
                </span>
            </div>

            <form @submit.prevent="handleSubmit()">
                <!-- Display Name -->
                <div class="field mt-5">
                    <span class="p-float-label">
                        <InputText
                            :id="
                                displayName.propertyName.toLowerCase()
                            "
                            :name="
                                displayName.propertyName.toLowerCase()
                            "
                            :model-value="displayName.value"
                            :autocomplete="
                                displayName.propertyName.toLowerCase()
                            "
                            class="w-full"
                            :class="{
                                'p-invalid':
                                    displayName.touched &&
                                    !displayName.isValid,
                            }"
                            @input="
                                this.state.setProperty<string>(
                                    displayName.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="displayName.touch()" />
                        <label
                            :for="
                                displayName.propertyName.toLowerCase()
                            ">
                            {{
                                displayName.propertyName.toTitleCase()
                            }}
                            <span
                                v-if="displayName.isRequired"
                                class="p-error">
                                *
                            </span>
                        </label>
                    </span>
                    <div
                        v-if="
                            displayName.touched &&
                            !displayName.isValid
                        ">
                        <small
                            v-for="error in displayName.errors"
                            :key="error"
                            class="p-error">
                            {{ error }} <br />
                        </small>
                    </div>
                </div>

                <!-- Submit -->
                <Button
                    class="w-full"
                    type="submit"
                    label="Submit"
                    :loading="loading"
                    :disabled="!state.model.isValid">
                </Button>
            </form>
        </div>
    </div>
</template>

<style scoped></style>
