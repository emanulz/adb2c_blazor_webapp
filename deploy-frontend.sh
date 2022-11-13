#!/usr/bin/env bash

dotnet build -c Release
dotnet publish -c Release
vercel --prod bin/Release/net6.0/publish/wwwroot/