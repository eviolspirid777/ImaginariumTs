<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Выберите карту<strong style="font-size: 22px;">Ассоциация: {{ store.codeWord }}</strong></span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" @click="() => emits('hideModal')"></span>
        <ErrorModal :errorText="error" v-if="typeof error === 'string' && error.length" @clearError="() => error=''"></ErrorModal>
      </div>
      <ul class="modal-container-cards">
        <li v-for="(card,key) in cards" :key="key"><img :src="`../../imaginImag/${card?.card?.cardName}`" @click="selectCard(card)"></li>
      </ul>
      <div v-if="store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader == false">
        <div style="margin-top: 20px; height: 120px; width: 100%; display: flex; align-items: center; justify-content: center; flex-flow: column;">
          <strong style="font-size: x-large;">Выбранная карта</strong><br>
          <img v-if="selectedCardByUser" :src="`../../imaginImag/${selectedCardByUser?.card?.cardName}`">
        </div>
        <button v-if="selectedCardByUser" class="modal-wrapper-button-ready-small" @click="submitAnswer">Submit</button>
        <div v-else style="margin-top: 40px;">
            <svg class="spinner" viewBox="0 0 50 50">
              <circle class="path" cx="25" cy="25" r="20" fill="none" stroke-width="5"></circle>
            </svg>
            <span>Выберите карту</span>
        </div>
      </div>
      <span v-else style="font-size: large; margin-top: 50px;">Ждите...</span>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount, watch} from 'vue';
import { usePlayersStore } from '@/stores/playersStore';
import axios from 'axios';
import ErrorModal from './UI_elements/ErrorModal.vue';
import { type ScoreCard } from '@/types/ScoreCard';

const emits = defineEmits(["hideModal"]);

const store = usePlayersStore();

const cards = ref<Array<ScoreCard>>([])
const selectedCardByUser = ref<ScoreCard>()
const isValid = ref<boolean>(true)
const error = ref<String>("")

var checkCards:any;
var checkUsers:any;

const cardsRefresh = async ():Promise<void> => {
  const {data} = await axios.get('http://localhost:5276/api/User/getCurrentCards');
  cards.value = data;
}

const selectCard = async (card:ScoreCard): Promise<void> => {
  selectedCardByUser.value = card;
}

const submitAnswer = async () => {
  if(isValid.value == true && store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader == false && store.currentPlayer?.selectedCard?.cardName != selectedCardByUser.value?.card?.cardName) {
    await axios.post(`http://localhost:5276/api/User/selectUserCard?authorName=${store.currentPlayer?.name}&cardName=${selectedCardByUser.value?.card?.cardName}`);
    isValid.value = false;
  }
  else {
    if(isValid.value == false)
      error.value = 'Карта уже выбрана!';
    if(store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader == true)
      error.value = 'Вы админ!';
    if(store.currentPlayer?.selectedCard?.cardName != selectedCardByUser.value?.card?.cardName)
      error.value = 'Это ваша Карта!'
  }
}

//Если все пользователи готовы
watch(() => store.cards, async (newValue) => {
  if(newValue?.length == store.players?.length && store.players?.filter(p => p?.isLeader == false).every(p => p?.isReady)) //Проверка на наличие карты в сторе
    emits("hideModal")
})

onMounted(async() => {
  await axios.post("http://localhost:5276/api/User/unReady");
  await cardsRefresh();
  await store.fetchPlayers();
  checkCards = setInterval(cardsRefresh, 100);
  checkUsers = setInterval(store.fetchPlayers, 100);
});

onBeforeUnmount(async() => {
  await axios.post("http://localhost:5276/api/User/unReady");
  clearInterval(checkCards);
  clearInterval(checkUsers);
});
</script>

<style lang="scss" scoped>
.modal-wrapper{
  height: 400px;
  width: 800px;
  animation: none;
  box-shadow: 1px 1px 10px wheat;
}
.modal-container{
  &-header{
    font-size: 20px;
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
  }
  &-cards{
    display: flex;
    flex-flow: row wrap;
    margin-top: 20px;
    li{
      margin: 10px;
    }
  }
}
.spinner {
  animation: rotate 2s linear infinite;
  z-index: 2;
  position: absolute;
  top: 70%;
  left: 50%;
  margin: 0 0 0 -25px;
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