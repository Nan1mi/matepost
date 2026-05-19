FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MatePost/MatePost.csproj", "MatePost/"]
RUN dotnet restore "MatePost/MatePost.csproj"
COPY MatePost/ MatePost/
RUN dotnet publish "MatePost/MatePost.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
COPY matepost-prototype.html ./wwwroot/index.html
COPY images/ ./wwwroot/images/
ENTRYPOINT ["dotnet", "MatePost.dll"]
