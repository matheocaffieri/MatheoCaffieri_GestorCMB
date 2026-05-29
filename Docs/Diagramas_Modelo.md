# Diagramas de Modelo — Entidad-Relación y Clases (actualizados)

Este archivo actualiza los diagramas de **Entidad-Relación (ER)** y de **Clases** del documento original, incorporando las entidades que faltaban respecto del código real del sistema:

- **Seguridad / RBAC:** `Usuario`, `Familia` (Rol), `Acceso` (Patente) y sus tablas puente.
- **Configuración:** `Parametros` (márgenes y utilidad de la empresa).
- **Auditoría:** `Log` (bitácora de eventos).
- **Negocio:** `MaterialFaltante`, `InformeDeCompra`, `DetalleInformeMaterialFaltante` e `InformeMonto`, que no estaban (o estaban incompletos) en los diagramas originales.

> **Imágenes ya renderizadas:** los dos diagramas ER están exportados como SVG en `Docs/diagramas/`:
> - `ER_Negocio.svg`
> - `ER_Seguridad.svg`
>
> Se pueden insertar directamente en el Word (Insertar → Imágenes). El bloque Mermaid de cada diagrama se incluye también como fuente editable por si querés regenerarlos o modificarlos.

---

## 1. Arquitectura de persistencia (3 bases de datos)

El sistema **no usa una única base**, sino tres bases SQL Server separadas. Esto es relevante para los diagramas: no hay claves foráneas *físicas* entre bases; las relaciones que cruzan bases (ej. `Parametros.ModificadoPor → Usuario`) son **referencias lógicas**.

| Base de datos | Contenido | Acceso |
|---|---|---|
| **GestorCMB** | Datos del negocio (clientes, proyectos, materiales, empleados, informes) | Entity Framework 6 (EDMX, Database-First) |
| **GestorCMB_Users** | Seguridad/RBAC (usuarios, familias, accesos) y parámetros | ADO.NET directo (repositorios manuales) |
| **GestorCMB_Logs** | Bitácora de eventos (`Log`) | ADO.NET directo |

---

## 2. Diagrama Entidad-Relación — Negocio (base `GestorCMB`)

Imagen renderizada: **`Docs/diagramas/ER_Negocio.svg`**

```mermaid
erDiagram
    CLIENTE ||--o{ PROYECTO : "tiene"
    PROYECTO ||--o{ INFORME_DE_COMPRA : "genera"
    PROYECTO ||--o{ INFORME_MONTO : "registra"
    PROYECTO ||--o{ DETALLE_PROYECTO_EMPLEADO : "asigna"
    PROYECTO ||--o{ DETALLE_PROYECTO_MATERIAL : "consume"
    PROYECTO ||--o{ MATERIAL_FALTANTE : "detecta"
    EMPLEADO ||--o{ DETALLE_PROYECTO_EMPLEADO : "participa en"
    PROVEEDOR ||--o{ MATERIAL : "provee"
    MATERIAL ||--o{ DETALLE_PROYECTO_MATERIAL : "figura en"
    MATERIAL ||--o| INVENTARIO : "tiene stock"
    INFORME_DE_COMPRA ||--o{ DETALLE_INFORME_MATERIAL_FALTANTE : "incluye"
    MATERIAL_FALTANTE ||--o{ DETALLE_INFORME_MATERIAL_FALTANTE : "referenciado por"

    CLIENTE {
        Guid IdCliente PK
        string NombreContacto
        string RazonSocial
        string Mail
        int Telefono
        bool IsActive
    }
    PROYECTO {
        Guid IdProyecto PK
        Guid IdCliente FK
        string Descripcion
        EnumEstado Estado
        DateTime FechaInicio
        DateTime FechaFin
        string Ubicacion
    }
    EMPLEADO {
        Guid IdEmpleado PK
        string Nombre
        string Apellido
        int NroDocumento
        float Sueldo
        int CantidadProyectosActivos
        bool IsActive
    }
    PROVEEDOR {
        Guid IdProveedor PK
        string Descripcion
        int Telefono
        bool IsActive
    }
    MATERIAL {
        Guid IdMaterial PK
        string DescripcionArticulo
        string TipoMaterial
        string TipoUnidad
        float CostoPorUnidad
        Guid IdProveedor FK
    }
    INVENTARIO {
        Guid IdMaterialInventario PK
        int Cantidad
        Guid IdMaterial FK
    }
    DETALLE_PROYECTO_EMPLEADO {
        Guid IdDetalleProyectoEmpleado PK
        Guid IdProyecto FK
        Guid IdEmpleado FK
        DateTime FechaIngresoEmpleado
        float ValorGanancia
    }
    DETALLE_PROYECTO_MATERIAL {
        Guid IdDetalleMaterial PK
        Guid IdProyecto FK
        Guid IdMaterial FK
        int Cantidad
        float ValorGanancia
        DateTime FechaIngresoMaterial
    }
    MATERIAL_FALTANTE {
        Guid IdMaterialFaltante PK
        Guid IdProyecto FK
        string DescripcionArticuloFaltante
        string TipoMaterialFaltante
        string TipoUnidadMaterialFaltante
        int CantidadFaltante
    }
    INFORME_DE_COMPRA {
        Guid IdInformeCompra PK
        Guid IdProyecto FK
        DateTime FechaRealizacion
        string Estado
    }
    DETALLE_INFORME_MATERIAL_FALTANTE {
        Guid IdDetalleMaterialFaltante PK
        Guid IdInformeCompra FK
        Guid IdMaterialFaltante FK
    }
    INFORME_MONTO {
        Guid IdInformeMonto PK
        Guid IdProyecto FK
        float TotalMateriales
        float TotalEmpleados
        float MontoTotal
    }
```

