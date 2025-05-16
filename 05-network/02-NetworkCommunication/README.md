# Работа с сетью в C#

## 1. TCP/IP

### TcpListener
- Прослушивание TCP-соединений
- Методы:
  - `Start()`
  - `AcceptTcpClient()`
  - `AcceptSocket()`
  - `Stop()`

### TcpClient
- TCP-клиент
- Методы:
  - `Connect()`
  - `GetStream()`
  - `Close()`

## 2. UDP

### UdpClient
- UDP-клиент
- Методы:
  - `Send()`
  - `Receive()`
  - `Connect()`
  - `Close()`

## 3. HTTP

### HttpClient
- HTTP-клиент
- Методы:
  - `GetAsync()`
  - `PostAsync()`
  - `PutAsync()`
  - `DeleteAsync()`

### HttpListener
- HTTP-сервер
- Методы:
  - `Start()`
  - `GetContext()`
  - `Stop()`

## 4. Сокеты

### Socket
- Низкоуровневые сетевые операции
- Методы:
  - `Connect()`
  - `Bind()`
  - `Listen()`
  - `Accept()`
  - `Send()`
  - `Receive()`

## 5. DNS

### Dns
- Работа с DNS
- Методы:
  - `GetHostEntry()`
  - `GetHostAddresses()`
  - `GetHostName()`

## 6. IP-адреса

### IPAddress
- Работа с IP-адресами
- Методы:
  - `Parse()`
  - `TryParse()`
  - `GetAddressBytes()`

### IPEndPoint
- Конечная точка сети
- Свойства:
  - `Address`
  - `Port`

## 7. Асинхронные операции

### Асинхронные методы
- `ConnectAsync()`
- `SendAsync()`
- `ReceiveAsync()`
- `AcceptAsync()`

### Отмена операций
- `CancellationToken`
- `CancellationTokenSource`

## 8. Безопасность

### SSL/TLS
- `SslStream`
- Сертификаты
- Шифрование

### Аутентификация
- Basic Auth
- OAuth
- JWT

## 9. Лучшие практики

### Рекомендации
1. Использовать асинхронные операции
2. Правильно обрабатывать исключения
3. Закрывать соединения
4. Использовать таймауты
5. Применять безопасные протоколы

### Антипаттерны
1. Не закрывать соединения
2. Игнорировать исключения
3. Использовать синхронные операции
4. Не использовать таймауты
5. Игнорировать безопасность 