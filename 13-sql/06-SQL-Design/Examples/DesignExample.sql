-- Пример 1: Создание нормализованной схемы
-- Таблица клиентов (1NF)
CREATE TABLE customers (
    customer_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблица адресов (2NF)
CREATE TABLE addresses (
    address_id SERIAL PRIMARY KEY,
    customer_id INTEGER REFERENCES customers(customer_id),
    address_type VARCHAR(20) NOT NULL,
    street VARCHAR(100) NOT NULL,
    city VARCHAR(50) NOT NULL,
    state VARCHAR(50),
    postal_code VARCHAR(20) NOT NULL,
    country VARCHAR(50) NOT NULL
);

-- Таблица заказов (3NF)
CREATE TABLE orders (
    order_id SERIAL PRIMARY KEY,
    customer_id INTEGER REFERENCES customers(customer_id),
    order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) NOT NULL,
    total_amount DECIMAL(10,2) NOT NULL
);

-- Таблица товаров (3NF)
CREATE TABLE products (
    product_id SERIAL PRIMARY KEY,
    product_name VARCHAR(100) NOT NULL,
    description TEXT,
    price DECIMAL(10,2) NOT NULL,
    category_id INTEGER REFERENCES categories(category_id)
);

-- Таблица категорий (3NF)
CREATE TABLE categories (
    category_id SERIAL PRIMARY KEY,
    category_name VARCHAR(50) NOT NULL,
    parent_category_id INTEGER REFERENCES categories(category_id)
);

-- Таблица элементов заказа (3NF)
CREATE TABLE order_items (
    order_item_id SERIAL PRIMARY KEY,
    order_id INTEGER REFERENCES orders(order_id),
    product_id INTEGER REFERENCES products(product_id),
    quantity INTEGER NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL
);

-- Пример 2: Создание индексов
-- Индекс для поиска по email
CREATE INDEX idx_customers_email ON customers(email);

-- Составной индекс для поиска по имени и фамилии
CREATE INDEX idx_customers_name ON customers(last_name, first_name);

-- Индекс для поиска заказов по дате
CREATE INDEX idx_orders_date ON orders(order_date);

-- Индекс для поиска товаров по категории
CREATE INDEX idx_products_category ON products(category_id);

-- Пример 3: Партиционирование таблицы
-- Создание партиционированной таблицы заказов
CREATE TABLE orders_partitioned (
    order_id SERIAL,
    customer_id INTEGER,
    order_date TIMESTAMP,
    status VARCHAR(20),
    total_amount DECIMAL(10,2)
) PARTITION BY RANGE (order_date);

-- Создание партиций по месяцам
CREATE TABLE orders_2024_01 PARTITION OF orders_partitioned
    FOR VALUES FROM ('2024-01-01') TO ('2024-02-01');

CREATE TABLE orders_2024_02 PARTITION OF orders_partitioned
    FOR VALUES FROM ('2024-02-01') TO ('2024-03-01');

-- Пример 4: Создание представлений
-- Представление для отчета по продажам
CREATE VIEW sales_report AS
SELECT 
    c.customer_name,
    o.order_date,
    p.product_name,
    oi.quantity,
    oi.unit_price,
    oi.quantity * oi.unit_price as total_amount
FROM customers c
JOIN orders o ON c.customer_id = o.customer_id
JOIN order_items oi ON o.order_id = oi.order_id
JOIN products p ON oi.product_id = p.product_id;

-- Пример 5: Создание триггеров
-- Триггер для обновления общей суммы заказа
CREATE OR REPLACE FUNCTION update_order_total()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE orders
    SET total_amount = (
        SELECT SUM(quantity * unit_price)
        FROM order_items
        WHERE order_id = NEW.order_id
    )
    WHERE order_id = NEW.order_id;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER order_items_update
AFTER INSERT OR UPDATE OR DELETE ON order_items
FOR EACH ROW EXECUTE FUNCTION update_order_total();

-- Пример 6: Создание ограничений
-- Ограничение на положительную цену
ALTER TABLE products
ADD CONSTRAINT positive_price CHECK (price > 0);

-- Ограничение на положительное количество
ALTER TABLE order_items
ADD CONSTRAINT positive_quantity CHECK (quantity > 0);

-- Ограничение на статус заказа
ALTER TABLE orders
ADD CONSTRAINT valid_status CHECK (status IN ('pending', 'processing', 'shipped', 'delivered', 'cancelled'));

-- Пример 7: Создание функций
-- Функция для расчета скидки
CREATE OR REPLACE FUNCTION calculate_discount(
    customer_id INTEGER,
    order_amount DECIMAL
) RETURNS DECIMAL AS $$
DECLARE
    discount_rate DECIMAL;
BEGIN
    -- Определение скидки на основе истории заказов
    SELECT 
        CASE 
            WHEN COUNT(*) > 10 THEN 0.15
            WHEN COUNT(*) > 5 THEN 0.10
            WHEN COUNT(*) > 0 THEN 0.05
            ELSE 0
        END INTO discount_rate
    FROM orders
    WHERE customer_id = $1;
    
    RETURN order_amount * discount_rate;
END;
$$ LANGUAGE plpgsql;

-- Пример 8: Создание типов данных
-- Создание составного типа для адреса
CREATE TYPE address_type AS (
    street VARCHAR(100),
    city VARCHAR(50),
    state VARCHAR(50),
    postal_code VARCHAR(20),
    country VARCHAR(50)
);

-- Создание перечисления для статуса заказа
CREATE TYPE order_status AS ENUM (
    'pending',
    'processing',
    'shipped',
    'delivered',
    'cancelled'
); 