<script lang="ts">
    import TextInput from "@/components/TextInput/TextInput.vue";
    import Validation from "@/validation/validation";
    import { Validators } from "@/models/Validator";
    import FormInput from "@/models/FormInput";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput },
        data: () => {
            return {
                model: {
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
                    ]),
                },
                submitted: false,
                passwordRegex: Validation.Auth.Password.Pattern,
            };
        },
        computed: {
            formIsValid(): boolean {
                const inputs = Object.values(
                    this.model
                ) as Array<FormInput>;
                return inputs.every((x) => x.isValid);
            },
        },
        methods: {
            updateIsValid(emitted: { name: string; value: boolean }) {
                const inputs = Object.values(
                    this.model
                ) as Array<FormInput>;
                const input = inputs.find(
                    (x) =>
                        x.propertyName ==
                        emitted.name.toLocaleLowerCase()
                );
                if (input) {
                    input.isValid = emitted.value;
                }
            },
            updateValue(emitted: { name: string; value: string }) {
                const inputs = Object.values(
                    this.model
                ) as Array<FormInput>;
                const input = inputs.find(
                    (x) =>
                        x.propertyName ==
                        emitted.name.toLocaleLowerCase()
                );
                if (input) {
                    input.value = emitted.value;
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
                    <!-- Username-->
                    <div class="field">
                        <TextInput
                            v-model="model.email.value"
                            :name="model.email.propertyName"
                            :label="model.email.propertyName"
                            :validators="model.email.validators"
                            @update-is-valid="updateIsValid"
                            @update-value="updateValue"
                        />
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <span class="p-float-label">
                            <Password
                                id="password"
                                v-model="model.password.value"
                                :class="{
                                    'p-invalid': submitted,
                                }"
                                toggleMask
                                :strong-regex="passwordRegex"
                            >
                                <template #header>
                                    <h6>Pick a password</h6>
                                </template>
                                <template #footer="sp">
                                    {{ sp.level }}
                                    <Divider />
                                    <p class="m-2">Requirements:</p>
                                    <ul
                                        class="pl-2 ml-2 mt-0"
                                        style="line-height: 1.5"
                                    >
                                        <li>
                                            At least one lowercase
                                        </li>
                                        <li>
                                            At least one uppercase
                                        </li>
                                        <li>At least one numeric</li>
                                        <li>
                                            At least one special
                                            character
                                        </li>
                                        <li>Minimum 6 characters</li>
                                    </ul>
                                </template>
                            </Password>
                            <label
                                for="password"
                                :class="{
                                    'p-error': submitted,
                                }"
                            >
                                Password
                                <span class="p-error">*</span>
                            </label>
                        </span>
                        <small
                            v-if="submitted"
                            class="p-error"
                        >
                            This is an error
                        </small>
                    </div>

                    <!-- Submit -->
                    <Button
                        type="submit"
                        label="Submit"
                        class="mt-2"
                        :disabled="!formIsValid"
                    />
                </form>
                <pre>
                    {{ JSON.stringify(model, null, 2) }}
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

    .field {
        margin-bottom: 1.5rem;
    }

    @media screen and (max-width: 840px) {
        .card {
            width: 80%;
        }
    }
</style>
