#!/bin/bash

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P(4)ssword" -p 1433:1433 --name sqlserver1 -d mcr.microsoft.com/mssql/server:2019-latest
