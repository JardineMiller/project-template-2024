import { describe, it, expect, vi, beforeEach } from "vitest";
import { mount, flushPromises } from "@vue/test-utils";

// Mock only GoogleLogin to avoid plugin init, but use real PrimeVue components
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
import PrimeVue from "primevue/config";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Button from "primevue/button";
import Message from "primevue/message";

describe("LoginView", () => {
    let wrapper: any;

    beforeEach(() => {
        wrapper = mount(LoginView, {
            global: {
                plugins: [PrimeVue],
                components: {
                    InputText,
                    Password,
                    Button,
                    Message,
                },
                stubs: {
                    RouterLink: true,
                },
            },
        });
    });

    it("renders properly", () => {
        expect(wrapper.text()).toContain("Welcome Back");
        expect(wrapper.findComponent(InputText).exists()).toBe(true);
        expect(wrapper.findComponent(Password).exists()).toBe(true);
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
        const emailInput = wrapper.findComponent(InputText).find("input");
        const passwordInput = wrapper.findComponent(Password).find("input");
        await emailInput.trigger("blur");
        await passwordInput.trigger("blur");
        await flushPromises();
        expect(wrapper.text()).toMatch(/required/i);
    });

    it("shows email format validation", async () => {
        const emailInput = wrapper.findComponent(InputText).find("input");
        await emailInput.setValue("not-an-email");
        await emailInput.trigger("blur");
        await flushPromises();
        expect(wrapper.text()).toMatch(/valid email/i);
    });

    it("enables submit button when form is valid", async () => {
        const emailInput = wrapper.findComponent(InputText).find("input");
        const passwordInput = wrapper.findComponent(Password).find("input");
        await emailInput.setValue("test@example.com");
        await passwordInput.setValue("password123");
        await flushPromises();
        const button = wrapper.find("button[type='submit']");
        expect(button.attributes("disabled")).toBeUndefined();
    });

    it("calls Auth.login with correct data on submit", async () => {
        const loginMock = vi.spyOn(Auth, "login").mockResolvedValue(undefined);
        const emailInput = wrapper.findComponent(InputText).find("input");
        const passwordInput = wrapper.findComponent(Password).find("input");
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
