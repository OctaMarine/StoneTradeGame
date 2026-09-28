# Stone Trade Game

Web игра с экономической системой торговли камнями их крафтом и улучшениями.

Данный репозиторий содержит как клиентскую (Frontend), так и серверную (Backend) часть.

## Технологический стек

### Backend 
- **Фреймворк:** C# .NET 8.0, ASP.NET Core Web API
- **База данных:** PostgreSQL + Entity Framework Core 8 (Code First)
- **Безопасность:** JWT Bearer Authentication, BCrypt
- **Архитектура:** Layered Architecture (Web, BusinessLogic, DAL)

### Frontend 
- **Фреймворк:** React, TypeScript
- **Интеграция с API:** REST API (Axios) + **Vite Proxy**
- **Сборщик:** Vite

Серверная часть разделена на проекты для обеспечения слабой связности:
1. **`StoneActionServer.Web`** – Точка входа. Настройка DI, Middleware, контроллеры, конфигурация JWT.
2. **`StoneActionServer.BusinessLogic`** – Интерфейсы и реализации сервисов, валидация данных, игровые алгоритмы (расчет крафта, прокачка навыков).
3. **`StoneActionServer.DAL`** – Слой доступа к данным. Конфигурация EF Core, репозитории

### Требования
- Docker & Docker Compose

Для локального запуска:
- .NET 8 SDK
- Node.js >= 18 и npm
- PostgreSQL

### Как запустить локально

Бекенд
- Настройте подключение к базе данных в файле Backend\StoneActionServer.WebApi\appsettings.json
- (перейте в WebApi проект)cd StoneActionServer.WebApi 
- dotnet build
- dotnet ef migrations add InitialCreate --project ../StoneActionServer.DAL --startup-project .
- dotnet run --launch-profile http

Фронтенд
- npm install
- npm run dev