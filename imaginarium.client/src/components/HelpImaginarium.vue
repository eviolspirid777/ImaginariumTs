<template>
  <div class="modal-mask">
    <div class="modal-wrapper">
      <div class="modal-container">
        <span class="modal-container-header">Правила</span>
        <span class="fa-solid fa-circle-xmark modal-container-exit" @click="hideModalWindow"></span>
      </div>
      <span class="modal-wrapper-text">
        Добро пожаловать в игру "Имаджинариум"! Это захватывающая настольная игра, где вы окунетесь в мир воображения,
         креативности и стратегии. В "Имаджинариум" ваша задача - создать самую впечатляющую и удивительную коллекцию изображений.
         В игре вы будете использовать ресурсы, такие как карточки с изображениями, жетоны и специальные утилиты,
          чтобы создавать и расширять свою коллекцию. Ваша цель состоит в том, чтобы набрать наибольшее количество очков,
           объединяя изображения и строя фантастические композиции.<br><br>Вот несколько основных правил игры:<br>
      </span>
      <ul class="modal-wrapper-list">
        <li>
          <strong>Начало игры:</strong>
          <ul>
            <li>Каждый игрок получает игровое поле, ресурсы и набор карт изображений.</li>
            <li>Карты раскладываются на столе в общую колоду.</li>
          </ul>
        </li>
        <li>
          <strong>Ход игры:</strong>
          <ul>
            <li>Игроки проходят через серию раундов, в каждом из которых доступны различные действия.</li>
            <li>Действия могут включать приобретение новых карт изображений, активацию утилит, использование специальных способностей и т.д.</li>
          </ul>
        </li>
        <li>
          <strong>Сбор изображений:</strong>
          <ul>
            <li>Игроки выбирают карты из общей колоды или используют специальные утилиты, чтобы добавить изображения в свою коллекцию.</li>
            <li>Карты изображений имеют различные категории и характеристики, которые могут влиять на вашу стратегию и набранные очки.</li>
          </ul>
        </li>
        <li>
          <strong>Строительство композиций:</strong>
          <ul>
            <li>Ваша задача - создать фантастические композиции, объединяя карты изображений на вашем игровом поле.</li>
            <li>Каждая композиция может приносить вам очки в зависимости от комбинации изображений и выполнения определенных условий.</li>
          </ul>
        </li>
        <li>
          <strong>Оценка и подсчет очков:</strong>
          <ul>
            <li>В конце игры происходит подсчет очков на основе ваших построенных композиций и выполненных целей.</li>
            <li>Чем более разнообразные и впечатляющие ваши композиции, тем больше очков вы можете набрать.</li>
          </ul>
        </li>
      </ul>
      <span class="modal-wrapper-text">
        <br>Помните, что "Имаджинариум" - игра, где ваша креативность и способность строить уникальные композиции будут тесно связаны с вашей стратегией. Используйте свой воображаемый мир наилучшим образом, чтобы стать лучшим коллекционером изображений!<br><br>Удачи и приятной игры в "Имаджинариум"! 🎲✨
      </span>
    </div>
  </div>
</template>
<script lang="ts" setup>

const emits = defineEmits(["hideModal"]);

const hideModalWindow = ():void => {
  emits("hideModal");
}
</script>
<style lang="scss" scoped>
.modal {
  &-wrapper{
    width: 1000px;
    height: 700px;
    padding-left: 20px;
    padding-right: 20px;
    font-size: 18px;
    &-list{
      padding-left: 20px;
      padding-top: 10px;
      line-height: 1.1;
      & li{
        padding-left: 15px;
        padding-top: 4px;
        list-style-type: decimal;
      }
    }
  }

  &-container{
    display: flex;
    justify-content: space-between;
    margin-bottom: 40px;
    border-radius: 10px;
    background-color: rgba(0, 0, 0, 1);
    padding: 20px;
    padding-bottom: 20px;
    text-align: center;
    color: wheat;
    &-header{
      font-size: 28px;
      cursor: default;
      user-select: none;
    }

  }
}
</style>