FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Eagle.TestNode1/Eagle.TestNode1.csproj", "Eagle.TestNode1/"]
COPY ["Feature.Infrastructure/Feature.Infrastructure.csproj", "Feature.Infrastructure/"]
COPY ["Eagle/Eagle.csproj", "Eagle/"]
RUN dotnet restore "Eagle.TestNode1/Eagle.TestNode1.csproj"
COPY . .
WORKDIR "/src/Eagle.TestNode1"
RUN dotnet build "Eagle.TestNode1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Eagle.TestNode1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Eagle.TestNode1.dll"]