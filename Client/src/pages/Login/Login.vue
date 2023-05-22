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
    import MultiSelect from "primevue/multiselect";
    import { defineComponent } from "vue";
    import "../../validation/validation";

    export default defineComponent({
        components: { TextInput, PasswordInput, MultiSelect },
        data: () => {
            return {
                seletableCities: [
                    { name: "New York", code: "NY" },
                    { name: "Rome", code: "RM" },
                    { name: "London", code: "LDN" },
                    { name: "Istanbul", code: "IST" },
                    { name: "Paris", code: "PRS" },
                ],
                state: new StateTracker<LoginModel>(
                    new LoginModel([
                        // SIMPLE EXAMPLE

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
                        ),

                        // CUSTOM EXAMPLE

                        // new ModelProperty<string>(
                        //     "email",
                        //     undefined,
                        //     [
                        //         Validators.required(),
                        //         Validators.email(
                        //             "Emails suck. But you gotta include one"
                        //         ),
                        //         Validators.minLength(3),
                        //         Validators.maxLength(10),
                        //         Validators.custom(
                        //             (val) => val !== "Jardine",
                        //             "We don't take kindly to Jardines round here."
                        //         ),
                        //     ]
                        // ),

                        // new ModelProperty<string>(
                        //     "password",
                        //     undefined,
                        //     [
                        //         Validators.required(),
                        //         Validators.pattern(
                        //             ValidationAuth.Password.Pattern
                        //         ),
                        //         Validators.custom((val) => {
                        //             return ![
                        //                 "password",
                        //                 "Password",
                        //                 "pass1234",
                        //             ].includes(val);
                        //         }, "Pick a decent password. Seriously."),
                        //     ]
                        // ),

                        // COMPLEX OBJECT EXAMPLE

                        // new ModelProperty<
                        //     Array<{ name: string; code: string }>
                        // >(
                        //     "cities",
                        //     [{ name: "Rome", code: "RM" }],
                        //     [
                        //         Validators.required(),
                        //         Validators.minLength(
                        //             2,
                        //             "This field must contain at least 2 items"
                        //         ),
                        //         Validators.maxLength(
                        //             3,
                        //             "This field must contain no more than 3 items"
                        //         ),
                        //         Validators.custom(
                        //             (
                        //                 val: Array<{
                        //                     name: string;
                        //                     code: string;
                        //                 }>
                        //             ) =>
                        //                 !val.find(
                        //                     (x) =>
                        //                         x.name === "New York"
                        //                 ),
                        //             "Boo! New York!"
                        //         ),
                        //     ]
                        // ),
                    ]),
                    { trackChanges: false }
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
            cities(): ModelProperty<
                Array<{ name: string; code: string }>
            > {
                return this.state.model.cities;
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
                Auth.login(this.state.model.toRequest())
                    .then()
                    .catch();
            },
        },
    });
</script>

<template>
    <div
        class="mt-3 mr-3 flex gap-1 absolute top-0 right-0"
        v-if="state.trackChanges">
        <Button
            icon="pi pi-undo"
            severity="secondary"
            outlined
            aria-label="Undo"
            v-tooltip.top="'Undo'"
            @click="state.undo()"
            :disabled="!state.canUndo" />

        <Button
            icon="pi pi-undo"
            severity="secondary"
            outlined
            aria-label="Redo"
            v-tooltip.top="'Redo'"
            :style="'transform: scale(-1, 1)'"
            @click="state.redo()"
            :disabled="!state.canRedo" />
        <Button
            type="button"
            label="Clear"
            outlined
            @click="state.clear()"
            :disabled="!state.canUndo && !state.canRedo" />
    </div>

    <div class="flex justify-content-center align-content-center">
        <div class="card">
            <h5 class="text-center">Demo Form</h5>
            <form
                @submit.prevent="handleSubmit()"
                class="p-fluid">
                <!-- Email -->
                <div class="field">
                    <TextInput
                        v-model="email"
                        @on-change="onChange"
                        @on-blur="onBlur" />
                </div>

                <!-- Password -->
                <div class="field">
                    <PasswordInput
                        v-model="password"
                        @on-change="onChange"
                        @on-blur="onBlur" />
                </div>

                <!-- Complex objects -->
                <!--                <div class="field">-->
                <!--                    <span class="p-float-label">-->
                <!--                        <MultiSelect-->
                <!--                            v-model="cities.value"-->
                <!--                            :options="seletableCities"-->
                <!--                            :class="{-->
                <!--                                'p-invalid':-->
                <!--                                    cities.touched && !cities.isValid,-->
                <!--                            }"-->
                <!--                            @change="-->
                <!--                                cities.touch();-->
                <!--                                state.setProperty(-->
                <!--                                    'cities',-->
                <!--                                    $event.value-->
                <!--                                );-->
                <!--                            "-->
                <!--                            @blur="cities.touch()"-->
                <!--                            optionLabel="name"-->
                <!--                            placeholder="Select Cities"-->
                <!--                            :maxSelectedLabels="3" />-->

                <!--                        <label-->
                <!--                            :for="cities.propertyName.toLowerCase()">-->
                <!--                            {{ cities.propertyName.toTitleCase() }}-->
                <!--                            <span-->
                <!--                                v-if="cities.isRequired"-->
                <!--                                class="p-error">-->
                <!--                                *-->
                <!--                            </span>-->
                <!--                        </label>-->
                <!--                    </span>-->

                <!--                    <div v-if="cities.touched && !cities.isValid">-->
                <!--                        <small-->
                <!--                            v-for="error in cities.errors"-->
                <!--                            :key="error"-->
                <!--                            class="p-error">-->
                <!--                            {{ error }} <br />-->
                <!--                        </small>-->
                <!--                    </div>-->
                <!--                </div>-->

                <!-- Submit -->
                <Button
                    type="submit"
                    label="Submit"
                    :disabled="!state.model.isValid" />
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
