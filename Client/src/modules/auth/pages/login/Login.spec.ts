import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";

// Mock the google login component so plugin initialization isn't required
vi.mock("vue3-google-login", () => {
    return {
        GoogleLogin: {
            name: "GoogleLogin",
            props: ["callback"],
            template:
                "<button @click=\"callback && callback({ credential: 'test-token' })\">Google</button>",
        },
    };
});

import LoginView from "./Login.vue";
import Auth from "@/modules/auth/services/Auth";

describe("LoginView", () => {
    let wrapper: any;

    beforeEach(() => {
        wrapper = mount(LoginView, {
            global: {
                stubs: {
                    RouterLink: true,
                },
                components: {
                    // Stub PrimeVue components used by the view
                    InputText: { name: "InputText", template: "<input />" },
                    Password: { name: "Password", template: "<input />" },
                    Button: {
                        name: "Button",
                        props: ["loading", "disabled", "label", "type"],
                        template:
                            '<button :disabled="disabled" :type="type"><slot /></button>',
                    },
                    Message: {
                        name: "Message",
                        template: "<div><slot /></div>",
                    },
                },
            },
        });
    });

    it("renders properly", () => {
        expect(wrapper.text()).toContain("Welcome Back");
        expect(wrapper.find("input[name='email']").exists()).toBe(true);
        expect(wrapper.find("input[name='password']").exists()).toBe(true);
        expect(wrapper.findComponent({ name: "GoogleLogin" }).exists()).toBe(
            true
        );
        expect(wrapper.findComponent({ name: "RouterLink" }).exists()).toBe(
            true
        );
    });

    it("disables submit button when form is invalid", async () => {
        const button = wrapper.find("button[type='submit']");
        expect(button.attributes("disabled")).toBeDefined();
    });

    it("shows required validation for email and password", async () => {
        const emailInput = wrapper.find("input[name='email']");
        const passwordInput = wrapper.find("input[name='password']");
        await emailInput.trigger("blur");
        await passwordInput.trigger("blur");
        await flushPromises();
        expect(wrapper.text()).toMatch(/required/i);
    });

    it("shows email format validation", async () => {
        const emailInput = wrapper.find("input[name='email']");
        await emailInput.setValue("not-an-email");
        await emailInput.trigger("blur");
        await flushPromises();
        expect(wrapper.text()).toMatch(/valid email/i);
    });

    it("enables submit button when form is valid", async () => {
        const emailInput = wrapper.find("input[name='email']");
        const passwordInput = wrapper.find("input[name='password']");
        await emailInput.setValue("test@example.com");
        await passwordInput.setValue("password123");
        await flushPromises();
        const button = wrapper.find("button[type='submit']");
        expect(button.attributes("disabled")).toBeUndefined();
    });

    it("calls Auth.login with correct data on submit", async () => {
        const loginMock = vi.spyOn(Auth, "login").mockResolvedValue(undefined);
        const emailInput = wrapper.find("input[name='email']");
        const passwordInput = wrapper.find("input[name='password']");
        await emailInput.setValue("test@example.com");
        await passwordInput.setValue("password123");
        await flushPromises();
        await wrapper.find("form").trigger("submit.prevent");
        expect(loginMock).toHaveBeenCalledWith({
            email: "test@example.com",
            password: "password123",
        });
        loginMock.mockRestore();
    });

    it("shows error message for invalid credentials", async () => {
        wrapper.vm.state.model.responseErrors.invalidCredentials = true;
        await wrapper.vm.$forceUpdate();
        await flushPromises();
        expect(wrapper.text()).toContain("Login failed: Invalid credentials");
    });

    it("shows error message for email not confirmed", async () => {
        wrapper.vm.state.model.responseErrors.emailNotConfirmed = true;
        await wrapper.vm.$forceUpdate();
        await flushPromises();
        expect(wrapper.text()).toContain("Login failed: Email not confirmed");
    });

    it("calls Auth.googleLogin when GoogleLogin callback is triggered", async () => {
        const googleLoginMock = vi
            .spyOn(Auth, "googleLogin")
            .mockResolvedValue(undefined);
        const googleLogin = wrapper.findComponent({ name: "GoogleLogin" });
        expect(googleLogin.exists()).toBe(true);
        // Call the provided callback prop directly to simulate the google login
        const cb = googleLogin.props("callback") as Function;
        await cb({ credential: "test-token" });
        expect(googleLoginMock).toHaveBeenCalledWith("test-token");
        googleLoginMock.mockRestore();
    });

    it("shows loading state for Google login", async () => {
        wrapper.vm.googleLoading = true;
        await wrapper.vm.$forceUpdate();
        await flushPromises();
        expect(wrapper.vm.googleLoading).toBe(true);
    });
});
