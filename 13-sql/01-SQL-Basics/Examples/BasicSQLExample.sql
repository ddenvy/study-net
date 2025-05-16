-- Пример 1: Создание таблиц
CREATE TABLE employees (
    employee_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) UNIQUE,
    hire_date DATE NOT NULL,
    salary DECIMAL(10,2),
    department_id INTEGER
);

CREATE TABLE departments (
    department_id SERIAL PRIMARY KEY,
    department_name VARCHAR(50) NOT NULL,
    location VARCHAR(100)
);

-- Пример 2: Вставка данных
INSERT INTO departments (department_name, location)
VALUES 
    ('IT', 'Москва'),
    ('HR', 'Санкт-Петербург'),
    ('Sales', 'Казань');

INSERT INTO employees (first_name, last_name, email, hire_date, salary, department_id)
VALUES 
    ('Иван', 'Иванов', 'ivan@company.com', '2024-01-15', 100000.00, 1),
    ('Петр', 'Петров', 'petr@company.com', '2024-02-01', 95000.00, 1),
    ('Анна', 'Сидорова', 'anna@company.com', '2024-01-20', 110000.00, 2);

-- Пример 3: Базовые SELECT запросы
-- Выбор всех данных
SELECT * FROM employees;

-- Выбор конкретных колонок
SELECT first_name, last_name, salary 
FROM employees;

-- Фильтрация данных
SELECT * FROM employees 
WHERE salary > 100000;

-- Сортировка
SELECT * FROM employees 
ORDER BY salary DESC;

-- Пример 4: JOIN запросы
-- INNER JOIN
SELECT e.first_name, e.last_name, d.department_name
FROM employees e
INNER JOIN departments d ON e.department_id = d.department_id;

-- LEFT JOIN
SELECT e.first_name, e.last_name, d.department_name
FROM employees e
LEFT JOIN departments d ON e.department_id = d.department_id;

-- Пример 5: Агрегатные функции
-- COUNT
SELECT COUNT(*) as total_employees 
FROM employees;

-- AVG, MAX, MIN
SELECT 
    AVG(salary) as avg_salary,
    MAX(salary) as max_salary,
    MIN(salary) as min_salary
FROM employees;

-- GROUP BY
SELECT 
    department_id,
    COUNT(*) as employee_count,
    AVG(salary) as avg_salary
FROM employees
GROUP BY department_id;

-- Пример 6: Подзапросы
-- В WHERE
SELECT * FROM employees
WHERE salary > (SELECT AVG(salary) FROM employees);

-- В FROM
SELECT dept_name, avg_salary
FROM (
    SELECT 
        d.department_name as dept_name,
        AVG(e.salary) as avg_salary
    FROM employees e
    JOIN departments d ON e.department_id = d.department_id
    GROUP BY d.department_name
) as dept_stats
WHERE avg_salary > 100000;

-- Пример 7: Обновление данных
-- UPDATE
UPDATE employees
SET salary = salary * 1.1
WHERE department_id = 1;

-- DELETE
DELETE FROM employees
WHERE hire_date < '2024-01-01';

-- Пример 8: Создание индексов
-- Простой индекс
CREATE INDEX idx_employee_email ON employees(email);

-- Составной индекс
CREATE INDEX idx_employee_name ON employees(last_name, first_name);

-- Пример 9: Ограничения
-- NOT NULL
ALTER TABLE employees
ALTER COLUMN email SET NOT NULL;

-- UNIQUE
ALTER TABLE employees
ADD CONSTRAINT unique_email UNIQUE (email);

-- CHECK
ALTER TABLE employees
ADD CONSTRAINT positive_salary CHECK (salary > 0);

-- Пример 10: Представления
CREATE VIEW employee_details AS
SELECT 
    e.employee_id,
    e.first_name,
    e.last_name,
    e.email,
    d.department_name,
    e.salary
FROM employees e
JOIN departments d ON e.department_id = d.department_id; 