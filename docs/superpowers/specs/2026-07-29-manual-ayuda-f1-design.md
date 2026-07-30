# Manual de ayuda + F1 contextual — Diseño

**Fecha:** 2026-07-29
**Estado:** Aprobado (pendiente de revisión final del spec)

## Objetivo

Que el usuario pueda presionar **F1** en cualquier pantalla de GestorCMB y se abra el
manual de ayuda directamente en la sección que corresponde a esa pantalla. El manual
queda además como entregable (HTML imprimible a PDF), siguiendo el formato del ejemplo
`Manual-Ayuda-RedAceite.html`.

## Decisiones ya tomadas

- **Formato:** HTML único autocontenido abierto en el navegador predeterminado.
  Sin CHM, sin HelpMaker, sin NDoc, sin NuGets nuevos.
- **Idioma del manual:** solo español (coherente con el resto de los entregables).
- **Strings nuevos de UI:** en español **y** en inglés vía `Resources.resx` /
  `Resources.en-US.resx` + `LanguageService.T()`, según la convención i18n del proyecto.
- **Lógica en Services:** la resolución de sección y la apertura del manual viven en el
  proyecto Services. La UI solo aporta el enganche WinForms (filtro de teclado y menú).

## Componentes

### 1. El manual — `MatheoCaffieri_GestorCMB/Ayuda/Manual-Ayuda-GestorCMB.html`

