# Casos de Uso — Informes de Compra (agregados)

Este archivo agrupa los casos de uso vinculados a los **Informes de Compra** que no se encuentran cubiertos en la documentación original, o que reflejan funcionalidad incorporada posteriormente (soft-delete por estado e historial persistido por snapshot).

Un Informe de Compra atraviesa tres estados:
- **pendiente:** recién generado, aguarda confirmación de compra.
- **finalizado:** la compra se aplicó al proyecto; los materiales se sumaron al detalle del proyecto y se guardó un snapshot.
- **cancelado:** el informe fue descartado sin aplicarse; los materiales faltantes del proyecto siguen disponibles para regenerar otro informe más adelante.

---

## CU – Generar informe de compra

**Actor Principal:** Usuario autenticado con permiso `GESTIONAR_INFORMES_COMPRA`.

**Descripción:** Desde el detalle de un proyecto que posee materiales faltantes, el usuario genera un informe de compra que agrupa todos los materiales faltantes pendientes del proyecto. El sistema reutiliza un informe ya existente para ese mismo día (si hay) en lugar de crear uno nuevo.

**Precondiciones:**
- El usuario está autenticado y posee el permiso `GESTIONAR_INFORMES_COMPRA`.
- El proyecto tiene al menos un material faltante registrado.

**Postcondiciones:**
- Existe un Informe de Compra en estado **pendiente** asociado al proyecto, con los materiales faltantes incorporados como detalle.
- El usuario es redirigido (o tiene acceso) a la pantalla de gestión de Informes de Compra.

**Flujo Principal:**
1. El usuario, posicionado en el *Detalle del Proyecto*, hace clic en el botón **"Generar informe de compra"**.
2. El sistema obtiene todos los materiales faltantes del proyecto.
3. El sistema verifica si ya existe un informe del proyecto con fecha del día actual.
4. Si existe, lo reutiliza; si no, crea un nuevo `InformeDeCompra` con estado **"pendiente"** y fecha del día.
5. Para cada material faltante, el sistema agrega un `DetalleInformeMaterialFaltante` (omitiendo los que ya estuvieran asociados al informe).
6. El sistema confirma la operación al usuario y deja disponible el informe en la pantalla *Informes de Compra*.

**Flujos Alternativos:**
- **2a. El proyecto no tiene materiales faltantes:** el sistema muestra el mensaje *"No hay materiales faltantes para generar el informe"* (`err_informe_sin_faltantes`) y aborta la operación.
- **4a. Ya existía un informe del día:** el sistema no crea uno nuevo; agrega únicamente los faltantes que aún no estaban asociados a ese informe.
- **6a. Error al acceder a la base de datos:** el sistema revierte la transacción y muestra un mensaje de error genérico (`err_db_generic`).

---

## CU – Consultar informes de compra pendientes

**Actor Principal:** Usuario autenticado con permiso `VER_INFORMES_COMPRA`.

**Descripción:** El usuario consulta el listado de informes de compra pendientes, agrupados por proyecto. Por cada proyecto se muestra el informe más reciente con sus materiales faltantes asociados, y se ofrecen acciones para aplicar la compra, eliminar el informe o navegar al historial.

**Precondiciones:**
- El usuario está autenticado y posee el permiso `VER_INFORMES_COMPRA`.

**Postcondiciones:**
- Se muestra al usuario la lista de informes pendientes filtrable por proyecto/cliente.

**Flujo Principal:**
1. El usuario ingresa a la pantalla *Informes de Compra* desde el menú principal.
2. El sistema valida el permiso `VER_INFORMES_COMPRA`; si no lo posee, muestra *"No tenés permisos para acceder a esta pantalla"* y vuelve a *Home*.
3. El sistema obtiene la lista de informes (estado **pendiente**) y los agrupa por proyecto, tomando el informe más reciente de cada uno.
4. Para cada proyecto se construye un ítem visual que muestra: descripción del proyecto, fecha del informe y materiales faltantes incluidos.
5. El usuario puede:
   - Escribir un término de búsqueda y presionar **Buscar** o **Enter** para filtrar por descripción del proyecto o razón social del cliente.
   - Hacer clic en **Historial** para navegar a la pantalla de *Historial de Informes*.
   - Sobre cada ítem, ejecutar **Agregar compra** (CU – Aplicar compra) o **Eliminar** (CU – Cancelar informe de compra).

