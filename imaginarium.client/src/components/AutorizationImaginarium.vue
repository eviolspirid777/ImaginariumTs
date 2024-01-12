<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Введите имя...</span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" @click="hideModalWindow"></span>
        <ErrorModal :errorText="error" v-if="error.length" @clearError="() => error=''"></ErrorModal>
      </div>
      <span class="modal-wrapper-name">
        <input v-model="playerName"/>
      </span>

      <div class="modal-wrapper-button">
        <button class="ready" @click="login" @mouseover="hoverIcon = true" @mouseleave="hoverIcon = false">
          <i :class="{ 'fa-solid fa-check fa-xl': !hoverIcon, 'fa-solid fa-check fa-xl fa-beat-fade': hoverIcon }" style="color: green;"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import ErrorModal from "../components/UI_elements/ErrorModal.vue";
import { playersRequest } from "@/http/httpRequests";

const emits = defineEmits(["hideModal", "closeWindow"]);

const hoverIcon = ref<Boolean>(false);
const error = ref<String>("");
const playerName = ref<String>("");

const hideModalWindow = (): void => {
  emits("closeWindow");
};

const login = async (): Promise<void> => {
  try {
    if (playerName.value.length > 0) {
      const response = await playersRequest.userPostResponse(`autorize?sendName=${playerName.value}`)
      if (response.status === 204) {
        error.value = "Пользователь уже в игре!";
        throw new Error("Такого пользователя нет!");
      }
      emits("hideModal", response.data);
    } else {
      error.value = "Имя не введено!";
      throw new Error("Имя не введено!");
    }
  } catch (ex) {
    console.error(ex);
  }
};
</script>

<style lang="scss" scoped>
.i {
  top: 30px;
}

.modal {
  &-wrapper {
    width: 400px;
    height: 150px;

    &-name {
      font-size: 20px;
      cursor: default;
      user-select: none;

      & input {
        width: 250px;
        background-color: #fff;
      }
    }
    &-errorText{
      font-size: 55px;
      position: absolute;
      color: red;
      font-style: italic;
      font-weight: bold;
    }

  }

  &-container {
    background-color: rgba(0, 0, 0, 1);
    padding: 20px;
    color: wheat;
    font-size: 30px;

    &-header {
      margin: 0 auto; // Центрируем текст
      user-select: none;
    }
  }
}

</style>