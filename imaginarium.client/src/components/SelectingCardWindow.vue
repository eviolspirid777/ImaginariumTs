<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Выберите карту: {{ store.codeWord }}</span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" @click="hideModalWindow"></span>
      </div>
      <ul class="modal-container-cards">
        <li v-for="(card,key) in cards" :key="key"><img :src="`../../imaginImag/${card?.card?.cardName}`" @click="selectCard(card)"></li>
      </ul>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount } from 'vue';
import { usePlayersStore } from '@/stores/playersStore';
import axios from 'axios';
import { type ScoreCard } from '@/types/ScoreCard';

const emits = defineEmits(["hideModal","nextAdmin"]);

const store = usePlayersStore();

const cards = ref<Array<ScoreCard>>([])

var checkCards:any;

const hideModalWindow = ():void => {
  emits("hideModal");
}

const cardsRefresh = async ():Promise<void> => {
  const response = await axios.get('http://localhost:5276/api/User/getCurrentCards');
  cards.value = response.data;
}

const selectCard = (card:ScoreCard):void => {
  console.log(card)
}

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
  }
}
</style>