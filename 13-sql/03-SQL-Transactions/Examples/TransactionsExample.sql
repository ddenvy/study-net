-- Пример 1: Базовая транзакция
BEGIN TRANSACTION;

INSERT INTO orders (customer_id, order_date, total_amount)
VALUES (1, CURRENT_DATE, 1000.00);

INSERT INTO order_items (order_id, product_id, quantity, unit_price)
VALUES (LAST_INSERT_ID(), 1, 2, 500.00);

COMMIT;

-- Пример 2: Транзакция с точкой сохранения
BEGIN TRANSACTION;

INSERT INTO customers (customer_name, email)
VALUES ('Иван Иванов', 'ivan@example.com');

SAVEPOINT customer_created;

INSERT INTO orders (customer_id, order_date, total_amount)
VALUES (LAST_INSERT_ID(), CURRENT_DATE, 1500.00);

-- Если что-то пошло не так, можно откатиться к точке сохранения
ROLLBACK TO SAVEPOINT customer_created;

-- Или продолжить транзакцию
COMMIT;

-- Пример 3: Уровни изоляции
-- Установка уровня изоляции
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

BEGIN TRANSACTION;

-- Чтение данных
SELECT * FROM products WHERE product_id = 1;

-- Другая транзакция может изменить данные

-- Повторное чтение может показать другие данные
SELECT * FROM products WHERE product_id = 1;

COMMIT;

-- Пример 4: Обработка тупиков
BEGIN TRANSACTION;

-- Первая транзакция
UPDATE products SET stock = stock - 1 WHERE product_id = 1;
UPDATE products SET stock = stock - 1 WHERE product_id = 2;

-- Вторая транзакция (в другом соединении)
-- UPDATE products SET stock = stock - 1 WHERE product_id = 2;
-- UPDATE products SET stock = stock - 1 WHERE product_id = 1;

-- При возникновении тупика
-- ERROR: deadlock detected

ROLLBACK;

-- Пример 5: Долгая транзакция
BEGIN TRANSACTION;

-- Установка таймаута
SET lock_timeout = '5s';

-- Операции, которые могут занять много времени
UPDATE large_table SET status = 'processed' WHERE status = 'pending';

-- Если операция занимает слишком много времени, транзакция будет прервана
COMMIT;

-- Пример 6: Вложенные транзакции
BEGIN TRANSACTION;

INSERT INTO orders (customer_id, order_date, total_amount)
VALUES (1, CURRENT_DATE, 2000.00);

-- Вложенная транзакция
SAVEPOINT nested_transaction;

INSERT INTO order_items (order_id, product_id, quantity, unit_price)
VALUES (LAST_INSERT_ID(), 1, 1, 2000.00);

-- Откат вложенной транзакции
ROLLBACK TO SAVEPOINT nested_transaction;

-- Продолжение основной транзакции
INSERT INTO order_items (order_id, product_id, quantity, unit_price)
VALUES (LAST_INSERT_ID(), 2, 2, 1000.00);

COMMIT;

-- Пример 7: Распределенная транзакция
BEGIN TRANSACTION;

-- Операции с первой базой данных
INSERT INTO orders (customer_id, order_date, total_amount)
VALUES (1, CURRENT_DATE, 3000.00);

-- Операции со второй базой данных
INSERT INTO inventory (product_id, quantity)
VALUES (1, -1);

-- Если все операции успешны
COMMIT;

-- Пример 8: Транзакция с проверкой условий
BEGIN TRANSACTION;

-- Проверка условий
SELECT stock FROM products WHERE product_id = 1 FOR UPDATE;

-- Если условие выполняется
IF (SELECT stock FROM products WHERE product_id = 1) >= 1 THEN
    UPDATE products SET stock = stock - 1 WHERE product_id = 1;
    INSERT INTO orders (customer_id, order_date, total_amount)
    VALUES (1, CURRENT_DATE, 1000.00);
    COMMIT;
ELSE
    ROLLBACK;
END IF; 