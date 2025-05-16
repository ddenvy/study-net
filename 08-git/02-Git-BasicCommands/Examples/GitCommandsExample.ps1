# Инициализация репозитория
Write-Host "Инициализация репозитория..."
git init

# Настройка конфигурации
Write-Host "`nНастройка конфигурации..."
git config --global user.name "Иван Иванов"
git config --global user.email "ivan@example.com"

# Создание первого коммита
Write-Host "`nСоздание первого коммита..."
"# Мой первый проект" | Out-File -FilePath "README.md" -Encoding utf8
git add README.md
git commit -m "Добавлен README.md"

# Работа с ветками
Write-Host "`nРабота с ветками..."
git branch feature/new-feature
git checkout feature/new-feature

# Внесение изменений в новой ветке
Write-Host "`nВнесение изменений в новой ветке..."
"Новая функциональность" | Out-File -FilePath "feature.txt" -Encoding utf8
git add feature.txt
git commit -m "Добавлена новая функциональность"

# Возврат на основную ветку и слияние
Write-Host "`nВозврат на основную ветку и слияние..."
git checkout main
git merge feature/new-feature

# Работа с удаленным репозиторием
Write-Host "`nРабота с удаленным репозиторием..."
git remote add origin https://github.com/username/repo.git
git push -u origin main

# Просмотр истории
Write-Host "`nПросмотр истории..."
git log --oneline

# Отмена изменений
Write-Host "`nОтмена изменений..."
"Новые изменения" | Out-File -FilePath "changes.txt" -Encoding utf8
git add changes.txt
git reset HEAD changes.txt
Remove-Item changes.txt

# Восстановление версии
Write-Host "`nВосстановление версии..."
$commitHash = git rev-parse HEAD
git checkout $commitHash

Write-Host "`nДемонстрация завершена!" 