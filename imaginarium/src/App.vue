<template>
  <header class="header">
    <!-- <img class="header-logo" src="../icons/base-imaginarium-logo.png" alt="Имаджинариум" /> --> 
    <span v-for="(val, key) in helpMenu" :key="key" class="header-menu">
      <span :style="{ fontSize: val.fontsize }" 
      class="header-menu-item" @click="displaySwitcher(val.key)">
        <i :class="val.iconclass" /> {{ val.value }}
      </span>
  </span>

  </header>
  <main class="main">
    <AutorizationWindow v-if="autorizeWindow" @hideModal="autorizeComplete" @close-window="() => autorizeWindow = false"/>
    <HelpWindow v-show="helpWindow" @hideModal="() => helpWindow = false" />
    <CardsWindow v-show="cardsWindow" @hide-modal="() => cardsWindow = false"/>
    <WaitingRoom v-if="waitingRoomWindow" @hide-modal="hideWaitingRoom" :selected-user="currentUser" @switch="validateSwitcher"/>
  </main>
  <footer class="footer">
    <div class="footer-vk">
      <i class="fa-brands fa-vk"></i>
      <a class="footer-vk-tag" href="https://vk.com/id252193795">Вконтакте</a>
    </div>
  </footer>
</template>

<script setup lang="ts">
import {ref} from "vue"
import AutorizationWindow from "../src/components/AutorizationImaginarium.vue"
import HelpWindow from "../src/components/HelpImaginarium.vue"
import CardsWindow from "../src/components/CardsAdd.vue"
import "../icons/main.scss"
import WaitingRoom from "./components/WaitingRoom.vue"
import axios from "axios"

const autorizeWindow = ref(false);
const helpWindow = ref(false);
const cardsWindow = ref(false);
const waitingRoomWindow = ref(false);

const currentUser = ref([{}]);

const helpMenu = ref([
  { key: "upload", value: "Загрузить", iconclass: "fa-regular fa-cards", fontsize: "17px" },
  { key: "play", value: "Играть", iconclass: "fa-sharp fa-regular fa-game-board", fontsize: "36px" },
  { key: "help", value: "Описание", iconclass: "fa-sharp fa-regular fa-question", fontsize: "17px" }
]);

const hideWaitingRoom = async () => {
  waitingRoomWindow.value = false;
  if(currentUser.value.name != undefined)
    await axios.post(`https://localhost:7186/api/Users/sliceUser?name=${currentUser.value.name}`);
};

const validateSwitcher = async () => {
  await axios.get(`http://localhost:5276/api/Users/switchReady?name=${currentUser.value.name}`);
};

const displaySwitcher = (val:string) => {
  if(val === "play" && helpWindow.value === false && cardsWindow.value === false)
    autorizeWindow.value = true;
  else if(val === "help"&& autorizeWindow.value === false && cardsWindow.value === false)
    helpWindow.value = true;
  else if( val === "upload" && autorizeWindow.value === false && helpWindow.value === false)
    cardsWindow.value = true;
}

const autorizeComplete = (user: any) => {
  if(Object.keys(user).length > 0)
  {
    autorizeWindow.value = false;
    waitingRoomWindow.value = true;
    currentUser.value = user;
  }
  else
  {
    autorizeWindow.value = false;
    waitingRoomWindow.value = true;
  }

}
</script>

<style scoped lang="scss">
// :root{

// }
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
    padding: 5px 30px; /* Верхние и нижние отступы */
    margin: 0px 40px;
    border-radius: 30px;
    cursor: pointer;
    &:hover {
        transform: scale(1.5);
        background-color: rgba(255, 166, 0, 0.37); /* Цвет текста при наведении на .header-menu */
      }

    &-item {
      color: white;
      font-family: cursive;
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
  &-vk-tag{
    font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;
    font-weight: 800;
    font-size: 18px;
    margin-left: 20px;
    text-decoration: none;
    user-select: none;
    &:visited{
      color: white;
    }
    &:hover{
      font-size: 20px;
      color: wheat;
    }
  }
}
</style>