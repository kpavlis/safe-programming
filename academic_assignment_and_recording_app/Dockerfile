FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["academic_assignment_and_recording_app.csproj", "./"]
RUN dotnet restore "./academic_assignment_and_recording_app.csproj"

COPY . .
WORKDIR "/src/."

RUN dotnet publish "academic_assignment_and_recording_app.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080 

ENTRYPOINT ["dotnet", "academic_assignment_and_recording_app.dll"]