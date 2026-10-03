FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR "/src"
COPY ["Api/DockerCourseWebApi/DockerCourseWebApi.csproj", "./"]
RUN dotnet restore "DockerCourseWebApi.csproj"
COPY . .
RUN dotnet publish "Api/DockerCourseWebApi/DockerCourseWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443 
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DockerCourseWebApi.dll"]
