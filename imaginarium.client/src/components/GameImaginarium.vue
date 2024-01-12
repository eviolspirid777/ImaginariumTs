<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Имаджинариум</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
        <!-- <ErrorModal :errorText="error" v-if="error.length" @clearError="() => error=''"></ErrorModal> -->
      </div>
      <div class="modal-wrapper-game">
        <div class="modal-wrapper-game-score">
            <span class="modal-wrapper-game-score-header">Очки:</span>
            <ul>
              <li v-for="(player,key) in store.players" :key="key">{{player?.name}} : {{ player?.score }}</li>
            </ul>
        </div>
        <div v-for="(player,key) in store.players" :key="key" class="modal-wrapper-game-cards">
          <ul v-if="player && player.name == store.currentPlayer?.name" >
            <li v-for="(card,key2) in player.cards" :key="key2" @click="selectCard(card)"><img :src="`../../imaginImag/${card?.cardName}`"></li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, onBeforeUnmount, onBeforeMount, ref} from "vue"
import { usePlayersStore } from "@/stores/playersStore";
import { playersRequest } from "@/http/httpRequests";
// import ErrorModal from "@/UI_elements/ErrorModal.vue"
import axios from "axios"
import {type Card} from "../types/Card.ts"

const emits = defineEmits(["hideModal"]);

const store = usePlayersStore();

const isSelect = ref<Boolean>(false);
const error = ref<String>("")

let checkUsers:any;
let fetchScore:any;

const hideModalWindow = async () => {
  await playersRequest.userPost('endGame');
  emits("hideModal");
}

const selectCard = async(card: Card) => {
  if(!isSelect.value){
    console.log(card);
    //отправляем выбранную карточку на сервак
    await axios.post(`http://localhost:5276/api/User/selectCard?cardId=${card?.id}&name=${store.currentPlayer?.name}`);
    isSelect.value = true;
  }
  else{
    error.value = "Вы уже выбрали карточку!"
  }
}

const sortByScore = () => {
  store.players?.sort((a, b) => {
    if (a && b && a.score !== undefined && b.score !== undefined) {
      return b.score - a.score;
    }
    return 0;
  });
};

onBeforeMount(async() => {
  await playersRequest.userGet(`startGame`)
})

onMounted(() => {
  store.fetchPlayers()
  sortByScore();
  checkUsers = setInterval(store.fetchPlayers, 100);
  fetchScore = setInterval(sortByScore, 100);
});

onBeforeUnmount(() => {
  clearInterval(checkUsers);
  clearInterval(fetchScore);
});
</script>

<style lang="scss" scoped>
.modal {
  &-wrapper{
    width: 90%;
    height: 90%;
    display: flex;
    flex-flow: column nowrap;
    background-color: #000000;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    padding-left: 20px;
    padding-right: 20px;
    &-game{
      display: flex;
      flex-flow: row nowrap;
      &-score{
        display: flex;
        flex-flow: column nowrap;
        min-width: 30%;
        max-width: 15%;
        margin-top: 20px;
        padding: 5px;
        border: 1px solid wheat;
        &-header{
          align-self: center;
          color: rgb(218, 189, 136);
          font-size: 20px;
        }
        & ul{
          text-align: left;
          margin-bottom: 5px;
          margin-top: 20px;
        }
      }
      &-cards{
        display: flex;
        margin: 20px;
        width: 95%;
        padding: 15px;
        & ul{
          & li{
            margin: 5px;
          }
        }
      }
    }
  }
  &-container{
    display: flex;
    justify-content: space-between;
    border-radius: 10px;
    background-color: rgba(0, 0, 0, 1);
    padding: 20px;
    padding-bottom: 20px;
    text-align: center;
    color: wheat;
    font-size: 22px;
    &-header{
      padding-left: 43%;
      cursor: default;
      user-select: none;
    }
    &-exit{
      width: 20px;
      background-color: wheat;
      color: black;
      user-select: none;
      &:hover{
        background-color: rgb(218, 195, 151);
        cursor: pointer;
        box-shadow: 1px 1px 10px #b8c952;
      }
    }
  }
}
</style>