<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Имаджинариум</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <div class="modal-wrapper-game">
        <div class="modal-wrapper-game-score">
            <span class="modal-wrapper-game-score-header">Очки:</span>
            <ul v-for="(player,key) in players" :key="key">
                <li v-if="player">{{player.name}} : {{ player.score }}</li>
            </ul>
        </div>
        <div v-for="(player,key1) in players" :key="key1" class="modal-wrapper-game-cards">
            <ul v-for="(card,key2) in player.cards" :key="key2">
                <li><img :src="`../../imaginImag/${card.cardName}`"></li>
            </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {ref, onMounted, onBeforeUnmount, watch} from "vue"
import axios from "axios";
import type { User } from "@/types/User";

const emits = defineEmits(["hideModal"]);

const players = ref<Array<User>>();

let checkUsers:any;
let fetchScore:any;

const hideModalWindow = () => {
    emits("hideModal");
}

<<<<<<< HEAD
/** 
Реализовать watch за players, чтобы можно было отслеживать текущее кол-во очков 
**/
=======
watch(() => players.value, (newValue) => {
  players.value = newValue;
})
>>>>>>> 04cba4d7fbef1d3ef56ddf9689bf18678fba6707

const fetchPlayers = async ():Promise<void> => {
  try {
    const response = await axios.get(`http://localhost:5276/api/User/getUsers`);
    players.value = response.data;
  } catch (error) {
    console.error('Error fetching players:', error);
  }
};

const sortByScore = () => {
  players.value?.sort((a,b) => b?.score - a?.score);
}

onMounted(() => {
  fetchPlayers();
  sortByScore();
  checkUsers = setInterval(fetchPlayers, 100);
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
    width: 95%;
    height: 98%;
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
        min-width: 9%;
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
<<<<<<< HEAD
      padding-left: 46%;
      font-family: Apple Chancery, cursive;
=======
      padding-left: 43%;
>>>>>>> 04cba4d7fbef1d3ef56ddf9689bf18678fba6707
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