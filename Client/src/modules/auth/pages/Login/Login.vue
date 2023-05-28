<script lang="ts">
    import { Validators } from "@/models/validation/Validators";
    import ModelProperty from "@/models/state/ModelProperty";
    import StateTracker from "@/models/state/StateTracker";
    import LoginModel from "@/models/login/LoginModel";
    import Validation from "@/validation/validation";
    import Auth from "@/modules/auth/services/Auth";
    import "@/utils/extensions/string-extensions";
    import Message from "primevue/message";
    import { defineComponent } from "vue";
    import "@/validation/validation";

    export default defineComponent({
        components: {
            Message,
        },
        data: () => {
            return {
                invalidCredentials: false,
                passwordRegex:
                    Validation.Auth.Password.Pattern.toString(),
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
                    new ModelProperty<boolean>("rememberMe", false),
                ]),
                { trackChanges: false }
            );
        },
        computed: {
            email(): ModelProperty<string> {
                return this.state.model.email;
            },
            password(): ModelProperty<string> {
                return this.state.model.password;
            },
            rememberMe(): ModelProperty<boolean> {
                return this.state.model.rememberMe;
            },
        },
        methods: {
            handleSubmit(): void {
                this.loading = true;
                Auth.login(this.state.model.toRequest())
                    .catch(
                        (_: Error) => (this.invalidCredentials = true)
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
            class="surface-card p-4 shadow-2 border-round w-full lg:w-5 md:w-8">
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
                <a
                    class="font-medium no-underline ml-2 text-blue-500 cursor-pointer">
                    Create today!
                </a>
            </div>

            <form @submit.prevent="handleSubmit()">
                <Message
                    severity="error"
                    v-if="invalidCredentials"
                    @close="invalidCredentials = false">
                    Login failed: Invalid credentials
                </Message>

                <!-- Email -->
                <div class="field mt-5">
                    <span class="p-float-label">
                        <InputText
                            :id="email.propertyName.toLowerCase()"
                            :name="email.propertyName.toLowerCase()"
                            :model-value="email.value"
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
                            :id="password.propertyName.toLowerCase()"
                            :model-value="password.value"
                            class="w-full"
                            :input-class="'w-full'"
                            :class="{
                                'p-invalid':
                                    password.touched &&
                                    !password.isValid,
                            }"
                            :strong-regex="passwordRegex"
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

                <!-- Remember Me -->
                <div
                    class="flex align-items-center justify-content-between mb-6">
                    <div class="flex align-items-center">
                        <Checkbox
                            :id="
                                rememberMe.propertyName.toLowerCase()
                            "
                            :binary="true"
                            v-model="rememberMe.value"
                            class="mr-2">
                        </Checkbox>
                        <label
                            :for="
                                rememberMe.propertyName.toLowerCase()
                            ">
                            Remember me
                        </label>
                    </div>
                    <a
                        class="font-medium no-underline ml-2 text-blue-500 text-right cursor-pointer">
                        Forgot password?
                    </a>
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
