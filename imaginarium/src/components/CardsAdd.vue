<template>
    <div class="modal-mask">
      <div class="modal-wrapper">
        <div class="modal-container">
          <span class="modal-container-header">Добавить карточки</span>
          <span class="modal-container-exit" @click="hideModalWindow">x</span>
        </div>
        <div class="modal-wrapper-imageAdd">
            <input type="file" accept="image/*" @change="onFileSelect" style="color: wheat;">
            <button @click="uploadFile">
                SaveFile
            </button>
        </div>
      </div>
    </div>
  </template>
<script lang="ts" setup>
import {ref} from "vue"
import {saveAs} from "file-saver"

const emits = defineEmits(["hideModal"]);

const hideModalWindow = () => {
emits("hideModal");
}

const selectedFile = ref();

const onFileSelect = async (event: any) => {
  //присваивает переменной изображение
  selectedFile.value = await event.target.files[0];
}

const uploadFile = async () => {
  const response = selectedFile.value 
  console.log(response);
  const fileName = `img_${Date.now()}.png`;
  saveAs(response, fileName);
  console.log('Файл успешно сохранен!');
}
</script>
<style lang="scss" scoped>
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
    width: 300px;
    height: 140px;
    background-color: #000000;
    flex-flow: column wrap;
    border-radius: 10px;
    box-shadow: 1px 1px 10px #b8c952;
    color: wheat;
    font-family: Arial, Helvetica, sans-serif;
    padding-left: 20px;
    padding-right: 20px;
    &-text{
      font-size: 18px;
      cursor: default;
      user-select: none;
      & input{
        background-color: wheat;
      }
    }
    &-imageAdd{
        display: flex;
        align-items: center;
        justify-content: center;
        flex-flow: column wrap;
        & button{
            margin-top: 5px;
            background-color: wheat;
            color: black;
            user-select: none;
            font-weight: 800;
            border: 0 solid;
            &:hover{
                background-color: rgb(218, 195, 151);
                cursor: pointer;
                box-shadow: 1px 1px 10px #b8c952;
            }
        }
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
      padding-left: 10%;
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