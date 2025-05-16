-- Пример 1: Создание расширений
-- Расширение для работы с JSON
CREATE EXTENSION IF NOT EXISTS "jsonb";

-- Расширение для шифрования
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Расширение для полнотекстового поиска
CREATE EXTENSION IF NOT EXISTS "pg_trgm";

-- Пример 2: Работа с JSON
-- Создание таблицы с JSON данными
CREATE TABLE product_catalog (
    product_id SERIAL PRIMARY KEY,
    product_name VARCHAR(100),
    attributes JSONB,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Вставка JSON данных
INSERT INTO product_catalog (product_name, attributes)
VALUES (
    'Смартфон',
    '{
        "brand": "Samsung",
        "model": "Galaxy S21",
        "specs": {
            "screen": "6.2 inch",
            "ram": "8GB",
            "storage": "128GB"
        },
        "colors": ["black", "white", "blue"]
    }'::jsonb
);

-- Поиск по JSON
SELECT product_name, attributes->>'brand' as brand
FROM product_catalog
WHERE attributes->>'brand' = 'Samsung';

-- Пример 3: Полнотекстовый поиск
-- Создание таблицы с текстовыми данными
CREATE TABLE articles (
    article_id SERIAL PRIMARY KEY,
    title VARCHAR(200),
    content TEXT,
    search_vector tsvector
);

-- Создание индекса для полнотекстового поиска
CREATE INDEX articles_search_idx ON articles USING GIN (search_vector);

-- Обновление вектора поиска
UPDATE articles
SET search_vector = to_tsvector('russian', title || ' ' || content);

-- Поиск по тексту
SELECT title, content
FROM articles
WHERE search_vector @@ to_tsquery('russian', 'поиск & текст');

-- Пример 4: Партиционирование
-- Создание партиционированной таблицы
CREATE TABLE sales (
    sale_id SERIAL,
    sale_date DATE NOT NULL,
    amount DECIMAL(10,2),
    customer_id INTEGER
) PARTITION BY RANGE (sale_date);

-- Создание партиций
CREATE TABLE sales_2024_01 PARTITION OF sales
    FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

CREATE TABLE sales_2024_02 PARTITION OF sales
    FOR VALUES FROM ('2024-02-01') TO ('2024-03-01');

-- Пример 5: Материализованные представления
-- Создание материализованного представления
CREATE MATERIALIZED VIEW monthly_sales AS
SELECT 
    DATE_TRUNC('month', sale_date) as month,
    SUM(amount) as total_sales,
    COUNT(*) as number_of_sales
FROM sales
GROUP BY DATE_TRUNC('month', sale_date);

-- Обновление материализованного представления
REFRESH MATERIALIZED VIEW monthly_sales;

-- Пример 6: Репликация
-- Настройка репликации (на основном сервере)
ALTER SYSTEM SET wal_level = replica;
ALTER SYSTEM SET max_wal_senders = 10;
ALTER SYSTEM SET max_replication_slots = 10;

-- Создание пользователя для репликации
CREATE USER replicator WITH REPLICATION ENCRYPTED PASSWORD 'replication_password';

-- Пример 7: Мониторинг
-- Создание представления для мониторинга
CREATE VIEW database_stats AS
SELECT 
    datname as database_name,
    numbackends as active_connections,
    xact_commit as transactions_committed,
    xact_rollback as transactions_rollback,
    blks_read as blocks_read,
    blks_hit as blocks_hit
FROM pg_stat_database;

-- Пример 8: Расширенные типы данных
-- Создание составного типа
CREATE TYPE address AS (
    street VARCHAR(100),
    city VARCHAR(50),
    postal_code VARCHAR(20)
);

-- Создание перечисления
CREATE TYPE order_status AS ENUM (
    'pending',
    'processing',
    'shipped',
    'delivered'
);

-- Использование составного типа
CREATE TABLE customers (
    customer_id SERIAL PRIMARY KEY,
    customer_name VARCHAR(100),
    billing_address address,
    shipping_address address
);

-- Пример 9: Триггеры и функции
-- Создание функции для логирования
CREATE OR REPLACE FUNCTION log_changes()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO change_log (
        table_name,
        record_id,
        action,
        changed_at
    ) VALUES (
        TG_TABLE_NAME,
        NEW.id,
        TG_OP,
        CURRENT_TIMESTAMP
    );
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Создание триггера
CREATE TRIGGER customers_log_changes
AFTER INSERT OR UPDATE OR DELETE ON customers
FOR EACH ROW EXECUTE FUNCTION log_changes();

-- Пример 10: Расширенные возможности
-- Использование оконных функций
SELECT 
    customer_id,
    sale_date,
    amount,
    SUM(amount) OVER (
        PARTITION BY customer_id 
        ORDER BY sale_date
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) as running_total
FROM sales;

-- Использование регулярных выражений
SELECT product_name
FROM product_catalog
WHERE product_name ~ '^[A-Z]';

-- Использование массивов
SELECT product_name, attributes->'colors' as available_colors
FROM product_catalog
WHERE 'black' = ANY(attributes->'colors'); 