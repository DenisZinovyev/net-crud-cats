#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["DSR.Cats.Server.WebApi/DSR.Cats.Server.WebApi.csproj", "DSR.Cats.Server.WebApi/"]
RUN dotnet restore "DSR.Cats.Server.WebApi/DSR.Cats.Server.WebApi.csproj"
COPY . .
WORKDIR "/src/DSR.Cats.Server.WebApi"
RUN dotnet build "DSR.Cats.Server.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DSR.Cats.Server.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DSR.Cats.Server.WebApi.dll"]