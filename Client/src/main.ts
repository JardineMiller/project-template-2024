import { addJwtInterceptor } from "@/features/auth/interceptors/httpInterceptor";
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Calendar from "primevue/calendar";
import Dropdown from "primevue/dropdown";
import Checkbox from "primevue/checkbox";
import router from "./services/router";
import Divider from "primevue/divider";
import Tooltip from "primevue/tooltip";
import PrimeVue from "primevue/config";
import Dialog from "primevue/dialog";
import Button from "primevue/button";
import { createApp } from "vue";
import App from "./App.vue";
import "./assets/main.css";

function addVue() {
    const app = createApp(App);

    app.use(router);
    app.use(PrimeVue);

    app.component("Dialog", Dialog);
    app.component("InputText", InputText);
    app.component("Divider", Divider);
    app.component("Password", Password);
    app.component("Calendar", Calendar);
    app.component("Dropdown", Dropdown);
    app.component("Checkbox", Checkbox);
    app.component("Button", Button);

    app.directive("tooltip", Tooltip);

    app.mount("#app");
}

addVue();
addJwtInterceptor();
