# EnchantedLand — техническая документация

Документация Unity-проекта **EnchantedLand DND**.

- [Архитектура проекта](Documentation/ARCHITECTURE.md)
- [Установка и запуск](Documentation/SETUP.md)
- [Сетевая архитектура](Documentation/NETWORK_ARCHITECTURE.md)
- [Zenject / Extenject и DI](Documentation/DI_ZENJECT.md)


## Технологии

- Unity Editor: **6000.5.7f1** (по ProjectSettings/ProjectVersion.txt).
- C# и Zenject для внедрения зависимостей.
- UniTask для асинхронных операций.
- Netcode for GameObjects и Unity Multiplayer Services для сетевых сессий.
- Addressables, URP, TextMesh Pro и uGUI.

## Основные подсистемы

- Общие системы: Assets/Common.
- Игровые конфигурации и данные игрока: Assets/EnchantedLand/Common.
- Сцены загрузки, лобби, одиночной и сетевой игры: Assets/EnchantedLand.

Документы описывают поведение, подтверждённое текущим кодом. Некоторые сетевые и игровые методы ещё не реализованы полностью; ограничения перечислены в архитектурном документе.
