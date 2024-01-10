<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Добавить карточки</span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" style="font-size: 30px;" @click="hideModalWindow"></span>
      </div>
      <div class="modal-wrapper-imageAdd">
          <input type="file" accept="image/*" @change="selectFile" style="color: wheat;">
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

const hideModalWindow = ():void => {
emits("hideModal");
}

const selectedFile = ref<File | null>();

const selectFile = async (event: Event):Promise<void> => {
  const inputElement = event.target as HTMLInputElement;
  selectedFile.value = inputElement.files ? inputElement.files[0] : null;
  // let b = this.$refs.fileInput.file[0];
}

const uploadFile = async ():Promise<void> => {
  const response = selectedFile.value 
  const fileName = `img_${Date.now()}.png`;
  saveAs(response, fileName);
  console.log('Файл успешно сохранен!');
}
</script>
<style lang="scss" scoped>
.modal {
  &-wrapper{
    width: 300px;
    height: 140px;

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
    font-size: 22px;
    &-header{
      padding-left: 10%;
      cursor: default;
      user-select: none;
    }
    
  }
}
</style>