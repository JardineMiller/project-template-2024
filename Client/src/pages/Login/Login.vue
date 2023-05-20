<script lang="ts">
    import type {
        ModelPropertyEvent,
        ModelPropertyChangeEvent,
    } from "@/models/state/ModelProperty";
    import PasswordInput from "@/components/PasswordInput/PasswordInput.vue";
    import TextInput from "@/components/TextInput/TextInput.vue";
    import { Validators } from "@/models/validation/Validators";
    import ModelProperty from "@/models/state/ModelProperty";
    import StateTracker from "@/models/state/StateTracker";
    import LoginModel from "@/models/login/LoginModel";
    import Auth from "@/features/auth/services/Auth";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput, PasswordInput },
        data: () => {
            return {
                state: new StateTracker<LoginModel>(
                    new LoginModel([
                        new ModelProperty<string>(
                            "email",
                            undefined,
                            true,
                            [Validators.email()]
                        ),
                        new ModelProperty<string>(
                            "password",
                            undefined,
                            true
                        ),
                    ]),
                    { trackChanges: true }
                ),
            };
        },
        computed: {
            email(): ModelProperty<string> {
                return this.state.model.email;
            },
            password(): ModelProperty<string> {
                return this.state.model.password;
            },
        },
        methods: {
            onChange<T>(event: ModelPropertyChangeEvent<T>) {
                this.state.setProperty<T>(
                    event.propertyName,
                    event.value
                );
            },
            onBlur<T>(event: ModelPropertyEvent<T>) {
                this.state.model.get<T>(event.propertyName).touch();
            },
            handleSubmit(): void {
                Auth.login(this.state.model.toRequest());
            },
        },
    });
</script>

<template>
    <div
        class="mt-3 mr-3 flex gap-1 absolute top-0 right-0"
        v-if="state.trackChanges"
    >
        <Button
            icon="pi pi-undo"
            severity="secondary"
            outlined
            aria-label="Undo"
            v-tooltip.top="'Undo'"
            @click="state.undo()"
            :disabled="!state.canUndo"
        />

        <Button
            icon="pi pi-undo"
            severity="secondary"
            outlined
            aria-label="Redo"
            v-tooltip.top="'Redo'"
            :style="'transform: scale(-1, 1)'"
            @click="state.redo()"
            :disabled="!state.canRedo"
        />
        <Button
            type="button"
            label="Clear"
            outlined
            @click="state.clear()"
            :disabled="!state.canUndo && !state.canRedo"
        />
    </div>

    <div class="flex justify-content-center align-content-center">
        <div class="card">
            <h5 class="text-center">Login</h5>
            <form
                @submit.prevent="handleSubmit()"
                class="p-fluid"
            >
                <!-- Email -->
                <div class="field">
                    <TextInput
                        v-model="email"
                        @on-change="onChange"
                        @on-blur="onBlur"
                    />
                </div>

                <!-- Password -->
                <div class="field">
                    <PasswordInput
                        v-model="password"
                        @on-change="onChange"
                        @on-blur="onBlur"
                    />
                </div>

                <!-- Submit -->
                <Button
                    type="submit"
                    label="Submit"
                    :disabled="!state.model.isValid"
                />
            </form>
        </div>
    </div>
</template>

<style scoped>
    .card {
        min-width: 450px;
    }

    form {
        margin-top: 2rem;
    }

    form .field {
        margin-bottom: 2rem;
    }

    @media screen and (max-width: 840px) {
        .card {
            width: 80%;
        }
    }
</style>
