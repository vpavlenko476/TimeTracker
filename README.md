# Конфигурация

## time-tracker-backend
Сервис предназначен для отслеживания Jira задач и сохранения временного промежутка в рамках которого задача находилась в статусе InProgress.
 
GET /jira-items?begin={date-time}&end={date-time} - получение задач, над которыми работал пользователь

POST /log-time - создание записи о работе над задачей d Jira (логирование времени)

### Зависимости

Сервис зависит от следующих внешних сервисов:

1. Jira

### Подключение к БД

| Environment variable       | Description               | Default value |
|----------------------------|---------------------------|---------------|
| POSTGRES_CONNECTION_STRING | Подключение к БД Postgres |               |

### Подключение к брокеру сообщений

| Environment variable    | Description         | Default value |
|-------------------------|---------------------|---------------|
| KAFKA_CONNECTION_STRING | Подключение к Kafka |               |

### Пути до внешних сервисов

| Environment variable | Description                 | Default value |
|----------------------|-----------------------------|---------------|
| REST_JIRA_URL        | Базовый URL до сервиса Jira |               |

### Прочие настройки

| Environment variable | Description                                           | Default value |
|----------------------|-------------------------------------------------------|---------------|
| JIRA_AUTH_TOKEN      | Jira Api Token пользоваетля                           |               |
| TASK_REQUEST_DELAY   | Частота опроса сервиса Jira на наличие задач в работе | '00:01:00'    |

## time-tracker-frontend

Клиент к time-tracker-backend

| Environment variable  | Description                                    | Default value         |
|-----------------------|------------------------------------------------|-----------------------|
| REST_TIME_TRACKER_URL | Подключение к time-tracker-backend             | http://localhost:5100 |
| MEETING_TASK_KEY      | Номер задачи для списывания времени на встречи | 'IDPPSOP-4'           |

## Как развернуть

1. Установаить в time-tracker-backend.env обязательные параметры JIRA_AUTH_TOKEN и REST_JIRA_URL формата "https://host"
2. запустить docker-compose файл, для этого перейти в корень проекта и выолнить команду docker-compose up 
  
time-tracker-frontend будет доступн по адресу http://localhost:5200/ 

time-tracker-backend будет доступн по адресу http://localhost:5100/

   ``
