-- Пример 1: Анализ плана выполнения
EXPLAIN ANALYZE
SELECT o.order_id, c.customer_name, p.product_name
FROM orders o
JOIN customers c ON o.customer_id = c.customer_id
JOIN order_items oi ON o.order_id = oi.order_id
JOIN products p ON oi.product_id = p.product_id
WHERE o.order_date >= '2024-01-01';

-- Пример 2: Оптимизация индексов
-- Создание составного индекса
CREATE INDEX idx_orders_customer_date 
ON orders(customer_id, order_date);

-- Создание частичного индекса
CREATE INDEX idx_active_products 
ON products(product_name) 
WHERE is_active = true;

-- Пример 3: Оптимизация запроса с подзапросами
-- Неоптимальный вариант
SELECT c.customer_name,
       (SELECT COUNT(*) FROM orders WHERE customer_id = c.customer_id) as order_count,
       (SELECT SUM(total_amount) FROM orders WHERE customer_id = c.customer_id) as total_spent
FROM customers c;

-- Оптимизированный вариант
SELECT c.customer_name,
       COUNT(o.order_id) as order_count,
       COALESCE(SUM(o.total_amount), 0) as total_spent
FROM customers c
LEFT JOIN orders o ON c.customer_id = o.customer_id
GROUP BY c.customer_name;

-- Пример 4: Использование временных таблиц
WITH monthly_sales AS (
    SELECT 
        DATE_TRUNC('month', order_date) as month,
        SUM(total_amount) as total_sales
    FROM orders
    GROUP BY DATE_TRUNC('month', order_date)
)
SELECT 
    month,
    total_sales,
    LAG(total_sales) OVER (ORDER BY month) as previous_month_sales,
    (total_sales - LAG(total_sales) OVER (ORDER BY month)) / 
    LAG(total_sales) OVER (ORDER BY month) * 100 as growth_percentage
FROM monthly_sales;

-- Пример 5: Материализованное представление
CREATE MATERIALIZED VIEW mv_product_sales AS
SELECT 
    p.product_id,
    p.product_name,
    COUNT(oi.order_item_id) as total_orders,
    SUM(oi.quantity) as total_quantity,
    SUM(oi.quantity * oi.unit_price) as total_revenue
FROM products p
LEFT JOIN order_items oi ON p.product_id = oi.product_id
GROUP BY p.product_id, p.product_name;

-- Обновление материализованного представления
REFRESH MATERIALIZED VIEW mv_product_sales;

-- Пример 6: Параллельное выполнение
SET max_parallel_workers_per_gather = 4;
SET parallel_setup_cost = 10;
SET parallel_tuple_cost = 0.1;

EXPLAIN ANALYZE
SELECT 
    c.category_name,
    COUNT(p.product_id) as product_count,
    AVG(p.price) as avg_price
FROM categories c
JOIN products p ON c.category_id = p.category_id
GROUP BY c.category_name;

-- Пример 7: Оптимизация JOIN
-- Неоптимальный вариант
SELECT o.order_id, c.customer_name, p.product_name
FROM orders o, customers c, order_items oi, products p
WHERE o.customer_id = c.customer_id
AND o.order_id = oi.order_id
AND oi.product_id = p.product_id;

-- Оптимизированный вариант
SELECT o.order_id, c.customer_name, p.product_name
FROM orders o
INNER JOIN customers c ON o.customer_id = c.customer_id
INNER JOIN order_items oi ON o.order_id = oi.order_id
INNER JOIN products p ON oi.product_id = p.product_id;

-- Пример 8: Использование оконных функций
SELECT 
    customer_id,
    order_date,
    total_amount,
    SUM(total_amount) OVER (
        PARTITION BY customer_id 
        ORDER BY order_date
        ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
    ) as running_total
FROM orders
ORDER BY customer_id, order_date; 