Archivo HTML único, autocontenido, agregado al proyecto WinForms como `Content` con
`CopyToOutputDirectory: PreserveNewest` (termina en `bin\...\Ayuda\`).

Estructura (formato RedAceite):

- Header con título, subtítulo y datos del alumno.
- Sidebar fija con **buscador** (filtro por texto en vivo, JS inline) y navegación por anclas.
- **Fichas por módulo**, cada una con ancla estable, pasos de uso, reglas de negocio,
  permisos requeridos (badge) y capturas:
  - `#inicio` — cómo usar la ayuda (incluye la tecla F1)
  - `#mapa` — mapa del sistema (tiles con acceso a cada módulo)
  - `#m-login` — Login, recupero de contraseña, OTP
  - `#m-principal` — Pantalla principal, menú y permisos
  - `#m-proyectos` — Ver/agregar/editar proyectos, detalle, análisis, empleados y materiales del proyecto
  - `#m-inventario` — Inventario y materiales
  - `#m-proveedores` — Proveedores
  - `#m-informes` — Informes de compra e historial (soft-delete por estado)
  - `#m-empleados` — Empleados
  - `#m-clientes` — Clientes
  - `#m-usuarios` — Gestión de usuarios, roles y accesos
  - `#m-parametros` — Configurar parámetros
  - `#m-logs` — Visor de logs
  - `#m-backup` — Backup y restore
  - `#m-integridad` — Integridad DVH/DVV (qué significa el aviso y qué hacer)
- Secciones transversales:
  - `#mensajes` — tabla de mensajes de error/aviso reales (fuente: `Resources.resx`,
    `Validaciones.cs` y los `MessageBox.Show` del código), con causa y solución.
  - `#permisos` — matriz pantalla → permiso (`TipoPermiso`).
  - `#problemas` — problemas frecuentes.
  - `#glosario` — términos del dominio.
- CSS `@media print` para exportar a PDF desde el navegador (entregable).
- CSS `prefers-color-scheme: dark` como el ejemplo.

**Imágenes:** capturas extraídas de `C:\Users\mcaff\GestorCMB\Documentacion\Manual_Usuario_GestorCMB.pdf`,
embebidas como data URI (base64) para que el HTML siga siendo un solo archivo. Si alguna
sale con calidad insuficiente, se reemplaza por una captura real corriendo la app.

### 2. `Services/Ayuda/AyudaService.cs` (namespace `Services.Ayuda`)

Lógica pura, sin referencia a WinForms:

- `public static class AyudaService`
- Diccionario `string nombrePantalla → string ancla` (claves = nombres de tipo de la UI
  como string, ej. `"VerProyectosControl" → "m-proyectos"`, `"LoginForm" → "m-login"`).
  Se usan strings para no invertir la dependencia Services → UI.
- `AbrirManual()` — abre el manual en `#inicio`.
- `AbrirManual(string nombrePantalla)` — resuelve el ancla (fallback `#inicio`) y abre
  el navegador predeterminado con `Process.Start` sobre
  `file:///<BaseDirectory>/Ayuda/Manual-Ayuda-GestorCMB.html#<ancla>`.
- Ambos métodos devuelven `bool`: `true` si se pudo abrir, `false` si el archivo no
  existe o `Process.Start` falla. En el caso `false`, el service loguea Warn (convención
  de logging) y es el llamador de UI quien muestra el MessageBox traducido.
- Cobertura de pantallas: todos los UserControls de `MainPanel` y todos los Forms
  (Login, ForgotPassword, AddProyectos, EditProyecto, AgregarEmpleadoProyecto,
  AgregarMaterialProyecto, AnalisisProyecto, AddMateriales, EditProveedor,
  AddEmpleados, EditEmpleado, EditCliente, EditUser, Accesos, VerLogs, Backup).
  Pantalla no mapeada → `#inicio`.

### 3. `MatheoCaffieri_GestorCMB/AyudaMessageFilter.cs` (UI)

- `IMessageFilter` que intercepta `WM_KEYDOWN` de F1 en toda la app.
- Resuelve la pantalla activa: `Form.ActiveForm`; si es `MainForm`, usa el tipo del
  UserControl dentro de `MainPanel`. Pasa el nombre del tipo a
  `AyudaService.AbrirManual(nombre)`.
- Registro: **una línea** en `Program.cs`
  (`Application.AddMessageFilter(new AyudaMessageFilter())`).
- Si el manual no está, muestra `MessageBox` con texto traducido
  (`err_manual_no_encontrado`).

### 4. Menú "Ayuda" en `MainForm`

- Item de menú raíz nuevo (ej. `ayudaToolStripMenuItem`), **sin permiso requerido**
  (`Tag = null`), que llama `AyudaService.AbrirManual()`.
- Texto vía recursos: clave `mnu_ayuda` = "Ayuda" / "Help" en `Resources.resx` y
  `Resources.en-US.resx` (patrón `AplicarTraducciones()` existente en MainForm).

## Claves de traducción nuevas

| Clave | es | en |
|---|---|---|
| `mnu_ayuda` | Ayuda | Help |
| `err_manual_no_encontrado` | No se encontró el manual de ayuda. Reinstalá la aplicación o contactá al administrador. | Help manual not found. Reinstall the application or contact your administrator. |
| `cap_ayuda` | Ayuda | Help |

(El contenido del manual en sí NO se traduce: es solo español.)

## Manejo de errores

- Manual ausente en disco → Warn en logs + MessageBox traducido. La app sigue normal.
- Excepción al lanzar el navegador (`Process.Start`) → mismo tratamiento.
- F1 en pantalla sin mapeo → abre `#inicio` (nunca falla ni molesta).

## Qué NO incluye

- No se toca lógica de negocio ni BL/DAL.
- Sin versión en inglés del manual.
- Sin CHM (si el profe lo exige, se compila después desde este mismo HTML).
- Sin `HelpProvider`/`HelpRequested` por form: el filtro global cubre todo.

## Verificación

1. Compila la solución completa sin warnings nuevos.
2. F1 en Home, Ver Proyectos, Inventario, Gestión de Usuarios, Login y Backup abre el
   navegador en la sección correcta.
3. F1 en una pantalla sin ficha abre `#inicio`.
4. Menú "Ayuda" abre el manual; con idioma inglés activo el item dice "Help".
5. Renombrar el HTML y presionar F1 → MessageBox traducido, la app no se cae.
6. El buscador del manual filtra secciones; impresión a PDF sale legible.
