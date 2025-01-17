FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /usr/src/app
COPY . .
RUN dotnet restore
EXPOSE 8001
CMD ["dotnet", "run", "--urls=http://0.0.0.0:8001"]