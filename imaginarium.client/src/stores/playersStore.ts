import { playersRequest } from "@/http/httpRequests";
import type { User } from "@/types/User";
import { defineStore } from "pinia";
import { ref } from "vue";

export const usePlayersStore = defineStore("playersStore", () => {
    const players = ref<Array<User>>()
    const currentPlayer = ref<User>()

    const fetchPlayers = async ():Promise<void> => {
        try {
            players.value = await playersRequest.userGet(`getUsers`)
        } catch (error) {
            console.error('Error fetching players:', error);
        }
    };

    return {
        players,
        currentPlayer,
        fetchPlayers
    };
})