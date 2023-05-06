<template>
    <span class="p-float-label">
        <InputText
            :id="modelValue.propertyName.toLowerCase()"
            :name="modelValue.propertyName.toLowerCase()"
            :model-value="modelValue.value"
            :class="{
                'p-invalid': isInvalid,
            }"
            @input="onInput($event.target.value)"
            @blur="onBlur($event.target.value)"
        />
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
    import "../../extensions/string-extensions";
    import InputText from "primevue/inputtext";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "TextInput",
        components: { InputText },
        mixins: [FormInputMixin],
    });
</script>

<style scoped></style>
