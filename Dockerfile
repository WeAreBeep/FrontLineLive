FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# For compile Less style files in pre-build stage
RUN apt-get update \
	&& curl -sL https://deb.nodesource.com/setup_14.x | bash \
	&& apt-get install -y nodejs

WORKDIR /src

COPY ["Web/Web.csproj", "Web/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Web/Web.csproj"
COPY ["Web/package.json", "Web/"]
COPY ["Web/package-lock.json", "Web/"]
RUN cd Web \
	&& npm install
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]