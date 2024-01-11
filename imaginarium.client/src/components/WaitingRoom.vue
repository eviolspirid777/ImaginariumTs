<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Комната ожидания: {{headerText}}</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <ul v-for="(player,key) in store.players" :key="key">
        <li v-if="player">{{player.name}} <i :class="[player.isReady ? `fa-solid fa-check fa-beat`: `fa-solid fa-xmark fa-beat`]" :style="[player.isReady ? `color:green;`: `color:red;`]"/></li>
      </ul>
      <button class="modal-wrapper-ready" @click="isReadySwitcher">Ready</button>
    </div>
  </div>
</template>
<script lang="ts" setup>
import axios from 'axios';
import { ref, onMounted, onBeforeUnmount, computed, watch } from "vue";
import { usePlayersStore } from '@/stores/playersStore';
import { playersRequest } from "../http/httpRequests"

const emits = defineEmits(["hideModal", "switch", "startGame"]);

const store = usePlayersStore();

const isReady = ref<boolean>();

let checkUsers:any;
let checkUserState:any;

watch(() => store.players, (newValue) => {
  let cnt = 0;
  newValue?.forEach( player => {
    if(store.players && player?.isReady){
      cnt++;
      if(newValue.length == cnt)
        emits("startGame");
    }
  })
})

const checkState = async():Promise<void> => {
  isReady.value = await playersRequest.userGet(`checkState?name=${store.currentPlayer?.name}`)
}

const isReadySwitcher = ():void => {
  emits("switch");
};

const fetchPlayers = async ():Promise<void> => {
  try {
    store.players = await playersRequest.userGet(`getUsers`);
  } catch (error) {
    console.error('Error fetching players:', error);
  }
};

const headerText = computed(() => {
  if(store.players){
    if(store.players?.length == 1)
      return `${store.players.length} игрок`;
    else if(store.players && store.players?.length > 1 && store.players?.length < 5)
      return `${store.players.length} игрока`;
    return `${store.players.length} игроков`;
  }
  return "";
})

const hideModalWindow = async ():Promise<void> => {
  emits("hideModal");
};

onMounted(() => {
  fetchPlayers();
  checkUsers = setInterval(fetchPlayers, 100);
  checkUserState = setInterval(checkState, 100);
});

onBeforeUnmount(() => {
  clearInterval(checkUsers);
  clearInterval(checkUserState);
});
</script>
<style scoped lang="scss">
ul{
  li{
    display: flex;
    flex-flow: row nowrap;
    align-items: center;
    justify-content: center;
    width: 80px;
    height: 40px;
    max-width: 120px;
    border: 1px solid wheat;
    margin-top: 10px;
    i {
      color: green;
      margin-left: 18px;
    }
  }
}
.modal {
  &-wrapper{
    width: 37%;
    height: 40%;
    max-width: 37%;
    max-height: 40%;
    background-color: #000000;
    display: flex;
    flex-flow: column wrap;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    padding-left: 20px;
    padding-right: 20px;
    &-ready{
      height: 40px;
      width: 80px;
      margin-left: 600px;
      margin-top: 80px;
      color: black;
      background-color: wheat;
      border: 0px solid wheat;
      border-radius: 2px;
    }
  }

  &-container{
    display: flex;
    justify-content: space-between;
    width: 90%;
    border-radius: 10px;
    background-color: rgba(0, 0, 0, 1);
    padding: 20px;
    padding-bottom: 20px;
    text-align: center;
    color: wheat;
    font-size: 22px;
    &-header{
      padding-left: 25%;
      cursor: default;
      user-select: none;
      &.-active{
        
      }
    }
    &-exit{
      display: flex;
      align-items: center;
      justify-content: center;
      width: 30px;
      height: 25px;
      background-color: wheat;
      border-radius: 2px;
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