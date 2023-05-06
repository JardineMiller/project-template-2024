<script lang="ts">
    import PasswordInput from "@/components/PasswordInput/PasswordInput.vue";
    import TextInput from "@/components/TextInput/TextInput.vue";
    import { LoginForm, StateModel } from "@/models/StateModel";
    import FormInput from "@/models/forms/FormInput";
    import Validation from "@/validation/validation";
    import { Validators } from "@/models/Validator";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput, PasswordInput },
        data: () => {
            return {
                state: new StateModel<LoginForm>(
                    new LoginForm(
                        new FormInput("email", "", [
                            Validators.required(),
                            Validators.email(),
                        ]),
                        new FormInput("password", "", [
                            Validators.required(),
                            Validators.minLength(
                                Validation.Auth.Password.MinLength
                            ),
                            Validators.maxLength(
                                Validation.Auth.Password.MaxLength
                            ),
                            Validators.pattern(
                                new RegExp(
                                    Validation.Auth.Password.Pattern
                                )
                            ),
                        ])
                    ),
                    true
                ),
            };
        },
        methods: {
            onValueChange(emitted: { name: string; value: string }) {
                const inputs = Object.values(
                    this.state.model
                ) as Array<FormInput>;

                const input = inputs.find(
                    (x) =>
                        x.propertyName == emitted.name.toLowerCase()
                );

                if (input) {
                    this.state.makeChange(
                        emitted.name,
                        emitted.value,
                        true
                    );
                    input.touched = true;
                }
            },
            onBlur(emitted: { name: string }) {
                const inputs = Object.values(
                    this.state.model
                ) as Array<FormInput>;

                const input = inputs.find(
                    (x) =>
                        x.propertyName == emitted.name.toLowerCase()
                );

                if (input) {
                    input.touched = true;
                }
            },
            handleSubmit(): void {},
        },
    });
</script>

<template>
    <div class="form-demo">
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
                            v-model="state.model.email"
                            @on-value-change="onValueChange"
                            @on-blur="onBlur"
                        />
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <PasswordInput
                            v-model="state.model.password"
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

                <div
                    class="mt-3"
                    style="
                        position: absolute;
                        z-index: 9999;
                        top: 0;
                        right: 0;
                    "
                    v-if="state.allowUndo"
                >
                    <Button
                        icon="pi pi-undo"
                        severity="secondary"
                        outlined
                        aria-label="Undo"
                        v-tooltip.top="'Undo'"
                        @click="state.undo()"
                        :disabled="!state.hasChanges"
                    />

                    <Button
                        icon="pi pi-undo"
                        severity="secondary"
                        outlined
                        aria-label="Redo"
                        v-tooltip.top="'Redo'"
                        :style="'transform: scale(-1, 1)'"
                        @click="state.redo()"
                        :disabled="!state.hasUndoneChanges"
                    />
                    <Button
                        type="button"
                        label="Clear"
                        outlined
                        @click="state.clear()"
                        :disabled="
                            !state.hasChanges &&
                            !state.hasUndoneChanges
                        "
                    />
                </div>

                <pre>
                    {{ JSON.stringify(state.model, null, 2) }}
                </pre>
            </div>
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
