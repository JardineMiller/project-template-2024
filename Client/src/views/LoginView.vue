<script lang="ts">
    import Validation from "@/validation/validation";
    export default {
        data() {
            return {
                username: "",
                password: "",
                submitted: false,
                passwordRegex: Validation.Auth.Password.Pattern,
                impossibleRegex:
                    "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])(?=.{8,})[A-Za-z\\d@$!%*?&]+$",
            };
        },
        methods: {
            handleSubmit() {
                this.submitted = true;
            },
        },
    };
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
                    <div class="field">
                        <span class="p-float-label p-input-icon-left">
                            <InputText
                                id="name"
                                v-model="username"
                                :class="{
                                    'p-invalid': submitted,
                                }"
                            />
                            <i class="pi pi-id-card"></i>
                            <label for="name">Username *</label>
                        </span>
                        <small
                            v-if="submitted"
                            class="p-error"
                            >This is an error</small
                        >
                    </div>
                    <div class="field">
                        <span class="p-float-label p-input-icon-left">
                            <Password
                                id="password"
                                v-model="password"
                                :class="{
                                    'p-invalid': submitted,
                                }"
                                toggleMask
                                :medium-regex="impossibleRegex"
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
                            <i class="pi pi-lock"></i>
                            <label
                                for="password"
                                :class="{
                                    'p-error': submitted,
                                }"
                                >Password *</label
                            >
                        </span>
                        <small
                            v-if="submitted"
                            class="p-error"
                            >This is an error</small
                        >
                    </div>
                    <Button
                        type="submit"
                        label="Submit"
                        class="mt-2"
                    />
                </form>
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
