FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Копируем все файлы текущей папки (собранного приложения) в контейнер
COPY . .

# Указываем порт
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Запуск приложения
ENTRYPOINT ["dotnet", "ForhouseW.dll"]