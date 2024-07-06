# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/User/User.API/User.API.csproj", "Services/User/User.API/"]
COPY ["BuildingBlocks/BuildingBlock/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlock/"]
RUN dotnet restore "./Services/User/User.API/./User.API.csproj"
COPY . .
WORKDIR "/src/Services/User/User.API"
RUN dotnet build "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=hehehe
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

# Copy published output from publish stage
COPY --from=publish /app/publish .

# Install CA certificates and update certificates store

#RUN chmod 777 /https/rootCA.cer
#RUN apt-get update
#RUN apt-get install -y --no-install-recommends ca-certificates
#RUN cp /https/rootCA.cer /usr/local/share/ca-certificates/rootCA.crt
#RUN ls /usr/local/share/ca-certificates/
#RUN update-ca-certificates -qq
ENTRYPOINT ["dotnet", "User.API.dll"]
