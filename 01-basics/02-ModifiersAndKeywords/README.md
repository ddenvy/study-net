# Модификаторы и ключевые слова в C#

## Теория

### Модификаторы доступа
- `public` - доступно везде
- `private` - доступно только внутри класса
- `protected` - доступно внутри класса и в наследниках
- `internal` - доступно внутри сборки
- `protected internal` - доступно внутри сборки и в наследниках
- `private protected` - доступно внутри сборки только в наследниках

### Модификаторы класса
- `static` - статический класс/член
- `abstract` - абстрактный класс/метод
- `sealed` - запечатанный класс
- `partial` - разделение класса на части

### Модификаторы членов класса
- `readonly` - только для чтения
- `const` - константа
- `virtual` - виртуальный метод
- `override` - переопределение метода
- `new` - скрытие члена базового класса
- `async` - асинхронный метод

### Ключевые слова
- `this` - ссылка на текущий экземпляр
- `base` - ссылка на базовый класс
- `using` - директива для импорта пространств имен
- `namespace` - объявление пространства имен
- `using static` - импорт статических членов
- `nameof` - получение имени переменной/члена

## Примеры кода

В папке `Examples` вы найдете примеры использования различных модификаторов и ключевых слов.

## Практические задания

1. Создайте класс с различными модификаторами доступа
2. Реализуйте наследование с использованием модификаторов
3. Создайте статический класс с утилитами
4. Продемонстрируйте работу с ключевыми словами this и base

## Тесты

В папке `Tests` находятся тесты для проверки ваших знаний. 