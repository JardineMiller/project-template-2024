<template>
    <span class="p-float-label">
        <Password
            id="password"
            v-model="modelValue.value"
            :class="{
                'p-invalid': isInvalid,
            }"
            :strong-regex="passwordRegex"
            @input="onInput($event.target.value)"
            @blur="onBlur($event.target.value)"
            toggleMask
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
                    <li>At least one lowercase</li>
                    <li>At least one uppercase</li>
                    <li>At least one numeric</li>
                    <li>At least one special character</li>
                    <li>Minimum 6 characters</li>
                </ul>
            </template>
        </Password>

        <label :for="modelValue.propertyName.toLowerCase()">
            {{ modelValue.propertyName.toTitleCase() }}
            <span
                v-if="isRequired"
                class="p-error"
            >
                *
            </span>
        </label>
    </span>
    <div v-if="isInvalid">
        <small
            v-for="error in modelValue.errors"
            :key="error"
            class="p-error"
        >
            {{ error }} <br />
        </small>
    </div>
</template>

<script lang="ts">
    import FormInputMixin from "@/mixins/forms/FormInputMixin";
    import Validation from "@/validation/validation";
    import "../../extensions/string-extensions";
    import Password from "primevue/password";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "PasswordInput",
        components: { Password },
        mixins: [FormInputMixin],
        data: () => {
            return {
                passwordRegex: Validation.Auth.Password.Pattern,
            };
        },
    });
</script>

<style scoped></style>
