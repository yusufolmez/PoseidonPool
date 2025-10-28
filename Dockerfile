# Faz 1: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution dosyasýný kopyala
COPY *.sln .

# Presentation projesinin .csproj dosyasýný kopyala (Düzeltildi)
COPY ["Presentation/PoseidonPool.API/PoseidonPool.API.csproj", "Presentation/PoseidonPool.API/"]

# Diðer projelerinizi de ekleyin (Core ve Infrastructure da dahil)
COPY ["Core/PoseidonPool.Application/PoseidonPool.Application.csproj", "Core/PoseidonPool.Application/"]
COPY ["Core/PoseidonPool.Domain/PoseidonPool.Domain.csproj", "Core/PoseidonPool.Domain/"]
COPY ["Infrastructure/PoseidonPool.Infrastructure/PoseidonPool.Infrastructure.csproj", "Infrastructure/PoseidonPool.Infrastructure/"]
COPY ["Infrastructure/PoseidonPool.Persistance/PoseidonPool.Persistance.csproj", "Infrastructure/PoseidonPool.Persistance/"]


# Tüm projeleri restore et
RUN dotnet restore

# Tüm kaynak dosyalarýný kopyala
COPY . .

# Çalýþma dizinini Presentation projesine ayarla (Düzeltildi)
WORKDIR /src/Presentation/PoseidonPool.API
RUN dotnet build "PoseidonPool.API.csproj" -c Release -o /app/build

# Faz 2: Publish (Yayýnlama)
FROM build AS publish
# Proje adýný kullan (Düzeltildi)
RUN dotnet publish "PoseidonPool.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Faz 3: Final (Çalýþtýrma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

# Uygulamayý baþlat (Düzeltildi)
ENTRYPOINT ["dotnet", "PoseidonPool.API.dll"]