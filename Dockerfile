FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
#ENV ASPNETCORE_ENVIRONMENT=Production
#ENV ASPNETCORE_URLS http://*:5000
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /src
COPY DemoBlog.sln ./
COPY DemoBlog/DemoBlog.csproj DemoBlog/
COPY DAL/DAL.csproj DAL/
RUN dotnet restore
COPY . .
WORKDIR /src/DemoBlog
RUN dotnet build -c Release -o /app

FROM builder AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DemoBlog.dll"]

#CMD ASPNETCORE_URLS=http://*:$PORT dotnet DemoBlog.dll

#docker build -t demoblog .
#docker run -p 5000:80 demoblog