# 1. Этап сборки (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["BookApi.csproj", "./"]
RUN dotnet restore "./BookApi.csproj"

# Копируем весь остальной код и собираем релиз
COPY . .
RUN dotnet publish "BookApi.csproj" -c Release -o /app/publish

# 2. Этап запуска (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render дает порт динамически через переменную среды, .NET должен ее слушать
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "BookApi.dll"]