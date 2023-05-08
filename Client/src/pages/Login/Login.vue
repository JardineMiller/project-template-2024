<script lang="ts">
    import ModelProperty, {
        PropertyEvent,
        PropertyValueChangedEvent,
    } from "@/models/state/ModelProperty";
    import PasswordInput from "@/components/PasswordInput/PasswordInput.vue";
    import TextInput from "@/components/TextInput/TextInput.vue";
    import { Validators } from "@/models/validation/Validators";
    import StateTracker from "@/models/state/StateTracker";
    import LoginModel from "@/models/forms/LoginModel";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput, PasswordInput },
        data: () => {
            return {
                state: new StateTracker<LoginModel>(
                    new LoginModel(
                        new ModelProperty<string>(
                            "email",
                            undefined,
                            [
                                Validators.required(),
                                Validators.email(),
                            ]
                        ),
                        new ModelProperty<string>(
                            "password",
                            undefined,
                            [Validators.required()]
                        )
                    ),
                    true
                ),
            };
        },
        computed: {
            email(): ModelProperty<string> {
                return this.state.getProperty("email");
            },
            password(): ModelProperty<string> {
                return this.state.getProperty("password");
            },
        },
        methods: {
            onValueChange<T>(event: PropertyValueChangedEvent<T>) {
                this.state.setProperty<T>(
                    event.propertyName,
                    event.value
                );
            },
            onBlur(event: PropertyEvent) {
                this.state.touchProperty(event.propertyName);
            },
            handleSubmit(): void {},
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
                        @on-value-change="onValueChange"
                        @on-blur="onBlur"
                    />
                </div>

                <!-- Password -->
                <div class="field">
                    <PasswordInput
                        v-model="password"
                        @on-value-change="onValueChange"
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

            <pre>
                    {{ JSON.stringify(state.model, null, 2) }}
            </pre>
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
