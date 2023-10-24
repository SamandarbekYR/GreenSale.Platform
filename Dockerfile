FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ./src ./
WORKDIR /src/GreenSale.WebApi

RUN dotnet restore
RUN dotnet publish -c Release -o test

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS serve
WORKDIR /app
COPY --from=build /src/GreenSale.WebApi/test .

EXPOSE 80
EXPOSE 443

ENTRYPOINT [ "dotnet", "GreenSale.WebApi.dll" ]

