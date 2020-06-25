FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /work
COPY / /work/
RUN dotnet build --configuration Release
RUN cp -R /work/bin/Release/netcoreapp3.1/ /build
