<template>
  <header class="header">
    <img class="header-logo" src="../icons/base-imaginarium-logo.png" alt="Имаджинариум" />
    <span v-for="(val, key) in helpMenu" :key="key" class="header-menu">
      <span class="header-menu-item" @click="displaySwitcher(val.key)">
        <i :class="val.class" /> {{ val.value }}
      </span>
    </span>
  </header>
  <main class="main">
    <AutorizationWindow v-show="autorizeWindow" @hideModal="autorizeComplete"/>
    <HelpWindow v-show="helpWindow" @hideModal="() => helpWindow = false" />
    <CardsWindow v-show="cardsWindow" @hide-modal="() => cardsWindow = false"/>
    <WaitingRoom v-if="waitingRoomWindow" @hide-modal="() => waitingRoomWindow = false" :selected-user="currentUser"/>
      <!-- <div>
        <button @click="test">qweqwe</button>
      </div> -->
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

const autorizeWindow = ref(false);
const helpWindow = ref(false);
const cardsWindow = ref(false);
const waitingRoomWindow = ref(false);

const currentUser = ref ();

const helpMenu = ref([
{ key: "play", value: "Играть", class: "fa-sharp fa-regular fa-game-board"},
{ key: "help", value: "Помощь", class: "fa-sharp fa-regular fa-question"},
{ key: "upload", value: "Загрузить", class:"fa-regular fa-cards"}
])

const displaySwitcher = (val:string) => {
  if(val === "play" && helpWindow.value === false)
    autorizeWindow.value = true;
  else if(val === "help"&& autorizeWindow.value === false)
    helpWindow.value = true;
  else if( val === "upload" && (autorizeWindow.value === false && helpWindow.value === false))
    cardsWindow.value = true;
}

// const test = async () => {
//   let res = await axios.get("https://localhost:7186/api/Room/listRooms");
//   console.log("my data:",res.data)
// }

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
  }

}

</script>

<style scoped lang="scss">
// :root{

// }
.header{
  display: flex;
  position: absolute;
  width: 1875px;
  height: 60px;
  background-color: rgba(0,0,0,0.8);
  align-items: center;
  user-select: none;
  &-logo{
    color: white;
    margin-left: 20px;
    width: 100px;
    height: 60px;
  }
  &-menu{
    &-item{
      margin-left: 30px;
      color: white;
      cursor: pointer;
      font-family: cursive;
      font-size: 15px;
      & i{
        color: white;
      }
      &:hover{
        color: wheat;
        font-size: 18px;
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
  display: flex;
  align-items: center;
  width: 1875px;
  height: 40px;
  background-color: rgba(0,0,0,0.6);
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