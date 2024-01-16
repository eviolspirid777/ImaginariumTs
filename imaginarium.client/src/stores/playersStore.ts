import { playersRequest } from "@/http/httpRequests";
import type { ScoreCard } from "@/types/ScoreCard";
import type { User } from "@/types/User";
import { defineStore } from "pinia";
import { ref } from "vue";

export const usePlayersStore = defineStore("playersStore", () => {
    const players = ref<Array<User>>();
    const currentPlayer = ref<User>();
    const cards = ref<Array<ScoreCard>>();
    const codeWord = ref<String>("");

    const fetchPlayers = async ():Promise<void> => {
        try {
            players.value = await playersRequest.userGet(`getUsers`)
        } catch (error) {
            console.error('Error fetching players:', error);
        }
    };

    const fetchCards = async ():Promise<void> => {
        try{
            await playersRequest.userGet(`getCurrentCards`)
        } catch(error){
            console.error('Error fetching cards:', cards)
        }
    };

    const fetchWord = async ():Promise<void> => {
        try{
            await playersRequest.userGet(`getWord`)
        } catch(error){
            console.error('Error fetching word:', codeWord)
        }
    }

    return {
        players,
        currentPlayer,
        cards,
        codeWord,
        fetchPlayers,
        fetchCards,
        fetchWord
    };
})