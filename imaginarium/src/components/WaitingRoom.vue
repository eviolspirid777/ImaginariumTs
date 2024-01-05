<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Комната ожидания</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <ul v-for="(player,key) in players" :key="key">
        <li>{{player.name}} <i :class="[isValid ? `fa-solid fa-check fa-beat`: `fa-solid fa-xmark fa-beat`]" :style="[isValid ? `color:green;`: `color:red;`]"/></li>
      </ul>
      <button class="modal-wrapper-ready" @click="isReadySwitcher">Ready</button>
    </div>
  </div>
</template>
<script lang="ts" setup>
import axios from 'axios';
import {ref, onMounted, watch} from "vue";

const emits = defineEmits(["hideModal"]);

const props = defineProps({
    selectedUser: {
        type: Object,
        default: () => {}
    }
});

const players = ref(undefined);
const currentPlayer = ref(props.selectedUser);
const isValid = ref(false);


watch(() => props.selectedUser, (newVal) => {
  currentPlayer.value = newVal;
})

const isReadySwitcher = () => {
  console.log(currentPlayer.value)
  currentPlayer.value.isReady = !currentPlayer.value.isReady
  isValid.value = currentPlayer.value.isReady
}

const fetchPlayers = async () => {
  try {
    const response = await axios.get(`http://localhost:5276/api/Users/getUsers`);
    players.value = response.data;
  } catch (error) {
    console.error('Error fetching players:', error);
  }
};

onMounted(() => {
  fetchPlayers();
  setInterval(fetchPlayers, 5000);
});

const hideModalWindow = async () => {
  console.log("this is:",currentPlayer.value)
  await axios.post(`http://localhost:5276/api/Users/sliceUser`, currentPlayer.value);
  emits("hideModal", currentPlayer.value);
}
</script>
<style scoped lang="scss">
ul{
  li{
    width: 90px;
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
    top: 140;
    left: 0;
    width: 100%;
    height: 94%;
    background-color: rgba(0, 0, 0, 0.6);
    align-items: center;
    justify-content: center;
  }

  &-wrapper{
    width: 40%;
    height: 40%;
    background-color: #000000;
    flex-flow: column wrap;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    font-family: Arial, Helvetica, sans-serif;
    padding-left: 20px;
    padding-right: 20px;
    &-ready{
      width: 80px;
      height: 50px;
      margin-left: 85%;
      margin-top: 24%;
      color: black;
      background-color: wheat;
      border: 0px solid wheat;
      border-radius: 2px;
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
    font-family: Arial, Helvetica, sans-serif;
    font-size: 22px;
    &-header{
      padding-left: 35%;
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