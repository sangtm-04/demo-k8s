 
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MISA.Demo/Demo.WebApp.UI/Demo.WebApp.UI.csproj", "Demo.WebApp.UI/"]
COPY ["MISA.Demo/Demo.WebApp.DL/Demo.WebApp.DL.csproj", "Demo.WebApp.DL/"]
COPY ["MISA.Demo/Demo.WebApp.Entities/Demo.WebApp.Entities.csproj", "Demo.WebApp.Entities/"]
COPY ["MISA.Demo/Demo.WebApp.BL/Demo.WebApp.BL.csproj", "Demo.WebApp.BL/"]
COPY ["MISA.Demo/Demo.WebApp.Common/Demo.WebApp.Common.csproj", "Demo.WebApp.Common/"]
RUN dotnet restore "Demo.WebApp.UI/Demo.WebApp.UI.csproj"
COPY ./MISA.Demo .
WORKDIR "/src/Demo.WebApp.UI"
RUN dotnet build "Demo.WebApp.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.WebApp.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.WebApp.UI.dll"]