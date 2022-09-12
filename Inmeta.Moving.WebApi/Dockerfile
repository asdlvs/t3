#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Inmeta.Moving.WebApi/Inmeta.Moving.WebApi.csproj", "Inmeta.Moving.WebApi/"]
RUN dotnet restore "Inmeta.Moving.WebApi/Inmeta.Moving.WebApi.csproj"
COPY . .
WORKDIR "/src/Inmeta.Moving.WebApi"
RUN dotnet build "Inmeta.Moving.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inmeta.Moving.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inmeta.Moving.WebApi.dll"]