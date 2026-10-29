[![CI](https://github.com/Anastasiya-Evsyukova/OrdersPractice/actions/workflows/ci.yml/badge.svg)](https://github.com/Anastasiya-Evsyukova/OrdersPractice/actions/workflows/ci.yml)

Ссылка на репозиторий: https://github.com/Anastasiya-Evsyukova/OrdersPractice.git
<img width="784" height="462" alt="image" src="https://github.com/user-attachments/assets/f387abd8-489f-42aa-ac1f-7c6cc2e3cf3b" />
<img width="886" height="135" alt="image" src="https://github.com/user-attachments/assets/9e330d10-ba05-48aa-926b-862bc3f9c57f" />

В ходе написания тестов была выявлена логическая ошибка в классе DiscountCalculator. В исходном коде условие применения скидки было строго больше (total > Threshold), из-за чего на заказы ровно на 1000 рублей скидка не начислялась. В соответствии с бизнес-требованиями («от 1000 включительно»), тест Calculate_ReturnsExpected с параметром 1000 упал. Ошибка была исправлена заменой оператора на больше или равно (total >= Threshold).
Всего написано 24 тестовых метода (37 сценариев с учетом параметризации Theory). С помощью библиотеки Moq проверено корректное взаимодействие сервисов: при ошибке сохранения заказа в репозиторий событие не публикуется (проверено через Times.Never), а при ошибке публикации — исключение пробрасывается наверх. Тесты полностью изолированы от базы данных и брокера сообщений.

<img width="1785" height="505" alt="image" src="https://github.com/user-attachments/assets/a6591b79-9379-43a0-8942-d85698a92bef" />
