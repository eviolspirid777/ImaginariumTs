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
      <span class="modal-wrapper-error" v-if="error.length > 0">
        {{ error }}
      </span>
      <div class="modal-wrapper-button">
        <button type="button" @click="login">Enter</button>
      </div>
    </div>
  </div>
</template>
<script lang="ts" setup>
import { ref } from "vue"
import axios from "axios"

const emits = defineEmits(["hideModal"]);

const error = ref("")
const playerName = ref("")
// const playerPassword = ref("")

const hideModalWindow = () => {
  emits("hideModal", {});
}

// const translateText = async (text:string) => {
//   const encodedParams = new URLSearchParams();
//   encodedParams.set('from', 'en');
//   encodedParams.set('to', 'ru');
//   encodedParams.set('text', `${text}`);
  
//   const options = {
//     method: 'POST',
//     url: 'https://google-translate113.p.rapidapi.com/api/v1/translator/text',
//     headers: {
//       'content-type': 'application/x-www-form-urlencoded',
//       'X-RapidAPI-Key': 'faeb50bffdmsh9219c56861df2a4p12d215jsn05d3202774f1',
//       'X-RapidAPI-Host': 'google-translate113.p.rapidapi.com'
//     },
//     data: encodedParams,
//   }
//   const response = await axios.request(options);
//   console.log(response.data);
// }

const login = async () => {
  // translateText('it is me');
  try{
    if(playerName.value.length > 0){
      let response = (await axios.post(`http://localhost:5276/api/Users/autorize?sendName=${playerName.value}`));
      if(response.status == 400){
        error.value = "Такого пользователя нет!";
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
    top: 140;
    left: 0;
    width: 100%;
    height: 89%;
    background-color: rgba(0, 0, 0, 0.6);
    align-items: center;
    justify-content: center;
  }

  &-wrapper{
    width: 400px;
    height: 145px;
    background-color: #000000;
    display: flex;
    justify-content: flex-start;
    align-items: center;
    flex-flow: column nowrap;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    font-family: Arial, Helvetica, sans-serif;
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
        margin-right: 80px;
        margin-left: 80px;
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