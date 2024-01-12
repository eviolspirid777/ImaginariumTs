<template>
  <header class="header">
    <span v-for="(val, key) in helpMenu" :key="key" class="header-menu">
      <span :style="{ fontSize: val.fontsize }" 
      class="header-menu-item" @click="displaySwitcher(val.key)">
        <i :class="val.iconclass" /> {{ val.value }}
      </span>
    </span>
  </header>
  <main class="main">
    <AutorizationWindow v-if="windowsValid[windows.AUTORIZATION]" @hideModal="autorizeComplete" @close-window="() => windowsValid[windows.AUTORIZATION] = false"/>
    <HelpWindow v-show="windowsValid[windows.HELP]" @hideModal="() => windowsValid[windows.HELP] = false" />
    <CardsWindow v-show="windowsValid[windows.CARD]" @hide-modal="() => windowsValid[windows.CARD] = false"/>
    <WaitingRoom v-if="windowsValid[windows.WAITING]" @hide-modal="hideWaitingRoom" @start-game="startGame" @switch="validateSwitcher"/>
    <GameWindow v-if="windowsValid[windows.GAME]" @hide-modal="hideGameRoom"/>
  </main>
  <footer class="footer">
    <div class="footer-vk">
      <i class="fa-brands fa-vk"></i>
      <a class="footer-vk-tag" @click="redirectToVk" href="https://vk.com/id252193795">Вконтакте</a>
    </div>
  </footer>
</template>

<script setup lang="ts">
import {ref} from "vue"
import AutorizationWindow from "../src/components/AutorizationImaginarium.vue"
import HelpWindow from "../src/components/HelpImaginarium.vue"
import CardsWindow from "../src/components/CardsAdd.vue"
import WaitingRoom from "./components/WaitingRoom.vue"
import GameWindow from "../src/components/GameImaginarium.vue"
import {usePlayersStore} from "../src/stores/playersStore"
import axios from "axios"

const playersStore = usePlayersStore();

const helpMenu = ref<Array<any>>([
  { key: "upload", value: "Загрузить", iconclass: "fa-regular fa-cards", fontsize: "17px" },
  { key: "play", value: "Играть", iconclass: "fa-sharp fa-regular fa-game-board", fontsize: "36px" },
  { key: "help", value: "Правила", iconclass: "fa-sharp fa-regular fa-question", fontsize: "17px" }
]);

const windows = {
  AUTORIZATION: 0,
  CARD: 1,
  HELP: 2,
  WAITING: 3,
  GAME: 4
}

const windowsValid = ref({
  [windows.AUTORIZATION]: false,
  [windows.CARD]: false,
  [windows.HELP]: false,
  [windows.WAITING]: false,
  [windows.GAME]: false
})

const startGame = async ():Promise<any> => {
  windowsValid.value[windows.WAITING] = false;
  await axios.get("http://localhost:5276/api/User/randomCards");
  windowsValid.value[windows.GAME] = true;
}

const hideWaitingRoom = async ():Promise<any> => {
  windowsValid.value[windows.WAITING] = false;
  if(playersStore.currentPlayer != undefined)
    await axios.post(`http://localhost:5276/api/User/sliceUser?name=${playersStore.currentPlayer.name}`);
};

const hideGameRoom = async ():Promise<any> => {
  windowsValid.value[windows.GAME] = false;
  if(playersStore.currentPlayer != undefined)
    await axios.post(`http://localhost:5276/api/User/sliceUser?name=${playersStore.currentPlayer.name}`);
}

const validateSwitcher = async ():Promise<any> => {
  if(playersStore.currentPlayer != undefined)
    await axios.post(`http://localhost:5276/api/User/switchReady?name=${playersStore.currentPlayer.name}`);
};

const displaySwitcher = (val:string):any => {
  if(val === "play" && windowsValid.value[windows.HELP] === false && windowsValid.value[windows.CARD] === false)
    windowsValid.value[windows.AUTORIZATION] = true;
  else if(val === "help"&& windowsValid.value[windows.AUTORIZATION] === false && windowsValid.value[windows.CARD] === false)
    windowsValid.value[windows.HELP] = true;
  else if( val === "upload" && windowsValid.value[windows.AUTORIZATION] === false && windowsValid.value[windows.HELP] === false)
    windowsValid.value[windows.CARD] = true;
}

const autorizeComplete = (user: any) => {
  windowsValid.value[windows.AUTORIZATION] = false;
  windowsValid.value[windows.WAITING] = true;
  playersStore.currentPlayer = user;
}

const redirectToVk = ():void => {
  window.open('https://vk.com/avavictor');
}
</script>

<style scoped lang="scss">
.header {
  display: flex;
  position: fixed;
  width: 100%;
  height: 9%;
  background-color: rgba(0, 0, 0, 0.8);
  align-items: center;
  justify-content: center;
  user-select: none;

  &-logo {
    color: white;
    margin-left: 20px;
    width: 100px;
    height: 60px;
  }

  &-menu {
    transition: background-color 0.2s, transform 0.2s;
    padding: 5px 30px; 
    margin: 0px 40px;
    border-radius: 30px;
    cursor: pointer;
    &:hover {
        transform: scale(1.5);
        background-color: rgba(255, 166, 0, 0.37); 
      }

    &-item {
      color: white;
      font-size: 20px;
      
      & i {
        color: rgb(129, 104, 58);
      }

    }
  }
}
.main{
  padding-top: 60px;
  width: var(--my-width);
  height: 896px;
  display: flex;
  align-items: center;
  justify-content: center;
  &-modal{
    margin-top: -20px;
    width: 500px;
    height: 300px;
    background-color: black;
    border-radius: 10px;
  }

}

.footer{
  position: fixed;
  bottom: 0;
  display: flex;
  align-items: center;
  width: 100%;
  height: 40px;
  background-color: rgba(0, 0, 0, 0.6);
  z-index: 10;
  &-vk-tag{
    font-weight: 800;
    font-size: 18px;
    margin-left: 20px;
    text-decoration: none;
    user-select: none;
    color: wheat;
    &:visited{
      color: wheat;
    }
    &:hover{
      font-size: 20px;
      color: wheat;
    }
  }
}
</style>