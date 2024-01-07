<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Комната ожидания: {{headerText}}</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <ul v-for="(player,key) in players" :key="key">
        <li v-if="player">{{player.name}} <i :class="[player.isReady ? `fa-solid fa-check fa-beat`: `fa-solid fa-xmark fa-beat`]" :style="[player.isReady ? `color:green;`: `color:red;`]"/></li>
      </ul>
      <button class="modal-wrapper-ready" @click="isReadySwitcher">Ready</button>
    </div>
  </div>
</template>
<script lang="ts" setup>
import type { User } from '@/types/User';
import axios from 'axios';
import { ref, onMounted, onBeforeUnmount, computed, watch } from "vue";

const emits = defineEmits(["hideModal", "switch", "startGame"]);

const props = defineProps({
  selectedUser: {
    type: Object,
    default:() => {}
  }
})

const players = ref<Array<User>>();
const isReady = ref<boolean>();

let checkUsers:any;
let checkUserState:any;

watch(() => players.value, (newValue) => {
  let cnt = 0;
  newValue?.forEach( player => {
    if(player?.isReady){
      cnt++;
      if(newValue.length == cnt)
        emits("startGame");
    }
  })
})

const checkState = async():Promise<void> => {
  isReady.value = (await axios.get(`http://localhost:5276/api/User/checkState?name=${props.selectedUser.name}`)).data;
}

const isReadySwitcher = ():void => {
  emits("switch");
};

const fetchPlayers = async ():Promise<void> => {
  try {
    const response = await axios.get(`http://localhost:5276/api/User/getUsers`);
    players.value = response.data;
  } catch (error) {
    console.error('Error fetching players:', error);
  }
};

const headerText = computed(() =>{
  if(players.value){
    if(players.value?.length == 1)
      return `${players.value.length} игрок`;
    else if(players.value && players.value?.length > 1 && players.value?.length < 5)
      return `${players.value.length} игрока`;
    return `${players.value.length} игроков`;
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
  &-mask{
    position: absolute;
    display: flex;
    left: 0;
    margin-bottom: 16px;
    width: 100%;
    height: 87%;
    background-color: rgba(0, 0, 0, 0.6);
    align-items: center;
    justify-content: center;
  }

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
    font-family: Arial, Helvetica, sans-serif;
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
    font-family: Arial, Helvetica, sans-serif;
    font-size: 22px;
    &-header{
      padding-left: 25%;
      cursor: default;
      user-select: none;
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