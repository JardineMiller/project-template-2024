<script lang="ts">
    import StateTracker from "@/modules/stateTracker/models/StateTracker";
    import { defineComponent } from "Vue";

    export default defineComponent({
        name: "StateNavigator",
        props: {
            state: {
                type: StateTracker<any>,
                required: true,
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
</template>

<style scoped></style>
