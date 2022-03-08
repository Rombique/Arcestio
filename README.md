
# Arcestio
[![Build status](https://ci.appveyor.com/api/projects/status/y24x9vcbxu8mya03?svg=true)](https://ci.appveyor.com/project/Rombique/arcestio)

**Arcestio** is a tool that can help you migrate SQL scripts to your database.

## Features

 - Migrating plain SQL
 - Validating already migrated scripts
 - Database migration history
 - Supports PostgreSQL and MSSQL providers

## Roadmap

 - Add supports for MySQL, Oracle and SQLite
 - Add supports for absolute path to scripts folder
 - Add supports fore repeatable migrations

## How to use

 - Open folder with tool
 - Create folder (ex: SQL)
 - Open folder from previous step and create subfolders (ex: Tables, Data, Patches)
 - Put your migrations to this folders. Migrations have to named like ```V001_DescriptionOrName.sql``` or ```001_Desc_Or_Name.sql```
 - Open bash/powershell/cmd and start tool like:
 
 ```powershell
 Arcestio.exe -d "postgresql" -p "SQL" -f "Tables,Data,Patches" -c "Server=localhost; Database=postgresql; User Id=userid; Password=password;"
```
Where:
```-d``` is database provider. ```"postgresql"``` for PostgreSQL database. ```mssql``` for MSSQL database.
```-p``` is folder name from step two. Default value is 'SQL'.
```-f``` is subfolder names from step three. Order is important. Firstly will executed scripts from folder **Tables**, then **Data** and in the end from **Patches**.
```-c``` is connection string.
## Typical structure of scripts folders

```
Tool Folder
│   Arcestio.exe
│   ...
└───SQL
│   │
│   └───Tables
│   │   │   001_AddUsersTable.sql
│   │   │   002_AddToDoTable.sql
│   │   │   ...
|   └───Data
│   │   │   001_AddDataToUsersTable.sql
│   │   │   002_AddDataToToDoTable.sql
│   │   │   ...
|   └───Patches
│   │   │
│   │   └───V1
│   │   │   |   001_PatchUsersTable.sql
│   │   │   |   ...
|   |   └───V2
|   |   |   |   001_PatchUsersTable.sql
│   │   │   |   ...
```
