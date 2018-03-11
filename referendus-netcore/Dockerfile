FROM microsoft/aspnetcore:2.0 AS base

ENV NODE_VERSION 9.5.0
ENV ASPNETCORE_ENVIRONMENT Production

# set up node
RUN buildDeps='xz-utils' \
    && set -x \
    && apt-get update && apt-get install -y $buildDeps --allow-unauthenticated --no-install-recommends \
    && rm -rf /var/lib/apt/lists/* \
    && curl -SLO "https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.xz" \
    && tar -xJf "node-v$NODE_VERSION-linux-x64.tar.xz" -C /usr/local --strip-components=1 \
    && rm "node-v$NODE_VERSION-linux-x64.tar.xz" \
    && apt-get purge -y --auto-remove $buildDeps \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY referendus-netcore.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build referendus-netcore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish referendus-netcore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet referendus-netcore.dll