**Notas de este diagrama:**

- **`MATERIAL_FALTANTE` no tiene FK hacia `MATERIAL`.** Guarda la descripción, tipo y unidad como **texto** (`DescripcionArticuloFaltante`, etc.). Al confirmar una compra, el sistema busca el material en inventario **por coincidencia de texto** (descripción + tipo + unidad, sin distinguir mayúsculas ni espacios), no por clave foránea. Es una decisión de diseño: un faltante puede describir algo que todavía no existe como `Material` cargado.
- **`INFORME_DE_COMPRA.Estado`** es un `string` con tres valores: `pendiente` / `finalizado` / `cancelado` (soft-delete por estado).
- **`INVENTARIO`** mantiene el stock por material (relación 1 a 0..1 con `MATERIAL`).
- Las entidades con baja lógica (`IsActive`) **no se borran físicamente**: `Cliente`, `Empleado`, `Proveedor`.

---

## 3. Diagrama Entidad-Relación — Seguridad y Bitácora (bases `GestorCMB_Users` + `GestorCMB_Logs`)

Imagen renderizada: **`Docs/diagramas/ER_Seguridad.svg`**

Este es el modelo **Familia / Patente** (Rol / Acceso): un usuario obtiene permisos por dos vías combinables — **accesos directos** y **familias (roles)** —, y las familias pueden contener otras familias (jerarquía → patrón *Composite*).

```mermaid
erDiagram
    USUARIO ||--o{ USUARIO_FAMILIA : "tiene"
    FAMILIA ||--o{ USUARIO_FAMILIA : "agrupa"
    USUARIO ||--o{ USUARIO_ACCESO : "tiene directo"
    ACCESO ||--o{ USUARIO_ACCESO : "asignado a"
    FAMILIA ||--o{ FAMILIA_ACCESO : "contiene"
    ACCESO ||--o{ FAMILIA_ACCESO : "incluido en"
    FAMILIA ||--o{ FAMILIA_FAMILIA : "padre"
    FAMILIA ||--o{ FAMILIA_FAMILIA : "hijo"
    USUARIO ||--o{ PARAMETROS : "modifica"

    USUARIO {
        Guid IdUsuario PK
        string Mail
        string Contraseña "hash BCrypt"
        int Telefono
        string Idioma
        bool IsActive
        string Otp "nullable"
        DateTime OtpExpiry "nullable"
    }
    FAMILIA {
        Guid idFamilia PK
        string nombre "Rol"
    }
    ACCESO {
        Guid idAcceso PK
        string nombre "Patente"
        int dataKey "TipoPermiso"
    }
    USUARIO_FAMILIA {
        Guid idUsuario FK
        Guid idFamilia FK
    }
    USUARIO_ACCESO {
        Guid idUsuario FK
        Guid idAcceso FK
    }
    FAMILIA_ACCESO {
        Guid idFamilia FK
        Guid idAcceso FK
    }
    FAMILIA_FAMILIA {
        Guid idFamilia FK
        Guid idFamiliaHijo FK
    }
    PARAMETROS {
        Guid IdParametro PK
        decimal MargenEmpleados
        decimal MargenMateriales
        decimal UtilidadEmpresa
        DateTime UltimaModificacion
        Guid ModificadoPor FK "nullable"
    }
    LOG {
        DateTime Fecha
        string Nivel
        string Mensaje
        string Excepcion "nullable"
    }
```

**Notas de este diagrama:**

