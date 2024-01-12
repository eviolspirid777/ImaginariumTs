<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <span class="modal-container-header">
        Игроков в лобби: <span style="color:forestgreen"> {{ players?.length }}</span>
      </span>
      <span class="fa-solid fa-circle-xmark modal-container-exit" @click="hideModalWindow"></span>
      <ul>
        <li v-for="(player, key) in players" :key="key"> {{player?.name}} <i :class="[player?.isReady ? `ready-containter fa-solid fa-check fa-beat`: `ready-containter fa-solid fa-xmark fa-beat`]" :style="[player?.isReady ? `color:green;`: `color:red;`]"/></li>

      </ul>
      <div class="modal-wrapper-button">
      <button class="centered ready" @click="isReadySwitcher" @mouseover="hoverIcon = true" @mouseleave="hoverIcon = false">
          <i :class="{ 'fa-solid fa-check fa-xl': !hoverIcon, 'fa-solid fa-check fa-xl fa-beat-fade': hoverIcon }" position: absolute style="color: green;"></i>
        </button>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount, computed, watch } from "vue";
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
  align-items: stretch;

    font-size: 30px;
  li {
    // position: relative;
    // display: flex;
    // flex-flow: column wrap;
    // align-items: center;
    // justify-content: center; /* Выравнивание по горизонтали */
    // width: 130px;
    // height: 40px;
    // font-size: 30px;

    padding: 5px 20px;
    position: relative;
    text-align: center;
    max-width: 200px;
    border: 3px solid brown;
    border-radius: 10px;
    background-color: rgb(146, 106, 6);
    margin: 10px 50px;

    i {
      position: absolute;
      color: green;
      left: -35px;
      top: 50%;

    }
  }
}



.modal {
  &-wrapper{
    width: 700px;
    height: 600px;
    }
  
  &-container{
    display: flex;
    justify-content: space-between;
    width: 100%;
    height: 100%;

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

.ready-containter {
  width: 50px;
  height: 50px;
  border-radius: 100%;
  background-color: aqua;
}

</style>