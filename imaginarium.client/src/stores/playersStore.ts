import type { User } from "@/types/User";
import { defineStore } from "pinia";
import { ref } from "vue";

export const usePlayersStore = defineStore("playersStore", () => {
    const players = ref<Array<User>>()
    const currentPlayer = ref<User>()

    return {
        players,
        currentPlayer
    };
})