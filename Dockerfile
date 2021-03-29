# Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 as builder
WORKDIR /app 
COPY . .
RUN dotnet build --configuration Release

# Run
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runner
WORKDIR /app 
COPY --from=builder /app/bin/Release/net5.0/ /app
ENTRYPOINT [ "dotnet", "PersonalCrm.dll" ]