# Ejecución del proyecto

## Crear proyecto

1. Desde la carpeta de `back`

~~~sh
dotnet new webapi
~~~

Este comando crea:
- back.csproj
- regenera el Program.cs
- añade Swager

> [!note] 
> Si el comando pregunta "Overwrite Program.cs", decir que si y luego reajustar el fichero

## Arrancar desde VScode

1. Abrir el proyecto con VScode
2. Ir al fichero Program.c
3. Botón Run / f5

**Salida esperada**

El proyecto arranca bien
Abre el navegador
Te dice el puerto

## Arrancar desde Terminal

1. Desde la carpeta `back`: 

~~~
dotnet run
~~~

**Salida esperada**

~~~
Now listening on: https://localhost:7234
Now listening on: http://localhost:5234
~~~


## Instalar dependencias

1. Desde terminal ir al directorio `back`
2. Ejecutar estos comandos:

~~~sh
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0
dotnet add package Swashbuckle.AspNetCore
~~~

3. Reiniciar y construir proyecto de nuevo

~~~sh
dotnet restore
dotnet build
~~~

## Acceder a la aplicación vía navegador web

~~~sh
http://localhost:5067/
~~~

## Acceder al swagger vía navegador web (API)

~~~sh
http://localhost:5067/swagger
~~~