- **`FAMILIA` = Rol, `ACCESO` = Patente.** Un `Acceso` mapea 1 a 1 con un valor del enum `TipoPermiso` (campo `dataKey`); hay 15 patentes posibles.
- **`FAMILIA_FAMILIA`** es la relación recursiva que habilita el patrón *Composite*: una familia puede contener familias hijas. Permite roles compuestos (ej. "Gerente" que incluye los permisos de "Jefe de Obra" más otros).
- **Doble vía de permisos:** `USUARIO_ACCESO` (permisos directos del usuario) + `USUARIO_FAMILIA` (permisos heredados de sus roles). El permiso efectivo es la **unión** de ambas.
- **`PARAMETROS`** vive en `GestorCMB_Users`. Su campo `ModificadoPor` referencia *lógicamente* a `Usuario` (no es FK física porque, aunque comparten base aquí, el acceso es por ADO.NET sin constraints declaradas).
- **`LOG`** vive en una base aparte (`GestorCMB_Logs`) y **no tiene FK** hacia ninguna entidad: es una bitácora plana de eventos (`Fecha`, `Nivel`, `Mensaje`, `Excepcion`).

---

## 4. Diagrama de Clases — Modelo de dominio (base `GestorCMB`)

Imagen renderizada: **`Docs/diagramas/Clases_Dominio.svg`**

Las entidades del dominio son objetos de datos (propiedades + propiedades de navegación de EF). Los atributos completos están en el ER de la sección 2; acá se resaltan las **relaciones de navegación** entre clases.

```mermaid
classDiagram
    class Cliente {
        +Guid IdCliente
        +string RazonSocial
        +bool IsActive
        +List~Proyecto~ Proyectos
    }
    class Proyecto {
        +Guid IdProyecto
        +Guid IdCliente
        +string Descripcion
        +EnumEstado Estado
        +Cliente Cliente
    }
    class Empleado {
        +Guid IdEmpleado
        +float Sueldo
        +bool IsActive
    }
    class Proveedor {
        +Guid IdProveedor
        +string Descripcion
        +bool IsActive
    }
    class Material {
        +Guid IdMaterial
        +string DescripcionArticulo
        +float CostoPorUnidad
        +Proveedor Proveedor
    }
    class Inventario {
        +Guid IdMaterialInventario
        +int Cantidad
        +Material Material
    }
    class DetalleProyectoEmpleado {
        +DateTime FechaIngresoEmpleado
        +float ValorGanancia
    }
    class DetalleProyectoMaterial {
        +int Cantidad
        +float ValorGanancia
        +DateTime FechaIngresoMaterial
    }
    class MaterialFaltante {
        +Guid IdMaterialFaltante
        +string DescripcionArticuloFaltante
        +int CantidadFaltante
    }
    class InformeDeCompra {
        +Guid IdInformeCompra
        +DateTime FechaRealizacion
        +string Estado
    }
    class DetalleInformeMaterialFaltante {
        +Guid IdInformeCompra
        +Guid IdMaterialFaltante
    }
    class InformeMonto {
        +float TotalMateriales
        +float TotalEmpleados
        +float MontoTotal
    }

    Cliente "1" --> "0..*" Proyecto
    Proveedor "1" --> "0..*" Material
    Material "1" --> "0..1" Inventario
    Proyecto "1" --> "0..*" InformeDeCompra
    Proyecto "1" --> "0..*" InformeMonto
    Proyecto "1" --> "0..*" MaterialFaltante
    Proyecto "1" --> "0..*" DetalleProyectoEmpleado
    Proyecto "1" --> "0..*" DetalleProyectoMaterial
    Empleado "1" --> "0..*" DetalleProyectoEmpleado
    Material "1" --> "0..*" DetalleProyectoMaterial
    InformeDeCompra "1" --> "0..*" DetalleInformeMaterialFaltante
    MaterialFaltante "1" --> "0..*" DetalleInformeMaterialFaltante
```

---

## 5. Diagrama de Clases — Patrón Composite del RBAC (seguridad)

Imagen renderizada: **`Docs/diagramas/Clases_Composite_RBAC.svg`**

Este es el aporte de diseño más relevante para la defensa: el control de permisos está resuelto con el **patrón Composite**. Tanto un permiso individual (`Acceso`, *hoja*) como un grupo de permisos (`RolCompuesto`, *compuesto*) implementan la misma interfaz `IPermiso`, de modo que el `Usuario` consulta permisos de forma uniforme sin saber si está preguntándole a una hoja o a un compuesto.

