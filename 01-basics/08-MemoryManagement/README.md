# Управление памятью в C#

## 1. Управляемая и неуправляемая память

### Управляемая память
- Управляется сборщиком мусора (GC)
- Автоматическое выделение и освобождение
- Типы: `Memory<T>`, `Span<T>`, `ReadOnlyMemory<T>`, `ReadOnlySpan<T>`
- Пул памяти: `MemoryPool<T>`, `ArrayPool<T>`

### Неуправляемая память
- Требует ручного управления
- Методы работы:
  - `Marshal.AllocHGlobal` / `Marshal.FreeHGlobal`
  - `NativeMemory.Alloc` / `NativeMemory.Free`
  - `GCHandle` для закрепления объектов

## 2. Сборщик мусора (GC)

### Поколения
- Generation 0: новые объекты
- Generation 1: пережившие первую сборку
- Generation 2: долгоживущие объекты

### Методы управления
- `GC.Collect()` - принудительная сборка
- `GC.WaitForPendingFinalizers()` - ожидание финализаторов
- `GC.GetTotalMemory()` - получение информации о памяти

## 3. Ссылки и закрепление

### Типы ссылок
- Strong Reference - обычная ссылка
- Weak Reference - слабая ссылка
- Soft Reference - мягкая ссылка

### Закрепление объектов
- `GCHandle.Alloc` с `GCHandleType.Pinned`
- `MemoryHandle` для `Memory<T>`
- `fixed` statement

## 4. Пул памяти

### MemoryPool<T>
- Управление блоками памяти
- Методы:
  - `Rent()` - получение блока
  - `Return()` - возврат блока

### ArrayPool<T>
- Пул массивов
- Оптимизация для часто используемых массивов
- Методы:
  - `Rent()` - получение массива
  - `Return()` - возврат массива

## 5. Безопасность памяти

### Span<T> и Memory<T>
- Безопасная работа с памятью
- Поддержка стека и кучи
- Оптимизация производительности

### MemoryMarshal
- Преобразование типов памяти
- Безопасное приведение типов
- Работа с байтами

## 6. Лучшие практики

### Рекомендации
1. Использовать `using` для освобождения ресурсов
2. Избегать утечек памяти
3. Правильно работать с неуправляемыми ресурсами
4. Использовать пул памяти для оптимизации
5. Следить за закреплением объектов

### Антипаттерны
1. Не освобождать неуправляемую память
2. Слишком часто вызывать GC.Collect()
3. Долго держать закрепленные объекты
4. Игнорировать Dispose
5. Неправильно использовать WeakReference 