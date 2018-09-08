FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/Migrations/ .
COPY --from=build-env /app/docker_entrypoint.sh .
EXPOSE 5000
RUN chmod +x docker_entrypoint.sh
CMD /bin/bash docker_entrypoint.sh