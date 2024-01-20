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
        <div v-if="store.players?.find(p => p?.name === store.currentPlayer?.name)?.isLeader == true" class="modal-wrapper-game-code-word">
          <span for="me">Ассоциация: {{ codeWord }}</span>
          <input type="text" id="me" placeholder="Введите ассоциацию" v-model="tempWord" />
        </div>
        <div v-else class="modal-wrapper-game-code-word">
          <span for="me">Ассоциация: {{ codeWord }}</span>
        </div>
        <div v-for="(player,key) in store.players" :key="key">
          <div class="modal-wrapper-game-cards">
            <ul v-if="player && player.name == store.currentPlayer?.name" >
              <li v-for="(card,key2) in player.cards" :key="key2" @click="selectCard(card)"><img :src="`../../imaginImag/${card?.cardName}`"></li>
            </ul>
          </div>
        </div>
        <div class="modal-wrapper-game-submit">
          <button v-if="codeWord != '' || store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader" class="modal-wrapper-game-button" type="button" @click="submit" :disabled="isDisabled">Отправить</button>
          <div v-else class="modal-wrapper-game-wait">
              <svg class="spinner" viewBox="0 0 50 50">
                <circle class="path" cx="25" cy="25" r="20" fill="none" stroke-width="5"></circle>
              </svg>
              <span>Ждем</span>
          </div>
        </div>
      </div>
    </div>
  </div>
  <SelectCard v-if="isSelectCard" @hideModal="isSelectCard = false" @nextAdmin="submitCards"/>
</template>

<script lang="ts" setup>
import { onMounted, onBeforeUnmount, ref, watch, computed} from "vue"
import { usePlayersStore } from "@/stores/playersStore";
import { playersRequest } from "@/http/httpRequests";
import ErrorModal from "../components/UI_elements/ErrorModal.vue"
import _ from "lodash"
import axios from "axios"
import {type Card} from "../types/Card"
import SelectCard from "../components/SelectingCardWindow.vue"

const emits = defineEmits(["hideModal", "startVoting"]);

const store = usePlayersStore();

const isSelect = ref<boolean>(false);
const isDisabled = ref<boolean>(true);
const isSelectCard = ref<boolean>(false);

const error = ref<String>("")

const currentCard = ref<Card>();
const tempWord = ref<String>("");
const codeWord = computed(() => store.codeWord)

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
let checkWord:any;

const hideModalWindow = async ():Promise<void> => {
  await playersRequest.userPost('endGame');
  emits("hideModal");
}

//Срабатывает, когда все нажмут на кнопку ГОТОВО
const playersReady = async ():Promise<void> => {
  let cnt = 0;
  store.players?.forEach(async(player) => {
    if(store.players && player?.isReady){
      cnt++;
      if(store.players.length == cnt){
        // const response = await axios.post(`http://localhost:5276/api/User/playersReady`);
        // store.cards = response.data as Array<ScoreCard>;  //добавляет в стор карточки, которые сейчас в игровой сессии
        isSelectCard.value = true;
      }
    }
  })
}

const submitCards = async ():Promise<void> => {
  await axios.post(`http://localhost:5276/api/User/playersReady`);
}

const submit = async():Promise<void> => {
  if(!isSelect.value){
    if(store.players?.find(p => p?.name === store.currentPlayer?.name)?.isLeader){
      store.codeWord = tempWord.value;
      await axios.post(`http://localhost:5276/api/User/postWord?word=${codeWord.value}`);
    }
    await axios.post(`http://localhost:5276/api/User/selectCard?cardId=${currentCard.value?.id}&name=${store.currentPlayer?.name}`);
    // playersReady();
    isSelectCard.value = true;
    isSelect.value = true;
    isDisabled.value = true;
  }
  else{
    error.value = "Вы уже выбрали карточку!"
  }
}

const selectCard = async(card: Card) => {
  currentCard.value = card;
  isDisabled.value = false;
}

const sortByScore = () => {
  _.sortBy(store.players, 'score', 'desc'); //сортировка по полю score
};

watch(() => store.cards, (newValue) => {
  if(newValue?.length == store.players?.length)
    isSelectCard.value = true;
})

// watch(() => store.codeWord, (newValue) => {
//   store.codeWord = newValue;
// })

watch(()=> store.players, (newValue)=>{
  store.players = newValue;
})

onMounted(async() => {
  store.fetchPlayers();
  sortByScore();
  checkUsers = setInterval(store.fetchPlayers, 100);
  checkCards = setInterval(store.fetchCards, 3000);
  checkWord = setInterval(store.fetchWord, 3000);
  fetchScore = setInterval(sortByScore, 10000);
});

onBeforeUnmount(() => {
  clearInterval(checkUsers);
  clearInterval(fetchScore);
  clearInterval(checkCards);
  clearInterval(checkWord);
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
      &-wait{
        display: flex;
        flex-flow: column-reverse wrap;
        height: 185px;
        width: 200px;
        span{
          text-align: center;
          font-size: 18px;
        }
      }
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

.spinner {
  animation: rotate 2s linear infinite;
  z-index: 2;
  position: absolute;
  top: 50%;
  left: 50%;
  margin: -25px 0 0 -25px;
  width: 50px;
  height: 50px;
  
  & .path {
    stroke: hsl(51, 73%, 32%);
    stroke-linecap: round;
    animation: dash 1.5s ease-in-out infinite;
  }
  
}

@keyframes rotate {
  100% {
    transform: rotate(360deg);
  }
}

@keyframes dash {
  0% {
    stroke-dasharray: 1, 150;
    stroke-dashoffset: 0;
  }
  50% {
    stroke-dasharray: 90, 150;
    stroke-dashoffset: -35;
  }
  100% {
    stroke-dasharray: 90, 150;
    stroke-dashoffset: -124;
  }
}

</style>