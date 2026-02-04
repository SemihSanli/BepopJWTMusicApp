# Stage 1: Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution ve proje dosyalarını kopyala
COPY BepopJWT.sln .
COPY BepopJWT.API/BepopJWT.API.csproj BepopJWT.API/
COPY BepopJWT.BusinessLayer/BepopJWT.BusinessLayer.csproj BepopJWT.BusinessLayer/
COPY BepopJWT.DataAccessLayer/BepopJWT.DataAccessLayer.csproj BepopJWT.DataAccessLayer/
COPY BepopJWT.DTOLayer/BepopJWT.DTOLayer.csproj BepopJWT.DTOLayer/
COPY BepopJWT.EntityLayer/BepopJWT.EntityLayer.csproj BepopJWT.EntityLayer/
COPY BepopJWT.Consume/BepopJWT.Consume.csproj BepopJWT.Consume/

# NuGet paketlerini restore et
RUN dotnet restore

# Tüm kaynak kodları kopyala
COPY . .

# API projesini yayınla
RUN dotnet publish BepopJWT.API/BepopJWT.API.csproj -c Release -o /app/publish

# Stage 2: Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Build aşamasından yayınlanan dosyaları kopyala
COPY --from=build /app/publish .

# Container'ın dinleyeceği port
EXPOSE 8080

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "BepopJWT.API.dll"]