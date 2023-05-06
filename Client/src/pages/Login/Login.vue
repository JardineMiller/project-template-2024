<script lang="ts">
    import PasswordInput from "@/components/PasswordInput/PasswordInput.vue";
    import TextInput from "@/components/TextInput/TextInput.vue";
    import FormInput from "@/models/forms/FormInput";
    import Validation from "@/validation/validation";
    import { Validators } from "@/models/Validator";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput, PasswordInput },
        data: () => {
            return {
                form: {
                    email: new FormInput("email", "", [
                        Validators.required(),
                        Validators.email(),
                    ]),
                    password: new FormInput("password", "", [
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
                    ]),
                },
                passwordRegex: Validation.Auth.Password.Pattern,
            };
        },
        computed: {
            formIsValid(): boolean {
                const inputs = Object.values(
                    this.form
                ) as Array<FormInput>;
                return inputs.every((x) => x.isValid);
            },
        },
        methods: {
            updateValue(emitted: { name: string; value: string }) {
                const inputs = Object.values(
                    this.form
                ) as Array<FormInput>;

                const input = inputs.find(
                    (x) =>
                        x.propertyName == emitted.name.toLowerCase()
                );

                if (input) {
                    input.value = emitted.value;
                    input.touched = true;
                }
            },
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
                            v-model="form.email"
                            @update-value="updateValue"
                        />
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <PasswordInput
                            v-model="form.password"
                            @update-value="updateValue"
                        />
                    </div>

                    <!-- Submit -->
                    <Button
                        type="submit"
                        label="Submit"
                        :disabled="!formIsValid"
                    />
                </form>
                <pre>
                    {{ JSON.stringify(form, null, 2) }}
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
