# Faz 1: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution dosyas�n� kopyala
COPY *.sln .

# Presentation projesinin .csproj dosyas�n� kopyala (D�zeltildi)
COPY ["Presentation/PoseidonPool.API/PoseidonPool.API.csproj", "Presentation/PoseidonPool.API/"]

# Di�er projelerinizi de ekleyin (Core ve Infrastructure da dahil)
COPY ["Core/PoseidonPool.Application/PoseidonPool.Application.csproj", "Core/PoseidonPool.Application/"]
COPY ["Core/PoseidonPool.Domain/PoseidonPool.Domain.csproj", "Core/PoseidonPool.Domain/"]
COPY ["Infrastructure/PoseidonPool.Infrastructure/PoseidonPool.Infrastructure.csproj", "Infrastructure/PoseidonPool.Infrastructure/"]
COPY ["Infrastructure/PoseidonPool.Persistance/PoseidonPool.Persistance.csproj", "Infrastructure/PoseidonPool.Persistance/"]


# T�m projeleri restore et
RUN dotnet restore

# T�m kaynak dosyalar�n� kopyala
COPY . .

# �al��ma dizinini Presentation projesine ayarla (D�zeltildi)
WORKDIR /src/Presentation/PoseidonPool.API
RUN dotnet build "PoseidonPool.API.csproj" -c Release -o /app/build

# Faz 2: Publish (Yay�nlama)
FROM build AS publish
# Proje ad�n� kullan (D�zeltildi)
RUN dotnet publish "PoseidonPool.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Faz 3: Final (�al��t�rma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

# Uygulamay� ba�lat (D�zeltildi)
ENTRYPOINT ["dotnet", "PoseidonPool.API.dll"]