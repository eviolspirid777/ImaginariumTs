<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Комната ожидания</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <ul v-for="(player,key) in players" :key="key">
        <li>{{player.name}} <i :class="[player.isReady ? `fa-solid fa-check fa-beat`: `fa-solid fa-xmark fa-beat`]" :style="[player.isReady ? `color:green;`: `color:red;`]"/></li>
      </ul>
      <button class="modal-wrapper-ready" @click="isReadySwitcher">Ready</button>
    </div>
  </div>
</template>
<script lang="ts" setup>
import axios from 'axios';
import { ref, onMounted, onBeforeUnmount } from "vue";

const emits = defineEmits(["hideModal", "switch"]);

const props = defineProps({
  selectedUser: {
    type: Object,
    default:() => {}
  }
})

const players = ref();
const isReady = ref();

let checkUsers:any;
let checkUserState:any;

const checkState = async() => {
  isReady.value = (await axios.get(`http://localhost:5276/api/User/checkState?name=${props.selectedUser.name}`)).data;
}

const isReadySwitcher = () => {
  emits("switch");
};

const fetchPlayers = async () => {
  try {
    const response = await axios.get(`http://localhost:5276/api/User/getUsers`);
    players.value = response.data;
  } catch (error) {
    console.error('Error fetching players:', error);
  }
};

const hideModalWindow = async () => {
  emits("hideModal");
};

onMounted(() => {
  fetchPlayers();
  checkUsers = setInterval(fetchPlayers, 100);
  checkUserState = setInterval(checkState, 100)
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
      padding-left: 35%;
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