# Genera los scripts SQL de las bases de GestorCMB para el instalador
# (Advanced Installer > SQL Databases).
#
# Política de datos para una instalación limpia:
#   - GestorCMB        : solo esquema (sin clientes/materiales/proyectos)
#   - GestorCMB_Users  : roles, permisos y parámetros completos + SOLO el usuario admin
#   - CMB_USERS        : solo esquema (legacy, no se usa en runtime)
#   - GestorCMB_Logs   : solo esquema
#
# Uso: powershell -ExecutionPolicy Bypass -File .\generate-db-scripts.ps1

$ErrorActionPreference = 'Stop'

[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.Smo')
[void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.ConnectionInfo')

$serverName  = '.\SQLEXPRESS'
$adminUserId = 'C2C73905-2849-49D9-83C1-76913DC1A175'  # RolesService.AdminUserId

# Las 5 familias oficiales hardcodeadas por GUID en RolesService (las de prueba no viajan)
$rolesOficiales = "'b1a2f3e4-c5d6-4a7b-8e9f-100000000001'," +  # Administrador
                  "'b1a2f3e4-c5d6-4a7b-8e9f-100000000002'," +  # Jefe de obra
                  "'b1a2f3e4-c5d6-4a7b-8e9f-100000000003'," +  # Operario
                  "'b1a2f3e4-c5d6-4a7b-8e9f-100000000004'," +  # Comprador
                  "'b1a2f3e4-c5d6-4a7b-8e9f-100000000005'"     # Gerente

$outDir      = Join-Path $PSScriptRoot 'Scripts'
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

# DataTables: tablas cuyo contenido viaja completo (en orden de inserción, padres primero).
# FilteredInserts: tablas con filtro WHERE; NullColumns fuerza NULL (ej. otp pendiente).
$databases = [ordered]@{
    'GestorCMB' = @{ Orden = '01' }
    'GestorCMB_Users' = @{
        Orden      = '02'
        DataTables = @('Acceso', 'Parametros')
        FilteredInserts = @(
            @{ Table = 'Familia';         Where = "idFamilia IN ($rolesOficiales)" }
            @{ Table = 'Familia_Familia'; Where = "idFamilia IN ($rolesOficiales) AND idFamiliaHijo IN ($rolesOficiales)" }
            @{ Table = 'Familia_Acceso';  Where = "idFamilia IN ($rolesOficiales)" }
            @{ Table = 'Usuario';         Where = "idUsuario = '$adminUserId'"; NullColumns = @('otp', 'otpExpiry') }
            @{ Table = 'Usuario_Familia'; Where = "idUsuario = '$adminUserId' AND idFamilia IN ($rolesOficiales)" }
            @{ Table = 'Usuario_Acceso';  Where = "idUsuario = '$adminUserId'" }
        )
    }
    'CMB_USERS'      = @{ Orden = '03' }
    'GestorCMB_Logs' = @{ Orden = '04' }
}

function Format-SqlLiteral($value) {
    if ($value -eq $null -or $value -is [System.DBNull]) { return 'NULL' }
    switch ($value.GetType().Name) {
        'String'   { return "N'" + $value.Replace("'", "''") + "'" }
        'Guid'     { return "N'" + $value.ToString().ToUpper() + "'" }
        'Boolean'  { if ($value) { return '1' } else { return '0' } }
        'DateTime' { return "'" + $value.ToString('yyyy-MM-ddTHH:mm:ss.fffffff') + "'" }
        'Decimal'  { return $value.ToString([System.Globalization.CultureInfo]::InvariantCulture) }
        'Double'   { return $value.ToString('R', [System.Globalization.CultureInfo]::InvariantCulture) }
        'Byte[]'   { return '0x' + (($value | ForEach-Object { $_.ToString('X2') }) -join '') }
        default    { return $value.ToString() }
    }
}

function Get-PkColumns([System.Data.SqlClient.SqlConnection]$conn, [string]$table) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
SELECT kcu.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
  ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME AND kcu.TABLE_NAME = tc.TABLE_NAME
WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.TABLE_NAME = @t
ORDER BY kcu.ORDINAL_POSITION
"@
    [void]$cmd.Parameters.AddWithValue('@t', $table)
    $dt = New-Object System.Data.DataTable
    (New-Object System.Data.SqlClient.SqlDataAdapter $cmd).Fill($dt) | Out-Null
    return @($dt.Rows | ForEach-Object { $_.COLUMN_NAME })
}

# Genera INSERTs idempotentes: cada fila se inserta solo si su PK no existe ya en la tabla.
# Así el script puede correr sobre una base existente (upgrade/reinstalación) sin duplicar ni fallar.
function Get-FilteredInserts([string]$connStr, [string]$table, [string]$where, [string[]]$nullColumns) {
    $conn = New-Object System.Data.SqlClient.SqlConnection $connStr
    $conn.Open()
    try {
        $pkCols = Get-PkColumns $conn $table
        if ($pkCols.Count -eq 0) { Write-Warning "  [$table] sin PK: los INSERT no llevan guarda IF NOT EXISTS." }

        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT * FROM [dbo].[$table] WHERE $where"
        $dt = New-Object System.Data.DataTable
        (New-Object System.Data.SqlClient.SqlDataAdapter $cmd).Fill($dt) | Out-Null

        $inserts = @()
        foreach ($row in $dt.Rows) {
            $cols = @(); $vals = @(); $pkConds = @()
            foreach ($col in $dt.Columns) {
                $cols += "[$($col.ColumnName)]"
                if ($nullColumns -contains $col.ColumnName) { $vals += 'NULL' }
                else { $vals += Format-SqlLiteral $row[$col] }
                if ($pkCols -contains $col.ColumnName) {
                    $pkConds += "[$($col.ColumnName)] = $(Format-SqlLiteral $row[$col])"
                }
            }
            $insert = "INSERT [dbo].[$table] ($($cols -join ', ')) VALUES ($($vals -join ', '))"
            if ($pkConds.Count -gt 0) {
                $insert = "IF NOT EXISTS (SELECT 1 FROM [dbo].[$table] WHERE $($pkConds -join ' AND ')) $insert"
            }
            $inserts += $insert
        }
        return $inserts
    } finally { $conn.Close() }
}

$server = New-Object Microsoft.SqlServer.Management.Smo.Server $serverName
$server.ConnectionContext.Connect()

foreach ($dbName in $databases.Keys) {
    $info = $databases[$dbName]
    $db = $server.Databases[$dbName]
    if (-not $db) { Write-Warning "La base '$dbName' no existe en $serverName, se omite."; continue }

    Write-Host "Generando script de $dbName..."

    $scripter = New-Object Microsoft.SqlServer.Management.Smo.Scripter $server
    $opts = $scripter.Options
    $opts.ScriptSchema     = $true
    $opts.ScriptData       = $false
    $opts.DriAll           = $true   # PKs, FKs (como ALTER al final), defaults, checks
    $opts.Indexes          = $true
    $opts.Triggers         = $true
    $opts.WithDependencies = $true   # tablas padre antes que hijas
    $opts.NoCollation      = $true   # no atar el script a la collation de esta PC
    $opts.IncludeIfNotExists = $true # idempotente: no falla si la tabla/índice/FK ya existe (upgrade)

    $sb = New-Object System.Text.StringBuilder
    [void]$sb.AppendLine("-- Script generado para el instalador de GestorCMB")
    [void]$sb.AppendLine("-- Base: $dbName")
    [void]$sb.AppendLine("IF DB_ID(N'$dbName') IS NULL CREATE DATABASE [$dbName];")
    [void]$sb.AppendLine("GO")
    [void]$sb.AppendLine("USE [$dbName];")
    [void]$sb.AppendLine("GO")

    # 1) Esquema de todas las tablas
    $tables = @($db.Tables | Where-Object { -not $_.IsSystemObject })
    if ($tables.Count -gt 0) {
        foreach ($line in $scripter.EnumScript($tables)) {
            [void]$sb.AppendLine($line)
            [void]$sb.AppendLine("GO")
        }
    }

    # 2) Vistas, procedimientos y funciones
    $opts.WithDependencies = $false
    foreach ($collection in @($db.Views, $db.StoredProcedures, $db.UserDefinedFunctions)) {
        $objs = @($collection | Where-Object { -not $_.IsSystemObject })
        if ($objs.Count -gt 0) {
            foreach ($line in $scripter.EnumScript($objs)) {
                [void]$sb.AppendLine($line)
                [void]$sb.AppendLine("GO")
            }
        }
    }

    # 3) Datos completos de las tablas maestras (en el orden declarado), con guarda por PK
    $connStr = "Data Source=$serverName;Initial Catalog=$dbName;Integrated Security=True;TrustServerCertificate=True"
    if ($info.DataTables) {
        foreach ($tableName in $info.DataTables) {
            $inserts = Get-FilteredInserts $connStr $tableName '1=1' @()
            if ($inserts.Count -eq 0) { Write-Warning "  Sin filas para [$tableName]" }
            foreach ($ins in $inserts) {
                [void]$sb.AppendLine($ins)
                [void]$sb.AppendLine("GO")
            }
        }
    }

    # 4) Filas filtradas (solo admin), con guarda por PK
    if ($info.FilteredInserts) {
        foreach ($fi in $info.FilteredInserts) {
            $inserts = Get-FilteredInserts $connStr $fi.Table $fi.Where $fi.NullColumns
            if ($inserts.Count -eq 0) { Write-Warning "  Sin filas para [$($fi.Table)] con filtro '$($fi.Where)'" }
            foreach ($ins in $inserts) {
                [void]$sb.AppendLine($ins)
                [void]$sb.AppendLine("GO")
            }
        }
    }

    $outFile = Join-Path $outDir "$($info.Orden)_$dbName.sql"
    [System.IO.File]::WriteAllText($outFile, $sb.ToString(), (New-Object System.Text.UTF8Encoding $true))
    Write-Host "  -> $outFile"
}

$server.ConnectionContext.Disconnect()
Write-Host "Listo."
