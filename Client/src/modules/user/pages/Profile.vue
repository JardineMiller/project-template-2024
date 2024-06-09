<template>
    <div class="p-5 flex flex-column align-items-center flex-auto">
        <div class="col-10">
            <div class="text-900 font-medium text-xl mb-3">Profile</div>
            <div class="surface-card p-4 shadow-2 border-round">
                <div class="grid formgrid p-fluid">
                    <!-- Display name -->
                    <div class="field mb-4 col-6">
                        <label
                            class="font-medium"
                            for="displayName"
                        >
                            Display Name
                        </label>
                        <InputText
                            :id="'displayName'"
                            :autocomplete="'displayName'"
                            :class="{
                                'p-invalid': false,
                            }"
                            :model-value="user?.displayName"
                            :name="'displayName'"
                        />
                    </div>

                    <!-- Email -->
                    <div class="field mb-4 col-6">
                        <label
                            class="font-medium"
                            for="nickname"
                        >
                            Email
                        </label>
                        <InputText
                            :id="'email'"
                            :autocomplete="'email'"
                            :class="{
                                'p-invalid': false,
                            }"
                            :model-value="user?.email"
                            :name="'email'"
                        />
                    </div>

                    <!-- HR -->
                    <div
                        class="surface-100 mb-3 col-12"
                        style="height: 2px"
                    ></div>

                    <!-- Bio -->
                    <div class="field mb-4 col-6">
                        <label
                            class="font-medium"
                            for="nickname"
                        >
                            Bio
                        </label>
                        <Textarea
                            v-model="undefined"
                            auto-resize
                            rows="5"
                        ></Textarea>
                    </div>

                    <!-- Avatar -->
                    <div class="field mb-4 col-6">
                        <label
                            class="font-medium"
                            for="nickname"
                        >
                            Avatar
                        </label>

                        <div
                            class="surface-card flex h-full pb-4 justify-content-center"
                        >
                            <Toast />
                            <div
                                class="flex justify-content-center align-items-center gap-2"
                            >
                                <Avatar
                                    :label="user?.displayName[0]"
                                    class="mr-2"
                                    shape="circle"
                                    size="xlarge"
                                />

                                <FileUpload
                                    :maxFileSize="1000000"
                                    accept="image/*"
                                    chooseLabel="Browse"
                                    class="p-button-outlined"
                                    mode="basic"
                                    name="demo[]"
                                    url="/api/upload"
                                    @upload="onUpload"
                                />
                            </div>
                        </div>
                    </div>

                    <div
                        class="surface-100 mb-3 col-12"
                        style="height: 2px"
                    ></div>

                    <div class="flex w-full justify-content-end">
                        <Button
                            :loading="true"
                            class="w-auto"
                            label="Save Changes"
                        ></Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped></style>

<script lang="ts">
    import Auth from "@/modules/auth/services/Auth";
    import FileUpload from "primevue/fileupload";
    import InputText from "primevue/inputtext";
    import Textarea from "primevue/textarea";
    import { defineComponent } from "vue";
    import Button from "primevue/button";
    import Avatar from "primevue/avatar";
    import Toast from "primevue/toast";

    export default defineComponent({
        head: {
            title: "Profile",
        },
        components: {
            InputText,
            Textarea,
            FileUpload,
            Button,
            Toast,
            Avatar,
        },
        data() {
            return {
                files: [],
                totalSize: 0,
                totalSizePercent: 0,
            };
        },
        computed: {
            user() {
                return Auth.user.value;
            },
        },
        methods: {
            onUpload() {
                this.$toast.add({
                    severity: "info",
                    summary: "Success",
                    detail: "File Uploaded",
                    life: 3000,
                });
            },
        },
    });
</script>