**Flujos Alternativos:**
- **2a. Sin permisos:** se muestra mensaje de acceso denegado y se navega a *Home*.
- **3a. No hay informes pendientes:** el listado queda vacío, sin mensajes de error.
- **3b. Error al acceder a la base de datos:** se muestra `err_db_generic`.

---

## CU – Aplicar compra de un informe

**Actor Principal:** Usuario autenticado con permiso `GESTIONAR_INFORMES_COMPRA`.

**Descripción:** El usuario confirma que la compra de los materiales del informe fue efectivamente realizada. El sistema incorpora los materiales al detalle del proyecto, guarda un snapshot histórico, elimina los materiales faltantes y marca el informe como **finalizado**.

**Precondiciones:**
- El usuario está autenticado y posee el permiso `GESTIONAR_INFORMES_COMPRA`.
- Existe un informe de compra en estado **pendiente** con materiales faltantes asociados.

**Postcondiciones:**
- El informe queda en estado **finalizado** y se conserva un snapshot histórico de los materiales aplicados.
- Los materiales faltantes son removidos del proyecto y agregados al detalle de materiales del proyecto.
- Otros informes pendientes del mismo proyecto, si los hubiera, quedan en estado **cancelado** automáticamente (al haberse aplicado este informe).

**Flujo Principal:**
1. Desde la pantalla *Informes de Compra*, el usuario hace clic en el botón **"Agregar compra"** del ítem del informe a aplicar.
2. El sistema obtiene los identificadores de materiales faltantes incluidos en el informe.
3. Para cada material faltante, el sistema busca el material correspondiente en el inventario por **descripción + tipo + unidad** (comparación insensible a mayúsculas y espacios).
4. Si encuentra el material en inventario, registra/incrementa la cantidad en el detalle de materiales del proyecto, con fecha del día.
5. El sistema persiste un snapshot histórico de los materiales aplicados mediante `SnapshotService`.
6. El sistema elimina todos los registros de `DetalleInformeMaterialFaltante` que apunten a esos faltantes (incluyendo informes huérfanos anteriores).
7. El sistema marca el informe actual como **"finalizado"** y los demás informes pendientes del mismo proyecto como **"cancelado"**.
8. El sistema elimina los `MaterialFaltante` del proyecto (ya están aplicados al detalle).
9. El sistema muestra el mensaje *"Compra aplicada"* y recarga la lista de informes pendientes.

**Flujos Alternativos:**
- **2a. El informe no posee materiales asociados:** se muestra el error `err_informe_sin_materiales` y se aborta.
- **3a. Algún material faltante no se encuentra en inventario:** el sistema lo omite (registra un *warning* en el log) y continúa con los restantes.
- **3b. Ninguno de los faltantes coincide con materiales del proyecto:** se muestra el error `err_informe_sin_faltantes_proyecto` y se aborta.
- **9a. Error al acceder a la base de datos:** el sistema revierte la transacción completa y muestra `err_db_generic`. Ningún cambio queda persistido.

---

## CU – Cancelar informe de compra

**Actor Principal:** Usuario autenticado con permiso `GESTIONAR_INFORMES_COMPRA`.

**Descripción:** El usuario descarta un informe de compra pendiente sin aplicarlo al proyecto. El informe queda registrado en el historial con estado **cancelado** y los materiales faltantes del proyecto se mantienen disponibles para generar un nuevo informe más adelante (soft-delete por estado).

**Precondiciones:**
- El usuario está autenticado y posee el permiso `GESTIONAR_INFORMES_COMPRA`.
- Existe un informe de compra en estado **pendiente**.

**Postcondiciones:**
- El informe queda en estado **cancelado** (no se elimina físicamente de la base de datos).
- Los materiales faltantes del proyecto permanecen intactos.

