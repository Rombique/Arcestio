

# Arcestio
[![Build status](https://ci.appveyor.com/api/projects/status/y24x9vcbxu8mya03?svg=true)](https://ci.appveyor.com/project/Rombique/arcestio)

**Arcestio** is a tool that can help you migrate SQL scripts to your database.

## Features

 - Migrating plain SQL
 - Supports repeatable migrations
 - Validating already migrated scripts
 - Database migration history
 - Supports PostgreSQL and MSSQL providers

## Roadmap

 - Add supports for MySQL, Oracle and SQLite

## How to use

 1. Open folder with tool
 2. Create folder (ex: SQL)
 3.  Open folder from previous step and create subfolders (ex: Tables, Data, Patches)
 4. Put your migrations to this folders. Migrations have to named like ```V001_DescriptionOrName.sql``` or ```001_Desc_Or_Name.sql```
    for common scripts and ```R_TypicalRepeatable.sql``` for repeatable migrations.
  5. Open bash/powershell/cmd and start tool like:

 
 ```powershell
 Arcestio.exe -d "postgresql" -p "SQL" -f "Tables,Data,Patches" -c "Server=localhost; Database=postgresql; User Id=userid; Password=password;"
```
Where:
```-d``` is database provider. ```"postgresql"``` for PostgreSQL database, ```"mssql"``` for MSSQL database.
```-p``` is folder name from step two. Default value is 'SQL'.
```-f``` is subfolder names from step three. **Order is important!** Firstly will executed scripts from folder **Tables**, then **Data** and in the end from **Patches**.
```-c``` is connection string.
## Typical structure of scripts folders

```
Root folder
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
│   │   │   |   R_RepeatablePatch.sql
│   │   │   |   ...
|   |   └───V2
|   |   |   |   001_PatchUsersTable.sql
│   │   │   |   ...
...
```
