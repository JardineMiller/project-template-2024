<script lang="ts">
    import FileUpload, {
        type FileUploadBeforeUploadEvent,
        type FileUploadUploadEvent,
    } from "primevue/fileupload";
    import type GetUserDetailsResponse from "@/modules/user/models/GetUserDetailsResponse";
    import type UploadImageResponse from "@/modules/user/models/UploadImageResponse";
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import UpdateUserModel from "@/modules/user/models/UpdateUserModel";
    import { Validators } from "@/modules/forms/validation/Validators";
    import ModelProperty from "@/modules/forms/models/ModelProperty";
    import "@/utils/extensions/string/string-extensions";
    import Validation from "@/validation/validation";
    import Auth from "@/modules/auth/services/Auth";
    import User, { URLs } from "@/modules/user/services/User";
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
                loading: true,
                saving: false,
                files: [],
                totalSize: 0,
                totalSizePercent: 0,
                state: {} as StateTracker<UpdateUserModel>,
            };
        },
        created() {
            this.fetchData();
        },
        computed: {
            user() {
                return Auth.user.value;
            },
            uploadUrl(): string {
                return URLs.UPLOAD_IMAGE;
            },
            email(): ModelProperty<string> {
                return this.state.model.email;
            },
            bio(): ModelProperty<string> {
                return this.state.model.bio;
            },
            avatarUrl(): ModelProperty<string> {
                return this.state.model.avatarUrl;
            },
            displayName(): ModelProperty<string> {
                return this.state.model.displayName;
            },
        },
        methods: {
            async fetchData() {
                const userData = await User.getUserDetails();
                this.initialise(userData);
                this.loading = false;
            },
            initialise(userData: GetUserDetailsResponse) {
                this.state = new StateTracker<UpdateUserModel>(
                    new UpdateUserModel([
                        new ModelProperty<string>("email", userData.email, [
                            Validators.required(),
                            Validators.email(),
                        ]),
                        new ModelProperty<string>("bio", userData.bio, [
                            Validators.maxLength(Validation.User.Bio.MaxLength),
                        ]),
                        new ModelProperty<string>(
                            "avatarUrl",
                            userData.avatarUrl,
                            []
                        ),
                        new ModelProperty<string>(
                            "displayName",
                            userData.displayName,
                            [
                                Validators.required(),
                                Validators.minLength(
                                    Validation.User.DisplayName.MinLength
                                ),
                                Validators.maxLength(
                                    Validation.User.DisplayName.MaxLength
                                ),
                            ]
                        ),
                    ])
                );
            },
            beforeSend(event: FileUploadBeforeUploadEvent) {
                event.xhr.setRequestHeader(
                    "Authorization",
                    `Bearer ${Auth.token.value}`
                );
            },
            onUpload(event: FileUploadUploadEvent) {
                const response = JSON.parse(
                    event.xhr.response
                ) as UploadImageResponse;
                this.avatarUrl.value = response.imageUrl;

                if (Auth.user.value) {
                    Auth.user.value.avatarUrl = response.imageUrl;
                }

                this.$toast.add({
                    severity: "info",
                    summary: "Success",
                    detail: "File Uploaded",
                    life: 1500,
                });
            },
            async clearAvatar(): Promise<void> {
                if (!this.avatarUrl.value) {
                    return;
                }

                this.saving = true;

                try {
                    let fileName = this.avatarUrl.value as string;

                    try {
                        const url = new URL(fileName);
                        fileName = url.pathname.split("/").pop() || fileName;
                    } catch {
                        fileName = (fileName as string).split("/").pop() || fileName;
                    }

                    // strip query string if present
                    fileName = (fileName as string).split("?")[0];

                    const result = await User.deleteAvatar(fileName);

                    // update UI and auth
                    this.avatarUrl.value = undefined;

                    if (Auth.user.value) {
                        Auth.user.value.avatarUrl = undefined;
                    }

                    this.$toast.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Avatar removed",
                        life: 1500,
                    });
                } catch (err) {
                    this.$toast.add({
                        severity: "error",
                        summary: "Error",
                        detail: "Could not remove avatar",
                        life: 1500,
                    });
                } finally {
                    this.saving = false;
                }
            },
            async handleSubmit(): Promise<void> {
                this.saving = true;

                const payload = this.state.model.toRequest();

                try {
                    const result = await User.updateUser(payload);

                    if (Auth.user.value) {
                        Auth.user.value.displayName = result.displayName;
                        Auth.user.value.email = result.email;
                        Auth.user.value.avatarUrl = result.avatarUrl;
                    }

                    // Update local model values from server response
                    this.state.setProperty<string>(
                        "bio",
                        result.bio ?? ""
                    );

                    this.$toast.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Saved",
                        life: 1500,
                    });
                } catch (err) {
                    this.$toast.add({
                        severity: "error",
                        summary: "Error",
                        detail: "Could not save changes",
                        life: 1500,
                    });
                } finally {
                    this.saving = false;
                }
            },
        },
    });
