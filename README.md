# Изучение C# - Пошаговое руководство

Этот проект представляет собой структурированное руководство по изучению C#. Каждая тема находится в отдельной папке с примерами кода и практическими заданиями.

## Структура проекта

### 01. Основы C#
- **01-ValueAndReferenceTypes** - Ссылочные и значимые типы данных
- **02-ModifiersAndKeywords** - Модификаторы и ключевые слова
- **03-GC-BasicConcepts** - Основные концепции сборщика мусора
- **04-GC-Generations** - Поколения сборщика мусора
- **05-GC-HeapRegions** - Области кучи
- **06-GC-WorkModes** - Режимы работы сборщика мусора
- **07-UnmanagedResources** - Неуправляемые ресурсы
- **08-MemoryManagement** - Управление памятью

### 02. Многопоточность
- **09-Multithreading-BasicConcepts** - Базовые концепты многопоточности
- **10-Multithreading-SyncPrimitives** - Примитивы синхронизации
- **11-Multithreading-Async** - Асинхронность

### 03. Типы данных и коллекции
- **12-DataTypes** - Типы данных
- **13-Collections** - Коллекции

### 04. ASP.NET
- **14-ASPNET-BasicConcepts** - Базовые концепции
- **15-ASPNET-RequestHandling** - Обработка запросов

### 05. Сетевые протоколы и коммуникации
- **16-NetworkProtocols** - Основы сетевых протоколов
- **17-NetworkCommunication** - Сетевая коммуникация
- **18-IO-Streams** - Потоки ввода-вывода

### 06. Безопасность и аутентификация
- **19-Security-Basics** - Основы безопасности
- **20-Authentication** - Аутентификация и авторизация
- **21-SecurityBestPractices** - Лучшие практики безопасности

### 07. Entity Framework
- **22-EF-BasicConcepts** - Базовые концепции
- **23-EF-ChangeTracking** - Отслеживание изменений

### 08. Git
- **24-Git-BasicConcepts** - Базовые концепции
- **25-Git-BasicCommands** - Основные команды Git

### 09. Архитектура
- **26-Architecture-OOP** - Объектно-ориентированное программирование
- **27-Architecture-Approaches** - Подходы к архитектуре
- **28-Architecture-Patterns** - Паттерны проектирования
- **29-Architecture-ArchitecturalPatterns** - Архитектурные паттерны

### 10. Message Brokers
- **30-MessageBrokers-BasicConcepts** - Основные концепции
- **31-MessageBrokers-DeliveryGuarantees** - Гарантии доставки
- **32-MessageBrokers-RabbitMQ** - Работа с RabbitMQ
- **33-MessageBrokers-Kafka** - Работа с Kafka

### 11. C# углубленно
- **34-CSharp-Compilation** - Компиляция C#
- **35-CSharp-Scope** - Области видимости
- **36-CSharp-GC** - Сборщик мусора
- **37-CSharp-ThreadSafeCollections** - Потокобезопасные коллекции

### 12. Backend протоколы
- **38-Backend-Protocols** - Протоколы бэкенда

### 13. SQL
- **39-SQL-Basics** - Основы SQL
- **40-SQL-Optimization** - Оптимизация SQL
- **41-SQL-Transactions** - Транзакции
- **42-SQL-Security** - Безопасность SQL
- **43-SQL-PostgreSQL** - Работа с PostgreSQL
- **44-SQL-Design** - Проектирование БД

## Как использовать

1. Каждая папка содержит:
   - Теоретический материал в файле README.md
   - Примеры кода в папке Examples
   - Практические задания в папке Tasks
   - Тесты для проверки знаний в папке Tests

2. Рекомендуется изучать темы последовательно, так как они построены от простого к сложному.

## Требования

- .NET 7.0 SDK или выше
- Visual Studio 2022 или Visual Studio Code
- Git для работы с репозиторием

## Структура каждой темы

Каждая тема содержит:
- `README.md` - теоретический материал
- `Examples/` - папка с примерами кода
- `Tasks/` - папка с практическими заданиями
- `Tests/` - папка с тестами для проверки знаний
- `Program.cs` - основной файл программы
- `[FolderName].csproj` - файл проекта

## Как начать

1. Клонируйте репозиторий
2. Откройте решение StudyDotNet.sln в Visual Studio
3. Начните с первой темы и двигайтесь последовательно
4. Выполняйте практические задания
5. Проверяйте свои знания с помощью тестов