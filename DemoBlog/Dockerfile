FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
#EXPOSE 5001

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DemoBlog/DemoBlog.csproj", "DemoBlog/"]
RUN dotnet restore "DemoBlog/DemoBlog.csproj"
COPY . .
WORKDIR "/src/DemoBlog"
RUN dotnet build "DemoBlog.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DemoBlog.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoBlog.dll"]