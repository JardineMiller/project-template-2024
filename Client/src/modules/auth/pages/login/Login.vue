<script lang="ts">
    import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import { Validators } from "@/modules/forms/validation/Validators";
    import ModelProperty from "@/modules/forms/models/ModelProperty";
    import LoginModel from "@/modules/auth/models/login/LoginModel";
    import "@/utils/extensions/string/string-extensions";
    import Auth from "@/modules/auth/services/Auth";
    import InputText from "primevue/inputtext";
    import Password from "primevue/password";
    import { RouterLink } from "vue-router";
    import type { AxiosError } from "axios";
    import Message from "primevue/message";
    import { defineComponent } from "vue";
    import Button from "primevue/button";
    import "@/validation/validation";

    export default defineComponent({
        head: {
            title: "Login",
        },
        components: {
            Button,
            InputText,
            Message,
            Password,
            RouterLink,
        },
        data: () => {
            return {
                state: {} as StateTracker<LoginModel>,
                loading: false,
            };
        },
        created() {
            this.state = new StateTracker<LoginModel>(
                new LoginModel([
                    new ModelProperty<string>("email", undefined, [
                        Validators.required(),
                        Validators.email(),
                    ]),
                    new ModelProperty<string>("password", undefined, [
                        Validators.required(),
                    ]),
                ])
            );
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
            handleSubmit(): void {
                this.loading = true;
                Auth.login(this.state.model.toRequest())
                    .catch((error: AxiosError) =>
                        this.state.model.handleErrorResponse(
                            error.response?.data as HttpErrorResponse
                        )
                    )
                    .finally(() => {
                        this.loading = false;
                    });
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
                <img
                    src="@/assets/logo.svg"
                    alt="Image"
                    height="50"
                    class="mb-3" />
                <div class="text-900 text-3xl font-medium mb-3">
                    Welcome Back
                </div>
                <span class="text-600 font-medium line-height-3">
                    Don't have an account?
                </span>
                <RouterLink
                    class="font-medium no-underline ml-2 text-blue-500 cursor-pointer"
                    to="/register">
                    Create one
                </RouterLink>
            </div>

            <form @submit.prevent="handleSubmit()">
                <Message
                    severity="error"
                    v-if="
                        state.model.responseErrors.invalidCredentials
                    "
                    @close="
                        state.model.responseErrors.invalidCredentials = false
                    ">
                    Login failed: Invalid credentials
                </Message>

                <Message
                    severity="warn"
                    v-if="
                        state.model.responseErrors.emailNotConfirmed
                    "
                    @close="
                        state.model.responseErrors.emailNotConfirmed = false
                    ">
                    Login failed: Email not confirmed
                </Message>

                <!-- Email -->
                <div class="field mt-5">
                    <span class="p-float-label">
                        <InputText
                            :id="email.propertyName.toLowerCase()"
                            :name="email.propertyName.toLowerCase()"
                            :model-value="email.value"
                            :autocomplete="
                                email.propertyName.toLowerCase()
                            "
                            class="w-full"
                            :class="{
                                'p-invalid':
                                    email.touched && !email.isValid,
                            }"
                            @input="
                                this.state.setProperty<string>(
                                    email.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="email.touch()" />
                        <label
                            :for="email.propertyName.toLowerCase()">
                            {{ email.propertyName.toTitleCase() }}
                            <span
                                v-if="email.isRequired"
                                class="p-error">
                                *
                            </span>
                        </label>
                    </span>
                    <div v-if="email.touched && !email.isValid">
                        <small
                            v-for="error in email.errors"
                            :key="error"
                            class="p-error">
                            {{ error }} <br />
                        </small>
                    </div>
                </div>

                <!-- Password -->
                <div class="field">
                    <span class="p-float-label">
                        <Password
                            :name="
                                password.propertyName.toLowerCase()
                            "
                            :model-value="password.value"
                            class="w-full"
                            :input-class="'w-full'"
                            :input-props="{
                                id: password.propertyName.toLowerCase(),
                                autocomplete:
                                    password.propertyName.toLowerCase(),
                            }"
                            :class="{
                                'p-invalid':
                                    password.touched &&
                                    !password.isValid,
                            }"
                            @input="
                                this.state.setProperty<string>(
                                    password.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="password.touch()"
                            toggleMask
                            :feedback="false">
                        </Password>

                        <label
                            :for="
                                password.propertyName.toLowerCase()
                            ">
                            {{ password.propertyName.toTitleCase() }}
                            <span
                                v-if="password.isRequired"
                                class="p-error">
                                *
                            </span>
                        </label>
                    </span>
                    <div v-if="password.touched && !password.isValid">
                        <small
                            v-for="error in password.errors"
                            :key="error"
                            class="p-error">
                            {{ error }} <br />
                        </small>
                    </div>
                </div>

                <!-- Submit -->
                <Button
                    class="w-full"
                    icon="pi pi-user"
                    type="submit"
                    label="Sign In"
                    :loading="loading"
                    :disabled="!state.model.isValid">
                </Button>
            </form>
        </div>
    </div>
</template>

<style scoped>
    form .field {
        margin-bottom: 2rem;
    }
</style>
