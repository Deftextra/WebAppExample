# /bin/bash

if  [ -z $(test dotnet list --global | grep dotnet-ef) ];then
    dotnet tool install --global dotnet-ef 
else 
    dotnet tool update --global dotnet-ef
fi

dotnet ef migrations add Intial
dotnet ef database update
dotnet run INITDB=true