**Flujo Principal:**
1. Desde la pantalla *Informes de Compra*, el usuario hace clic en el botón **"Eliminar"** del ítem del informe a cancelar.
2. El sistema obtiene el informe por su identificador.
3. El sistema actualiza el estado del informe a **"cancelado"**.
4. El sistema muestra el mensaje *"Informe eliminado"* y recarga la lista de informes pendientes.

**Flujos Alternativos:**
- **2a. El identificador del informe es inválido o no existe:** se muestra el error `err_informe_id_required` o el error genérico correspondiente.
- **4a. Error al acceder a la base de datos:** se revierte la transacción y se muestra `err_db_generic`.

---

## CU – Consultar historial de informes de compra

**Actor Principal:** Usuario autenticado con permiso `VER_INFORMES_COMPRA`.

**Descripción:** El usuario consulta el historial completo de informes de compra cerrados (estados **finalizado** y **cancelado**), pudiendo ver por cada uno la fecha, el proyecto asociado, su estado final y la lista de materiales involucrados. Para los informes finalizados, los materiales se reconstruyen desde el snapshot persistido; para los cancelados, se leen las referencias originales en base de datos.

**Precondiciones:**
- El usuario está autenticado y posee el permiso `VER_INFORMES_COMPRA`.

**Postcondiciones:**
- Se muestra al usuario el historial filtrable. No se modifican datos (audit trail de solo lectura).

**Flujo Principal:**
1. Desde la pantalla *Informes de Compra*, el usuario hace clic en el botón **"Historial"**.
2. El sistema navega a la pantalla *Historial de Informes*.
3. El sistema obtiene todos los informes con estado distinto de **pendiente** (es decir, **finalizado** y **cancelado**).
4. Para cada informe:
   - Si el estado es **"finalizado"**, el sistema reconstruye la lista de materiales leyendo el snapshot persistido por `SnapshotService` desde almacenamiento local.
   - Si el estado es **"cancelado"**, el sistema obtiene los materiales faltantes asociados al informe directamente desde la base de datos.
5. El sistema construye un ítem visual por cada informe, mostrando fecha, descripción del proyecto, estado y materiales.
6. El usuario puede escribir un término de búsqueda y presionar **Buscar** o **Enter** para filtrar por descripción del proyecto o razón social del cliente.
7. El usuario puede volver a la pantalla anterior haciendo clic en el botón **«**.

**Flujos Alternativos:**
- **3a. No hay informes en el historial:** la lista queda vacía, sin errores.
- **4a. No se encuentra el snapshot de un informe finalizado:** el ítem se muestra sin la lista de materiales (el sistema no aborta la carga del resto).
- **3b. Error al acceder a la base de datos:** se muestra `err_db_generic`.

---

## Observaciones de cobertura

- **CUs nuevos.** Los cinco CUs anteriores no existen en la documentación original, o reemplazan a un único CU genérico de *"Generar informe de compra"* que no cubría la separación entre aplicar, cancelar y consultar historial.
- **Permisos involucrados:** los CUs de **consultar** dependen de `VER_INFORMES_COMPRA`; los de **generar / aplicar / cancelar** dependen de `GESTIONAR_INFORMES_COMPRA`. Ambos pertenecen al enum `TipoPermiso` y se asignan a través del módulo de Usuarios.
- **Soft-delete:** se documenta explícitamente que **Eliminar** un informe no borra el registro físico, sino que cambia su estado a *cancelado*. Esto es central para el historial y para auditoría.
- **Snapshot:** se aclara que los informes finalizados conservan una "foto" de los materiales aplicados, persistida en almacenamiento local (`%LOCALAPPDATA%\GestorCMB\historial\`), de manera independiente de la base. Esto preserva la trazabilidad incluso después de que los `MaterialFaltante` originales son eliminados al confirmar la compra.
- **Ubicación sugerida en el documento principal:** dentro de la sección **"8.4 Casos de Uso – Informes"** (o donde estén agrupados los CUs de informes en tu documento), reemplazando el CU genérico anterior y sumando los cuatro nuevos. Si el orden actual fuese cronológico de uso, conviene presentarlos en este orden: *Generar → Consultar pendientes → Aplicar / Cancelar → Consultar historial*.
