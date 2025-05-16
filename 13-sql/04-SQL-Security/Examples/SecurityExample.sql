-- Пример 1: Создание пользователей и ролей
-- Создание роли
CREATE ROLE sales_team;

-- Создание пользователя
CREATE USER sales_user WITH PASSWORD 'secure_password';

-- Назначение роли пользователю
GRANT sales_team TO sales_user;

-- Пример 2: Управление правами доступа
-- Предоставление прав на чтение
GRANT SELECT ON customers TO sales_team;

-- Предоставление прав на вставку
GRANT INSERT ON orders TO sales_team;

-- Отзыв прав
REVOKE DELETE ON products FROM sales_team;

-- Пример 3: Защита от SQL-инъекций
-- Небезопасный запрос
-- SELECT * FROM users WHERE username = 'admin' AND password = 'password';

-- Безопасный запрос с параметрами
PREPARE user_query (text, text) AS
SELECT * FROM users WHERE username = $1 AND password = $2;

EXECUTE user_query('admin', 'password');

-- Пример 4: Шифрование данных
-- Создание таблицы с зашифрованными данными
CREATE TABLE secure_customers (
    customer_id SERIAL PRIMARY KEY,
    customer_name TEXT,
    credit_card pgp_sym_encrypt(credit_card_number, 'encryption_key'),
    email TEXT
);

-- Вставка зашифрованных данных
INSERT INTO secure_customers (customer_name, credit_card, email)
VALUES (
    'Иван Иванов',
    pgp_sym_encrypt('4111111111111111', 'encryption_key'),
    'ivan@example.com'
);

-- Пример 5: Маскирование данных
-- Создание представления с маскированными данными
CREATE VIEW masked_customers AS
SELECT 
    customer_id,
    customer_name,
    '****-****-****-' || RIGHT(credit_card, 4) as masked_credit_card,
    email
FROM customers;

-- Пример 6: Аудит действий
-- Создание таблицы аудита
CREATE TABLE audit_log (
    log_id SERIAL PRIMARY KEY,
    user_id TEXT,
    action TEXT,
    table_name TEXT,
    record_id INTEGER,
    action_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Создание триггера для аудита
CREATE OR REPLACE FUNCTION audit_trigger()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO audit_log (user_id, action, table_name, record_id)
    VALUES (current_user, TG_OP, TG_TABLE_NAME, NEW.id);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Применение триггера
CREATE TRIGGER customers_audit
AFTER INSERT OR UPDATE OR DELETE ON customers
FOR EACH ROW EXECUTE FUNCTION audit_trigger();

-- Пример 7: Политики безопасности
-- Создание политики RLS (Row Level Security)
ALTER TABLE orders ENABLE ROW LEVEL SECURITY;

CREATE POLICY sales_team_policy ON orders
    FOR ALL
    TO sales_team
    USING (region = current_setting('app.current_region'));

-- Пример 8: Мониторинг безопасности
-- Создание функции для проверки подозрительной активности
CREATE OR REPLACE FUNCTION check_suspicious_activity()
RETURNS TABLE (
    user_id TEXT,
    action_count INTEGER,
    last_action TIMESTAMP
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        user_id,
        COUNT(*) as action_count,
        MAX(action_time) as last_action
    FROM audit_log
    WHERE action_time > NOW() - INTERVAL '1 hour'
    GROUP BY user_id
    HAVING COUNT(*) > 100;
END;
$$ LANGUAGE plpgsql; 