<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Введите имя</span>
        <span class="modal-container-exit" @click="hideModalWindow">x</span>
      </div>
      <span class="modal-wrapper-name">
        Имя <input v-model="playerName"/>
      </span>
      <span class="modal-wrapper-password">
        Пароль <input v-model="playerPassword"/>
      </span>
      <span class="modal-wrapper-error" v-if="error.length > 0">
        {{ error }}
      </span>
      <div class="modal-wrapper-button">
        <button type="button" @click="registerAccount">Reg</button>
        <button type="button" @click="enterTheGame">Enter</button>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue"
import { useRouter } from 'vue-router'
import axios from "axios"

const router = useRouter();
const emits = defineEmits(["hideModal"]);

const error = ref("")
const playerName = ref("")
const playerPassword = ref("")

const hideModalWindow = () => {
  emits("hideModal");
}

const enterTheGame = async () => {
  try{
    if(playerName.value.length > 0 && playerPassword.value.length > 0){
      let user = {name:playerName.value, password:playerPassword.value};
      let response = (await axios.get(`http://localhost:5276/api/Users/autorize?name=${user.name}&password=${user.password}`));
      if(response.status == 404){
        error.value = "Неправильный пароль!";
        throw new Error("Неправильный пароль!");
      }
      else if(response.status == 400){
        error.value = "Такого пользователя нет!";
        throw new Error("Такого пользователя нет!");
      }
      console.log(response);
      router.push({ name: 'WaitingRoom'});
      playerName.value = "";
      playerPassword.value ="";
      emits("hideModal");
    }
    else {
      error.value = "Имя или пароль не введены!";
      throw new Error("Имя или пароль не введены!");
    }
  }
  catch(ex){
    console.error(ex)
  }
}

const registerAccount = async () => {
  try{
    if(playerName.value.length > 0 && playerPassword.value.length > 0) {
      let user = {name:playerName.value, password:playerPassword.value};
      let response = await axios.post(`http://localhost:5276/api/Users/register`, user);
      if(response.status == 400){
        error.value = "Такой пользователь уже есть!";
        throw new Error("Такой пользователь уже есть!");
      }
      playerName.value = "";
      playerPassword.value ="";
      emits("hideModal");
    }
    else {
      error.value = "Не ввели имя или пароль!";
      throw new Error("Не ввели имя или пароль!");
    }
  }
  catch(ex){
    console.error(ex);
  }
}
</script>
<style lang="scss" scoped>
.main-modal{
  margin-top: -20px;
  width: 500px;
  height: 300px;
  background-color: black;
  border-radius: 10px;
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
    width: 400px;
    height: 200px;
    background-color: #000000;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-flow: column nowrap;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    font-family: Arial, Helvetica, sans-serif;
    &-name{
      padding-left: 30px;
      font-size: 20px;
      cursor: default;
      user-select: none;
      & input{
        background-color: wheat;
      }
    }
    &-password{
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
        margin-right: 80px;
        margin-left: 70px;
        margin-top: 10px;
        min-height: 30px;
        width: 80px;
        background-color: wheat;
        font-weight: 800;
        border: 0 solid;
        border-radius: 1px;
        &:hover{
          background-color: rgb(218, 195, 151);
          box-shadow: 1px 1px 10px #b8c952;
        }
      }
    }
  }

  &-container{
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
    &-header{
      padding-left: 30%;
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