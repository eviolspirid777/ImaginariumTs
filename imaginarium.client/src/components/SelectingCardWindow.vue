<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Выберите карту: {{ store.codeWord }}</span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" @click="() => emits('hideModal')"></span>
        <ErrorModal :errorText="error" v-if="typeof error === 'string' && error.length" @clearError="() => error=''"></ErrorModal>
      </div>
      <ul class="modal-container-cards">
        <li v-for="(card,key) in cards" :key="key"><img :src="`../../imaginImag/${card?.card?.cardName}`" @click="selectCard(card)"></li>
      </ul>
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
const isValid = ref<boolean>(true)
const error = ref<String>("")

var checkCards:any;


const cardsRefresh = async ():Promise<void> => {
  const response = await axios.get('http://localhost:5276/api/User/getCurrentCards');
  cards.value = response.data;
}

const selectCard = async (card:ScoreCard): Promise<void> => {
  // newCard.value = {...card} as any;
  // if(typeof newCard.value?.name != `undefined`)
  //   newCard.value?.name.push(store.currentPlayer?.name);
  // console.log(newCard.value)
  // if(store.currentPlayer?.name != undefined)
  //   newCard.value?.name?.push(store.currentPlayer?.name);
  // await axios.post('http://localhost:5276/api/User/postCard', newCard.value);
  if(isValid.value == true && store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader == false && store.currentPlayer?.selectedCard?.cardName != card?.card?.cardName) {
    await axios.post(`http://localhost:5276/api/User/selectUserCard?authorName=${store.currentPlayer?.name}&cardName=${card?.card?.cardName}`);
    isValid.value = false;
  }
  else{
    if(isValid.value == false)
      error.value = 'Карта уже выбрана!';
    if(store.players?.find(p => p?.name == store.currentPlayer?.name)?.isLeader == true)
      error.value = 'Вы админ!';
    if(store.currentPlayer?.selectedCard?.cardName != card?.card?.cardName)
      error.value = 'Это ваша Карта!'
  }
}

//Если все пользователи готовы
watch(() => store.players, async (newValue) => {
  if(newValue?.every(p => p?.isReady)){
    await axios.post('http://localhost:5276/api/User/fetchScore')
    emits("hideModal");
  }
})

onMounted(async() => {
  await cardsRefresh()
  checkCards = setInterval(cardsRefresh, 100);
});

onBeforeUnmount(() => {
  clearInterval(checkCards);
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
</style>