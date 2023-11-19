<script lang="ts">
    import type HttpErrorResponse from "@/modules/http/models/HttpErrorResponse";
    import RegisterModel from "@/modules/auth/models/register/RegisterModel";
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import { Validators } from "@/modules/forms/validation/Validators";
    import ModelProperty from "@/modules/forms/models/ModelProperty";
    import "@/utils/extensions/string/string-extensions";
    import Validation from "@/validation/validation";
    import Auth from "@/modules/auth/services/Auth";
    import InputText from "primevue/inputtext";
    import Password from "primevue/password";
    import { routes } from "@/router/router";
    import type { AxiosError } from "axios";
    import Message from "primevue/message";
    import Divider from "primevue/divider";
    import { defineComponent } from "vue";
    import Button from "primevue/button";

    export default defineComponent({
        head: {
            title: "Register",
        },
        name: "Register",
        components: {
            Button,
            Divider,
            InputText,
            Password,
            Message,
        },
        data: () => {
            return {
                loading: false,
                routes: routes,
                awaitingConfirmation: false,
                passwordRegex:
                    Validation.Auth.Password.Pattern.source,
                state: new StateTracker<RegisterModel>(
                    new RegisterModel([
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
                            [
                                Validators.required(),
                                Validators.pattern(
                                    Validation.Auth.Password.Pattern
                                ),
                                Validators.minLength(
                                    Validation.Auth.Password.MinLength
                                ),
                                Validators.maxLength(
                                    Validation.Auth.Password.MaxLength
                                ),
                            ]
                        ),
                        new ModelProperty<string>(
                            "firstName",
                            undefined,
                            [
                                Validators.required(),
                                Validators.minLength(
                                    Validation.User.FirstName
                                        .MinLength
                                ),
                                Validators.maxLength(
                                    Validation.User.FirstName
                                        .MaxLength
                                ),
                            ]
                        ),
                        new ModelProperty<string>(
                            "lastName",
                            undefined,
                            [
                                Validators.required(),
                                Validators.minLength(
                                    Validation.User.LastName.MinLength
                                ),
                                Validators.maxLength(
                                    Validation.User.LastName.MaxLength
                                ),
                            ]
                        ),
                    ])
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
            firstName(): ModelProperty<string> {
                return this.state.model.firstName;
            },
            lastName(): ModelProperty<string> {
                return this.state.model.lastName;
            },
        },
        methods: {
            handleSubmit(): void {
                this.loading = true;
                Auth.register(this.state.model.toRequest())
                    .then((_) => {
                        this.awaitingConfirmation = true;
                    })
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
                    Create Account
                </div>
                <span class="text-600 font-medium line-height-3">
                    Already have an account?
                </span>
                <RouterLink
                    class="font-medium no-underline ml-2 text-blue-500 cursor-pointer"
                    :to="routes.login.path">
                    Log in
                </RouterLink>
            </div>

            <form
                v-if="!awaitingConfirmation"
                @submit.prevent="handleSubmit()">
                <Message
                    severity="error"
                    v-if="state.model.responseErrors.duplicateEmail"
                    @close="
                        state.model.responseErrors.duplicateEmail = false
                    ">
                    An account with this email already exists
                </Message>

                <Message
                    severity="error"
                    v-if="state.model.responseErrors.creationFailed"
                    @close="
                        state.model.responseErrors.creationFailed = false
                    ">
                    Create Account Failed: Something went wrong
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
                            :medium-regex="'^(?=.{1000,})'"
                            :strong-regex="passwordRegex"
                            @input="
                                this.state.setProperty<string>(
                                    password.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="password.touch()"
                            toggleMask>
                            <template #header>
                                <h6 class="mb-2">Pick a password</h6>
                            </template>
                            <template #footer>
                                <Divider />
                                <p class="my-2 font-bold">
                                    Requirements:
                                </p>
                                <ul
                                    class="pl-2 ml-2 mt-0"
                                    style="line-height: 1.5">
                                    <li>At least one lowercase</li>
                                    <li>At least one uppercase</li>
                                    <li>At least one numeric</li>
                                    <li>
                                        At least one special character
                                    </li>
                                    <li>Minimum 6 characters</li>
                                    <li>Maximum 50 characters</li>
                                </ul>
                            </template>
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

                <!-- First Name -->
                <div class="field">
                    <span class="p-float-label">
                        <InputText
                            :id="firstName.propertyName.toLowerCase()"
                            :name="
                                firstName.propertyName.toLowerCase()
                            "
                            :model-value="firstName.value"
                            :autocomplete="
                                firstName.propertyName.toLowerCase()
                            "
                            class="w-full"
                            :class="{
                                'p-invalid':
                                    firstName.touched &&
                                    !firstName.isValid,
                            }"
                            @input="
                                this.state.setProperty<string>(
                                    firstName.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="firstName.touch()" />
                        <label
                            :for="
                                firstName.propertyName.toLowerCase()
                            ">
                            {{ firstName.propertyName.toTitleCase() }}
                            <span
                                v-if="firstName.isRequired"
                                class="p-error">
                                *
                            </span>
                        </label>
                    </span>
                    <div
                        v-if="
                            firstName.touched && !firstName.isValid
                        ">
                        <small
                            v-for="error in firstName.errors"
                            :key="error"
                            class="p-error">
                            {{ error }} <br />
                        </small>
                    </div>
                </div>

                <!-- Last Name -->
                <div class="field">
                    <span class="p-float-label">
                        <InputText
                            :id="lastName.propertyName.toLowerCase()"
                            :name="
                                lastName.propertyName.toLowerCase()
                            "
                            :model-value="lastName.value"
                            :autocomplete="
                                lastName.propertyName.toLowerCase()
                            "
                            class="w-full"
                            :class="{
                                'p-invalid':
                                    lastName.touched &&
                                    !lastName.isValid,
                            }"
                            @input="
                                this.state.setProperty<string>(
                                    lastName.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="lastName.touch()" />
                        <label
                            :for="
                                lastName.propertyName.toLowerCase()
                            ">
                            {{ lastName.propertyName.toTitleCase() }}
                            <span
                                v-if="lastName.isRequired"
                                class="p-error">
                                *
                            </span>
                        </label>
                    </span>
                    <div v-if="lastName.touched && !lastName.isValid">
                        <small
                            v-for="error in lastName.errors"
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
                    label="Create Account"
                    :loading="loading"
                    :disabled="!state.model.isValid">
                </Button>
            </form>
            <div v-if="awaitingConfirmation">
                <Message
                    severity="success"
                    :closable="false">
                    Please confirm email address to complete
                    registration
                </Message>
            </div>
        </div>
    </div>
</template>

<style scoped>
    form .field {
        margin-bottom: 2rem;
    }
</style>
