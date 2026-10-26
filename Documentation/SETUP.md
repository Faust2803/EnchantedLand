# Установка и запуск EnchantedLand

## Требования

- Git.
- Unity Hub.
- Unity Editor версии 6000.5.7f1, указанная в ProjectSettings/ProjectVersion.txt.
- JetBrains Rider или Visual Studio.
- Доступ к сети для восстановления пакетов Unity Package Manager и Git-зависимости UniTask.

## Получение проекта

    git clone https://github.com/Faust2803/EnchantedLand.git
    cd EnchantedLand

Добавьте папку проекта в Unity Hub и откройте её подходящей версией Unity.

## Восстановление зависимостей

Unity Package Manager использует Packages/manifest.json и Packages/packages-lock.json. При первом открытии дождитесь завершения импорта и разрешения пакетов. Среди зависимостей есть UniTask, Netcode for GameObjects, Unity Multiplayer Services, Addressables и URP.

Если пакет не устанавливается:
1. Проверьте интернет и доступ к GitHub.
2. Проверьте ошибки в Unity Console и Package Manager.
3. Убедитесь, что версия Editor соответствует версии проекта.
4. Не удаляйте packages-lock.json без необходимости: сначала попробуйте повторное разрешение пакетов штатными средствами Unity.

## Запуск

1. Откройте сцену загрузки Load.
2. Проверьте Build Settings: путь лобби в настройках сборки может не совпадать с фактическим путём файла сцены.
3. Проверьте настройки Zenject и ссылки на ScriptableObject-конфигурации.
4. Для сетевой игры настройте Unity Services и Authentication для используемого окружения.
5. Запустите два экземпляра приложения для проверки сетевого подключения.

## Локальные данные

Данные игрока сохраняются в Application.persistentDataPath/player_data.json. Для сброса локального прогресса закройте приложение и удалите этот файл в каталоге persistent data соответствующей платформы. Перед удалением сделайте резервную копию.

## Диагностика

- Ошибки отсутствующих пакетов: проверьте Package Manager и Git-зависимости.
- Сцена не загружается: проверьте Build Settings и точный путь файла сцены.
- DI-зависимость равна null: проверьте регистрацию в Installer и способ создания объекта.
- Игрок не появляется в сети: проверьте NetworkObject, регистрацию prefab и серверный порядок spawn.
- Не подключается клиент: проверьте Unity Services, Authentication, Relay и код подключения.
