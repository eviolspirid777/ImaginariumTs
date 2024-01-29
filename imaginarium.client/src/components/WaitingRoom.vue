<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <span class="modal-container-header">
        Игроков в лобби: <span style="color:forestgreen"> {{ store.players?.length }}</span>
      </span>
      <span class="fa-solid fa-circle-xmark modal-container-exit" @click="hideModalWindow"></span>
      <ul>
        <li v-for="(player, key) in store.players" :key="key"> {{player?.name}} 
          <div :class="[player?.isReady ? `readiness-circle-check fa-solid fa-check`: `readiness-circle-xmark fa-solid fa-xmark`]"/>
        </li>
      </ul>
      <button class="modal-wrapper-button-ready centered" @click="isReadySwitcher" @mouseover="hoverIcon = true" @mouseleave="hoverIcon = false">
          <i :class="{ 'fa-solid fa-check fa-xl': !hoverIcon, 'fa-solid fa-check fa-xl fa-beat-fade': hoverIcon }" position: absolute style="color: green;"></i>
        </button>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount, watch } from "vue";
import { usePlayersStore } from '@/stores/playersStore';
import { playersRequest } from "../http/httpRequests"

const emits = defineEmits(["hideModal", "switch", "startGame"]);
const hoverIcon = ref(false);

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

.centered {
  position: absolute;
  bottom: 40px;
  right: 60px;
}

ul {
  position: absolute;
  width: 100%;
  height: 70%;
  top: 100px;
  display: flex;
  flex-flow: column wrap;
  align-items: center;
  font-size: 30px;

  li {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px;
    position: relative;
    background-color: darkslateblue;
    width: 260px;
    overflow: hidden;
    border: 3px solid brown;
    border-radius: 10px;
    margin: 10px;
  }
}

.modal {
  &-wrapper{
    width: 700px;
    height: 600px;
    }
  
  &-container{
    background-color: rgba(0, 0, 0, 1);
    text-align: center;
    color: wheat;
    font-size: 22px;
    
    &-header{
      font-size: 40px;
      cursor: default;
      user-select: none;
    }
  }
}

.readiness-circle {
  width: 40px;
  height: 40px;
  border-radius: 100%;
  background-color: rgb(125, 216, 22);
  color:red; 
  display: flex; 
  align-items: center; 
  justify-content: center;
  margin-left: 15px;
  right: 0;
  justify-self: right;
  position: absolute;
  margin-right: 5px;

  &-check{
    @extend .readiness-circle;
    background-color: green;
    color: rgb(0, 230, 0);
  }
  &-xmark{
    @extend .readiness-circle;
    background-color: rgb(131, 40, 40);
    color: red;
  }
}

</style>