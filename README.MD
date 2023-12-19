Предполагаемое разделение библиотек:
ChatCommon - интерфейсы и сообщения
ChatDb - база данных
ChatNetwork - сетевое взаимодействие
ChatApp - приложение где будет клиент и сервер (в рамках 1 семинара мы не успеем разделить клиент и сервер на два независимых приложения).
В первой работе мы создаем библиотеки и наполняем код ChatCommon и ChatDb

Теперь поработает с ChatNetwork. Добавим интерфейсов для сетевого взаимодействия 1 - для клиента, 2 - для сервера. Но сделаем

Попросите студентов подумать как можно изменить код источников сообщений таким образом
чтобы в интерфейсах не фигурировали эндпоинты. 
И заодно перенести интерфейсы в библиотеку Common чтобы они стали выглядеть следующим образом:

void Send(ChatMessage message, T toAddr);
ChatMessage Receive(ref T fromAddr);
public T CreateNewT();
public T CopyT(T t);

public interface IMessageSourceClient<T>
{
public void Send(ChatMessage message, T toAddr);
public ChatMessage Receive(ref T fromAddr);
public T CreateNewT();
public T GetServer();
}