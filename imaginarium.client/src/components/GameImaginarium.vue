<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <div>
          <span class="modal-container-header">{{currentMove[valid.chooseCard]}}</span>
          <span class="modal-container-exit" @click="hideModalWindow">x</span>
        </div>
        <ErrorModal :errorText="error" v-if="error.length" @clearError="() => error=''" />
      </div>
      <div class="modal-wrapper-game">
        <div class="modal-wrapper-game-score">
            <span class="modal-wrapper-game-score-header">Очки:</span>
            <ul>
              <li v-for="(player,key) in store.players" :key="key">{{player?.name}} : {{ player?.score }}</li>
            </ul>
        </div>
        <div class="modal-wrapper-game-code-word" v-if="store.players?.find(p => p?.name === store.currentPlayer?.name)?.isLeader == true">
          <span for="me">Ассоциация:</span>
          <input type="text" id="me" placeholder="Введите ассоциацию" v-model="codeWord">
        </div>
        <div v-for="(player,key) in store.players" :key="key" class="modal-wrapper-game-cards">
          <ul v-if="player && player.name == store.currentPlayer?.name" >
            <li v-for="(card,key2) in player.cards" :key="key2" @click="selectCard(card)"><img :src="`../../imaginImag/${card?.cardName}`"></li>
          </ul>
        </div>
        <!-- <div class="selected-card" v-if="store.currentPlayer?.selectedCard" style="height: 400px; widows: 400px;">
          <img :src="`../../ImaginImag/${store.currentPlayer?.selectedCard?.cardName}`">
        </div> -->
        <div class="modal-wrapper-game-submit">
          <button class="modal-wrapper-game-button" type="button" @click="submit" :disabled="isDisabled">Отправить</button>
          <!--Тут будет компонента ожидания(кружок крутящийся), для ожидания других игроков-->
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { onMounted, onBeforeUnmount, ref, watch} from "vue"
import { usePlayersStore } from "@/stores/playersStore";
import { playersRequest } from "@/http/httpRequests";
import ErrorModal from "../components/UI_elements/ErrorModal.vue"
import _ from "lodash"
import axios from "axios"
import {type Card} from "../types/Card"
import {type ScoreCard} from "../types/ScoreCard" 

const emits = defineEmits(["hideModal", "startVoting"]);

const store = usePlayersStore();

const isSelect = ref<boolean>(false);
const isDisabled = ref<boolean>(false);

const error = ref<String>("")

const codeWord = ref<String>("");
const currentCard = ref<Card>();

let valid = {
  chooseCard: 1,
  chooseCardAdmin: 2,
  wait: 3
}

const currentMove = ref({
  [valid.chooseCard]:"Выберите карту",
  [valid.chooseCardAdmin]: "Выберите карту и ассоциацию",
  [valid.wait]:"Ждите"
});

let checkUsers:any;
let fetchScore:any;
let checkCards:any;

const hideModalWindow = async ():Promise<void> => {
  await playersRequest.userPost('endGame');
  emits("hideModal");
}

//Срабатывает, когда все нажмут на кнопку ГОТОВО
const playersReady = async ():Promise<void> => {
  let cnt = 0;
  store.players?.forEach( async(player) => {
    if(store.players && player?.isReady){
      cnt++;
      if(store.players.length == cnt){
        const response = await axios.post(`http://localhost:5276/api/User/playersReady`);
        store.cards = response.data as Array<ScoreCard>;
      }
    }
  })
}

const submit = async():Promise<void> => {
  if(!isSelect.value){
    await axios.post(`http://localhost:5276/api/User/selectCard?cardId=${currentCard.value?.id}&name=${store.currentPlayer?.name}`);
    playersReady();
    isSelect.value = true;
    isDisabled.value = true
  }
  else{
    error.value = "Вы уже выбрали карточку!"
  }
}

const selectCard = async(card: Card) => {
  currentCard.value = card;
}

const sortByScore = () => {
  // store.players?.sort((a, b) => {
  //   if (a && b && a.score !== undefined && b.score !== undefined) {
  //     return b.score - a.score;
  //   }
  //   return 0;
  // });
  _.sortBy(store.players, 'score', 'desc'); //сортировка по полю score
};

watch(() => store.cards, (newValue) => {
  if(newValue?.length == store.players?.length){
    emits("startVoting")
  }
})

onMounted(async() => {
  store.fetchPlayers();
  sortByScore();
  checkUsers = setInterval(store.fetchPlayers, 100);
  checkCards = setInterval(store.fetchCards, 300);
  fetchScore = setInterval(sortByScore, 100);
});

onBeforeUnmount(() => {
  clearInterval(checkUsers);
  clearInterval(fetchScore);
  clearInterval(checkCards);
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
      flex-flow: column nowrap;
      &-score{
        display: flex;
        flex-flow: column nowrap;
        min-width: 8%;
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
          display: flex;
          flex-flow: row wrap;
          & li{
            margin: 15px;
            &:active{
              border: 4px solid yellow;
            }
          }
        }
      }
      &-code-word{
        margin-top: 20px;
        font-size: x-large;
        & span{
          margin-right: 10px;
        }
      }
      &-submit{
        display: flex;
        flex-flow: row nowrap;
        align-items: center;
        justify-content: center;
      }
      &-button{
        font-size: 24px;
        width: 120px;
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