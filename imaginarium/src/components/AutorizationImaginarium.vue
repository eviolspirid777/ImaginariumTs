<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Введите имя</span>
    
        <span class="modal-container-exit" @click="hideModalWindow"><i class="fa-solid fa-circle-xmark"></i></span>
      </div>
      <span class="modal-wrapper-name">
        <input v-model="playerName"/>
      </span>
      <span class="modal-wrapper-error" v-if="error.length > 0">
        {{ error }}
      </span>
      <div class="modal-wrapper-button">
        <button type="button" @click="login" @mouseover="hoverIcon = true" @mouseleave="hoverIcon = false">
          <i :class="{'fa-solid fa-check fa-xl': !hoverIcon, 'fa-solid fa-check fa-xl fa-beat-fade': hoverIcon}" style="color: green;"></i>
        </button>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue"
import axios from "axios"

const emits = defineEmits(["hideModal", "closeWindow"]);

const hoverIcon = ref(false);
const error = ref("")
const playerName = ref("")

const hideModalWindow = () => {
  emits("closeWindow");
}

const login = async () => {
  try{
    if(playerName.value.length > 0){
      let response = (await axios.post(`http://localhost:5276/api/Users/autorize?sendName=${playerName.value}`));
      if(response.status == 400){
        error.value = "Такого пользователя нет или он уже в игре!";
        throw new Error("Такого пользователя нет!");
      }
      console.log(response.data);
      playerName.value = "";
      emits("hideModal", response.data);
    }
    else {
      error.value = "Имя не введено!";
      throw new Error("Имя не введено!");
    }
  }
  catch(ex){
    console.error(ex)
  }
}
</script>
<style lang="scss" scoped>
.main-modal{
  margin-top: -20px;
  width: 500px;
  height: 230px;
  background-color: black;
  border-radius: 10px;
}
.modal {
  &-mask{
    position: absolute;
    display: flex;
    top: 0px;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.6);
    align-items: center;
    justify-content: center;
  }

  &-wrapper{
    width: 400px;
    height: 150px;
    position: relative; 
    background-color: #000000;
    display: flex;
    justify-content: flex-start;
    align-items: center;
    flex-flow: column nowrap;
    border-radius: 10px;
    box-shadow: 0px 0px 30px #dd1313;
    color: wheat;
    font-family: Arial, Helvetica, sans-serif;

    animation: pulseShadow 1.5s infinite alternate ease-in-out;
    @keyframes pulseShadow {
      from {
        box-shadow: 0px 0px 15px #dd1313;
      }
      to {
        box-shadow: 0px 0px 60px #dd1313;
      }
    }

    &-name{
      font-size: 20px;
      cursor: default;
      user-select: none;
      & input{
        background-color: wheat;
      }
    }
    &-error{
      color: red;
      font-size: 16px;
    }
    &-button{
      display: flex;
        button{
        margin-top: 10px;
        min-height: 30px;
        width: 80px;
        background-color: wheat;
        font-weight: 800;
        border: 0 solid;
        border-radius: 6px;
        &:hover{
          background-color: rgb(218, 195, 151);
          box-shadow: 0px 0px 12px #0ffc22;
        }
      }
    }
  }

  &-container {
  position: relative;
  display: flex;
  width: 360px;
  justify-content: space-between;
  border-radius: 10px;
  background-color: rgba(0, 0, 0, 1);
  padding: 20px;
  padding-bottom: 20px;
  text-align: center;
  color: wheat;
  font-family: Arial, Helvetica, sans-serif;
  font-size: 22px;

  &-header {
    margin: 0 auto; // Центрируем текст
    cursor: default;
    user-select: none;
  }

  &-exit {
    color: red;
    margin: 10px;
    right: 0;
    top: 0;
    user-select: none;
    position: absolute;

    &:hover {
      cursor: pointer;
    }
  }
}

}
</style>