```mermaid
classDiagram
    class IPermiso {
        <<interface>>
        +string Nombre
        +Guid Id
        +TienePermiso(TipoPermiso) bool
    }
    class Acceso {
        -string Nombre
        -Guid Id
        -TipoPermiso DataKey
        +TienePermiso(TipoPermiso) bool
    }
    class RolCompuesto {
        -string Nombre
        -Guid Id
        -List~IPermiso~ _hijos
        +AgregarHijo(IPermiso)
        +QuitarHijo(IPermiso)
        +TienePermiso(TipoPermiso) bool
        +ObtenerHijos() IReadOnlyCollection~IPermiso~
    }
    class Usuario {
        +Guid IdUsuario
        +string Mail
        +bool IsActive
        -List~IPermiso~ _permisos
        +AgregarPermiso(IPermiso)
        +TienePermiso(TipoPermiso) bool
    }
    class TipoPermiso {
        <<enumeration>>
        VER_PROYECTOS
        GESTIONAR_PROYECTOS
        VER_INVENTARIO
        GESTIONAR_MATERIALES
        VER_EMPLEADOS
        GESTIONAR_EMPLEADOS
        VER_CLIENTES
        GESTIONAR_CLIENTES
        VER_PROVEEDORES
        GESTIONAR_PROVEEDORES
        VER_INFORMES_COMPRA
        GESTIONAR_INFORMES_COMPRA
        VER_LOGS
        CONFIGURAR_PARAMETROS
        GESTIONAR_USUARIOS
    }

    IPermiso <|.. Acceso : implementa
    IPermiso <|.. RolCompuesto : implementa
    RolCompuesto o--> "0..*" IPermiso : _hijos
    Usuario o--> "0..*" IPermiso : _permisos
    Acceso ..> TipoPermiso : DataKey
```

**Cómo se lee el patrón:**

- **`IPermiso`** = *Component*. Define `TienePermiso(TipoPermiso)`.
- **`Acceso`** = *Leaf* (hoja). Devuelve `true` solo si su `DataKey` coincide con el permiso consultado.
- **`RolCompuesto`** = *Composite*. Mantiene una lista de hijos `IPermiso` (que pueden ser `Acceso` u otros `RolCompuesto`) y delega la consulta recursivamente: devuelve `true` si **algún** hijo tiene el permiso.
- **`Usuario`** consulta `TienePermiso(...)` sobre su colección de `IPermiso` sin distinguir hojas de compuestos → polimorfismo uniforme, núcleo del patrón.
- La estructura de árbol se persiste en las tablas `Familia`, `Acceso`, `Familia_Familia`, `Familia_Acceso`, `Usuario_Familia` y `Usuario_Acceso` (ver sección 3).

---

## 6. Observaciones de cobertura

| Entidad / concepto | ¿Estaba en el doc original? | Acción |
|---|---|---|
| `Usuario` | Incompleta / ausente | Agregada (con `Otp`/`OtpExpiry` para recuperación) |
| `Familia` (Rol) | Ausente | Agregada |
| `Acceso` (Patente) | Ausente | Agregada |
| Tablas puente RBAC | Ausentes | Agregadas (`Usuario_Familia`, `Usuario_Acceso`, `Familia_Acceso`, `Familia_Familia`) |
| `Parametros` | Ausente | Agregada |
| `Log` (bitácora) | Ausente | Agregada |
| `MaterialFaltante` | Ausente / incompleta | Agregada |
| `InformeDeCompra` + `Estado` | Incompleta | Completada (soft-delete por estado) |
| `DetalleInformeMaterialFaltante` | Ausente | Agregada |
| `InformeMonto` | Ausente / incompleta | Agregada |
| Patrón Composite (RBAC) | No diagramado | Diagrama de clases dedicado (sección 5) |

### Ubicación sugerida en el documento principal

- Las **secciones 2 y 3** (ER) reemplazan al diagrama Entidad-Relación original.
- La **sección 4** (clases de dominio) reemplaza/actualiza el diagrama de clases del modelo de negocio.
- La **sección 5** (Composite) se suma como diagrama de clases del subsistema de seguridad; conviene ubicarla junto a la explicación de patrones de diseño.

### Cómo regenerar las imágenes

Los bloques ```mermaid``` son la fuente editable. Para volver a generar los SVG/PNG: pegá el bloque en [mermaid.live](https://mermaid.live) y exportá, o pedímelo y los regenero con la herramienta de Mermaid. Los cuatro diagramas ya están renderizados en `Docs/diagramas/`:

| Sección | Archivo |
|---|---|
| 2. ER de Negocio | `ER_Negocio.svg` |
| 3. ER de Seguridad y Bitácora | `ER_Seguridad.svg` |
| 4. Clases de dominio | `Clases_Dominio.svg` |
| 5. Composite del RBAC | `Clases_Composite_RBAC.svg` |

> Nota: `ER_Negocio.svg` tiene un ajuste manual de posición de la caja `EMPLEADO` (se subió ~60px para despejar el cruce con `MATERIAL_FALTANTE`). Si regenerás ese diagrama desde la fuente Mermaid, el ajuste se pierde.
