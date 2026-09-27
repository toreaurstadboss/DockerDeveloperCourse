
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["DockerCourseWebApi/DockerCourseWebApi.csproj", "DockerCourseWebApi/"]
RUN dotnet restore "DockerCourseWebApi/DockerCourseWebApi.csproj"
COPY . .
WORKDIR "/src/DockerCourseWebApi"
RUN dotnet publish "DockerCourseWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443 
COPY --from=build /app/publish .

