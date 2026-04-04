import { mount } from "@vue/test-utils";
import { ref } from "vue";
import { describe, it, expect, vi, beforeEach } from "vitest";

// Create reactive refs for the Auth mock so tests can inspect / modify them
const mockAuthUser = ref<any>(undefined);
const mockAuthToken = ref<string | undefined>(undefined);

vi.mock("@/modules/auth/services/Auth", () => {
    return {
        default: {
            user: mockAuthUser,
            token: mockAuthToken,
        },
    };
});

// Create spying functions for the User service
const mockGetUserDetails = vi.fn().mockResolvedValue({
    email: "test@example.com",
    bio: "Initial bio",
    avatarUrl: "http://example.com/path/avatar.png",
    displayName: "Test User",
});

const mockUpdateUser = vi.fn().mockResolvedValue({
    email: "updated@example.com",
    bio: "updated bio",
    avatarUrl: undefined,
    displayName: "Updated User",
});

const mockDeleteAvatar = vi.fn().mockResolvedValue({});

vi.mock("@/modules/user/services/User", () => {
    return {
        default: {
            getUserDetails: mockGetUserDetails,
            updateUser: mockUpdateUser,
            deleteAvatar: mockDeleteAvatar,
        },
        URLs: { UPLOAD_IMAGE: "/upload" },
    };
});

// Import the component after mocks are set up
import Profile from "./Profile.vue";

const flush = () => new Promise((r) => setTimeout(r, 0));

beforeEach(() => {
    // Reset/make predictable values before each test
    mockAuthUser.value = {
        displayName: "Test User",
        email: "test@example.com",
        avatarUrl: "http://example.com/path/avatar.png",
    };
    mockAuthToken.value = "token-123";

    mockGetUserDetails.mockClear();
    mockUpdateUser.mockClear();
    mockDeleteAvatar.mockClear();
});

describe("Profile.vue", () => {
    it("fetches user details on mount and clears loading", async () => {
        const toastAdd = vi.fn();

        const wrapper = mount(Profile, {
            global: {
                mocks: { $toast: { add: toastAdd } },
                stubs: [
                    "InputText",
                    "Textarea",
                    "FileUpload",
                    "Button",
                    "Avatar",
                    "Toast",
                ],
            },
        });

        await flush();

        expect(mockGetUserDetails).toHaveBeenCalled();
        expect((wrapper.vm as any).loading).toBe(false);
    });

    it("adds Authorization header in beforeSend", async () => {
        const wrapper = mount(Profile, {
            global: {
                mocks: { $toast: { add: vi.fn() } },
                stubs: [
                    "InputText",
                    "Textarea",
                    "FileUpload",
                    "Button",
                    "Avatar",
                    "Toast",
                ],
            },
        });

        await flush();

        mockAuthToken.value = "abc-token";

        const setRequestHeader = vi.fn();
        (wrapper.vm as any).beforeSend({ xhr: { setRequestHeader } });

        expect(setRequestHeader).toHaveBeenCalledWith(
            "Authorization",
            "Bearer abc-token"
        );
    });

    it("onUpload updates avatarUrl, Auth and shows toast", async () => {
        const toastAdd = vi.fn();
        const wrapper = mount(Profile, {
            global: {
                mocks: { $toast: { add: toastAdd } },
                stubs: [
                    "InputText",
                    "Textarea",
                    "FileUpload",
                    "Button",
                    "Avatar",
                    "Toast",
                ],
            },
        });

        await flush();

        const ev = { xhr: { response: JSON.stringify({ imageUrl: "http://cdn/new.png" }) } };
        (wrapper.vm as any).onUpload(ev);

        expect((wrapper.vm as any).avatarUrl.value).toBe("http://cdn/new.png");
        expect(mockAuthUser.value.avatarUrl).toBe("http://cdn/new.png");
        expect(toastAdd).toHaveBeenCalledWith(
            expect.objectContaining({ summary: "Success", detail: "File Uploaded" })
        );
    });

    it("clearAvatar calls deleteAvatar and clears avatarUrl", async () => {
        const toastAdd = vi.fn();
        const wrapper = mount(Profile, {
            global: {
                mocks: { $toast: { add: toastAdd } },
                stubs: [
                    "InputText",
                    "Textarea",
                    "FileUpload",
                    "Button",
                    "Avatar",
                    "Toast",
                ],
            },
        });

        await flush();

        await (wrapper.vm as any).clearAvatar();

        expect(mockDeleteAvatar).toHaveBeenCalled();
        expect((wrapper.vm as any).avatarUrl.value).toBeUndefined();
        expect(mockAuthUser.value.avatarUrl).toBeUndefined();
        expect(toastAdd).toHaveBeenCalledWith(
            expect.objectContaining({ summary: "Success", detail: "Avatar removed" })
        );
    });

    it("handleSubmit updates user and shows saved toast", async () => {
        const toastAdd = vi.fn();
        const wrapper = mount(Profile, {
            global: {
                mocks: { $toast: { add: toastAdd } },
                stubs: [
                    "InputText",
                    "Textarea",
                    "FileUpload",
                    "Button",
                    "Avatar",
                    "Toast",
                ],
            },
        });

        await flush();

        await (wrapper.vm as any).handleSubmit();

        expect(mockUpdateUser).toHaveBeenCalled();
        expect(mockAuthUser.value.displayName).toBe("Updated User");
        expect(toastAdd).toHaveBeenCalledWith(
            expect.objectContaining({ summary: "Success", detail: "Saved" })
        );
    });
});