</script>

<template>
    <div class="p-5 flex flex-column align-items-center flex-auto">
        <div class="col-10">
            <div class="text-900 font-medium text-xl mb-3">Profile</div>
            <div class="surface-card p-4 shadow-2 border-round">
                <form
                    v-if="!loading"
                    class="grid formgrid p-fluid"
                    @submit.prevent="handleSubmit()"
                >
                    <!-- DISPLAY NAME -->
                    <div class="field mb-4 col-6">
                        <label :for="displayName.propertyName.toLowerCase()">
                            {{ displayName.propertyName.toTitleCase() }}
                            <span
                                v-if="displayName.isRequired"
                                class="p-error"
                            >
                                *
                            </span>
                        </label>
                        <InputText
                            :id="displayName.propertyName.toLowerCase()"
                            :autocomplete="
                                displayName.propertyName.toLowerCase()
                            "
                            :class="{
                                'p-invalid':
                                    displayName.touched && !displayName.isValid,
                            }"
                            :model-value="displayName.value"
                            :name="displayName.propertyName.toLowerCase()"
                            @input="
                                this.state.setProperty<string>(
                                    displayName.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="displayName.touch()"
                        />
                        <div v-if="displayName.touched && !displayName.isValid">
                            <small
                                v-for="error in displayName.errors"
                                :key="error"
                                class="p-error"
                            >
                                {{ error }} <br />
                            </small>
                        </div>
                    </div>

                    <!-- EMAIL -->
                    <div class="field mb-4 col-6">
                        <label :for="email.propertyName.toLowerCase()">
                            {{ email.propertyName.toTitleCase() }}
                            <span
                                v-if="email.isRequired"
                                class="p-error"
                            >
                                *
                            </span>
                        </label>
                        <InputText
                            :id="email.propertyName.toLowerCase()"
                            :autocomplete="email.propertyName.toLowerCase()"
                            :class="{
                                'p-invalid': email.touched && !email.isValid,
                            }"
                            :model-value="email.value"
                            :name="email.propertyName.toLowerCase()"
                            @input="
                                this.state.setProperty<string>(
                                    email.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="email.touch()"
                        />
                        <div v-if="email.touched && !email.isValid">
                            <small
                                v-for="error in email.errors"
                                :key="error"
                                class="p-error"
                            >
                                {{ error }} <br />
                            </small>
                        </div>
                    </div>

                    <!-- HR -->
                    <div
                        class="surface-100 mb-3 col-12"
                        style="height: 2px"
                    ></div>

                    <!-- BIO -->
                    <div class="field mb-4 col-6">
                        <label :for="bio.propertyName.toLowerCase()">
                            {{ bio.propertyName.toTitleCase() }}
                            <span
                                v-if="bio.isRequired"
                                class="p-error"
                            >
                                *
                            </span>
                        </label>
                        <Textarea
                            v-model="bio.value"
                            auto-resize
                            rows="5"
                            @input="
                                this.state.setProperty<string>(
                                    bio.propertyName,
                                    $event.target.value
                                )
                            "
                            @blur="bio.touch()"
                        ></Textarea>
                    </div>

                    <!-- AVATAR -->
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
                                    :image="
                                        avatarUrl.value
                                            ? avatarUrl.value
                                            : undefined
                                    "
                                    :label="
                                        avatarUrl ? '' : user?.displayName[0]
                                    "
                                    class="mr-2 flex-none"
                                    shape="circle"
                                    size="xlarge"
                                />

                                <FileUpload
                                    :maxFileSize="1000000"
                                    :with-credentials="true"
                                    auto
                                    :url="uploadUrl"
                                    accept="image/*"
                                    chooseLabel="Upload"
                                    class="p-button-outlined"
                                    mode="basic"
                                    name="file"
                                    @before-send="beforeSend"
                                    @upload="onUpload"
                                />
                                <Button
                                    icon="pi pi-trash"
                                    title="Clear avatar"
                                    class="p-button-danger p-button-icon-only"
                                    :disabled="!avatarUrl.value"
                                    @click="clearAvatar"
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
                            :loading="saving"
                            class="w-auto"
                            label="Save Changes"
                            type="submit"
                            :disabled="!state.model.isValid"
                        ></Button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<style scoped></style>
