# Manual de ayuda + F1 contextual — Plan de implementación

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Manual de ayuda HTML único (formato RedAceite) que se abre con F1 en la sección de la pantalla activa, con la lógica en Services y el enganche WinForms mínimo en la UI.

**Architecture:** `Services.Ayuda.AyudaService` (estático, sin WinForms) mapea nombre-de-pantalla → ancla y abre el navegador predeterminado con `file:///...#ancla`. En la UI, un `IMessageFilter` global registrado en `Program.cs` intercepta F1 y le pasa el nombre del tipo de la pantalla activa; un item de menú "Ayuda" en `MainForm` abre el manual desde `#inicio`. El manual es un HTML autocontenido en `Ayuda\` copiado a la salida por el csproj.

**Tech Stack:** .NET Framework 4.7.2, WinForms, csproj estilo viejo (includes explícitos), Python 3.13 + PyMuPDF 1.28 (ya instalado) solo como herramienta de build para extraer/embeber imágenes.

**Spec:** `docs/superpowers/specs/2026-07-29-manual-ayuda-f1-design.md`

## Global Constraints

- Sin NuGets ni referencias nuevas en ningún proyecto.
- Todo string visible de UI sale de `LanguageService.Current?.T("clave") ?? "fallback en español"`, con la clave agregada en `Properties\Resources.resx` (es) **y** `Properties\Resources.en-US.resx` (en).
- Convención de logs: fallas esperables → `LogHelper.Warn(scope, msg)`; scope estilo `"[Ayuda] ..."`. No loguear operaciones exitosas de UI.
- Comentarios en español y solo para explicar *por qué* (estilo del repo).
- Commits estilo del repo: `29-7: <descripción en minúsculas>` + línea `Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>`.
- Build de verificación (no hay proyectos de test en la solución; la verificación es compilar + smoke test manual):
  `& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" MatheoCaffieri_GestorCMB.sln /p:Configuration=Debug /v:m /nologo`
  Esperado: `0 Error(es)` y ningún warning nuevo respecto del build previo a los cambios.
- El manual es SOLO español. Las anclas del HTML y las del diccionario de `AyudaService` tienen que coincidir exactamente con la **tabla canónica de anclas** de abajo.

## Tabla canónica pantalla → ancla

| Tipo (nombre exacto de clase) | Ancla |
|---|---|
| `LoginForm`, `ForgotPasswordForm` | `m-login` |
| `MainForm`, `HomeControl` | `m-principal` |
| `VerProyectosControl`, `DetalleProyectoControl`, `AddProyectosForm`, `EditProyectoForm`, `AgregarEmpleadoProyectoForm`, `AgregarMaterialProyectoForm`, `AnalisisProyectoForm` | `m-proyectos` |
| `VerInventarioControl`, `AddMaterialesForm` | `m-inventario` |
| `ProveedorControl`, `EditProveedorForm` | `m-proveedores` |
| `InformesDeCompraControl`, `HistorialInformesControl` | `m-informes` |
| `VerEmpleadosControl`, `AddEmpleadosForm`, `EditEmpleadoForm` | `m-empleados` |
| `ClientesControl`, `EditClienteForm` | `m-clientes` |
| `GestionUsuariosControl`, `EditUserForm`, `AccesosForm` | `m-usuarios` |
| `ConfigurarParametrosControl` | `m-parametros` |
| `VerLogsForm` | `m-logs` |
| `BackupForm` | `m-backup` |
| cualquier otro / `null` | `inicio` |

Anclas transversales del manual (sin mapeo F1): `inicio`, `mapa`, `m-integridad`, `mensajes`, `permisos`, `problemas`, `glosario`.

---

### Task 1: `AyudaService` en Services

**Files:**
- Create: `Services/Ayuda/AyudaService.cs`
- Modify: `Services/Services.csproj` (bloque de `<Compile Include="Language\..." />`, ~línea 77)

**Interfaces:**
- Consumes: `Services.Logs.LogHelper.Warn(string scope, string msg)` (existente).
- Produces: `Services.Ayuda.AyudaService.AbrirManual() : bool` y `AbrirManual(string nombrePantalla) : bool`. Devuelven `false` si el HTML no existe o no se pudo lanzar el navegador (el llamador de UI muestra el error). Nunca lanzan excepción.

- [ ] **Step 1: Crear `Services/Ayuda/AyudaService.cs`**

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Services.Logs;

namespace Services.Ayuda
{
    // Ayuda contextual: resuelve la sección del manual según la pantalla activa
    // y abre el navegador predeterminado. No referencia WinForms a propósito:
    // la UI solo le pasa el nombre del tipo de la pantalla como string.
    public static class AyudaService
    {
        private const string ArchivoManual = @"Ayuda\Manual-Ayuda-GestorCMB.html";
        private const string AnclaInicio = "inicio";

        private static readonly Dictionary<string, string> Secciones =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["LoginForm"] = "m-login",
                ["ForgotPasswordForm"] = "m-login",
                ["MainForm"] = "m-principal",
                ["HomeControl"] = "m-principal",
                ["VerProyectosControl"] = "m-proyectos",
                ["DetalleProyectoControl"] = "m-proyectos",
                ["AddProyectosForm"] = "m-proyectos",
                ["EditProyectoForm"] = "m-proyectos",
                ["AgregarEmpleadoProyectoForm"] = "m-proyectos",
                ["AgregarMaterialProyectoForm"] = "m-proyectos",
                ["AnalisisProyectoForm"] = "m-proyectos",
                ["VerInventarioControl"] = "m-inventario",
                ["AddMaterialesForm"] = "m-inventario",
                ["ProveedorControl"] = "m-proveedores",
                ["EditProveedorForm"] = "m-proveedores",
                ["InformesDeCompraControl"] = "m-informes",
                ["HistorialInformesControl"] = "m-informes",
                ["VerEmpleadosControl"] = "m-empleados",
                ["AddEmpleadosForm"] = "m-empleados",
                ["EditEmpleadoForm"] = "m-empleados",
                ["ClientesControl"] = "m-clientes",
                ["EditClienteForm"] = "m-clientes",
                ["GestionUsuariosControl"] = "m-usuarios",
                ["EditUserForm"] = "m-usuarios",
                ["AccesosForm"] = "m-usuarios",
                ["ConfigurarParametrosControl"] = "m-parametros",
                ["VerLogsForm"] = "m-logs",
                ["BackupForm"] = "m-backup",
            };

        public static bool AbrirManual() => AbrirEnAncla(AnclaInicio);

        public static bool AbrirManual(string nombrePantalla)
        {
            string ancla;
            if (nombrePantalla == null || !Secciones.TryGetValue(nombrePantalla, out ancla))
                ancla = AnclaInicio;
            return AbrirEnAncla(ancla);
        }

        private static bool AbrirEnAncla(string ancla)
        {
            try
            {
                var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArchivoManual);
                if (!File.Exists(ruta))
                {
                    LogHelper.Warn("[Ayuda]", $"No se encontró el manual en {ruta}");
                    return false;
                }

                var url = new Uri(ruta).AbsoluteUri + "#" + ancla;

                // ShellExecute directo sobre un file:// suele descartar el #ancla,
                // así que invocamos el exe del navegador predeterminado con la URL
                // como argumento. Si no lo podemos resolver, abrimos sin ancla.
                var navegador = ResolverExeNavegador();
                if (navegador != null)
                    Process.Start(navegador, "\"" + url + "\"");
                else
                    Process.Start(ruta);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("[Ayuda]", ex, "No se pudo abrir el manual");
                return false;
            }
        }

        private static string ResolverExeNavegador()
        {
            try
            {
                string progId;
                using (var k = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
                {
                    progId = k?.GetValue("ProgId") as string;
                }
                if (string.IsNullOrEmpty(progId)) return null;

                string comando;
                using (var k = Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command"))
                {
                    comando = k?.GetValue(null) as string;
                }
                if (string.IsNullOrEmpty(comando)) return null;

                // El comando viene como: "C:\...\msedge.exe" --flags "%1"
                comando = comando.TrimStart();
                if (comando.StartsWith("\"", StringComparison.Ordinal))
                {
                    var fin = comando.IndexOf('"', 1);
                    return fin > 1 ? comando.Substring(1, fin - 1) : null;
                }
                var espacio = comando.IndexOf(' ');
                return espacio > 0 ? comando.Substring(0, espacio) : comando;
            }
            catch
            {
                return null;
            }
        }
    }
}
```

- [ ] **Step 2: Registrar el archivo en `Services/Services.csproj`**

En el `<ItemGroup>` que contiene `<Compile Include="Backup\BackupService.cs" />` (~línea 85), agregar en orden alfabético de carpeta (antes de las de `Backup\`):

```xml
    <Compile Include="Ayuda\AyudaService.cs" />
```

- [ ] **Step 3: Compilar**

Run: `& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" MatheoCaffieri_GestorCMB.sln /p:Configuration=Debug /v:m /nologo`
Esperado: 0 errores.

- [ ] **Step 4: Commit**

```powershell
git add Services/Ayuda/AyudaService.cs Services/Services.csproj
git commit -m @'
29-7: AyudaService en Services, resuelve seccion del manual y abre el navegador

Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>
'@
```

---

### Task 2: Claves de traducción es/en

**Files:**
- Modify: `MatheoCaffieri_GestorCMB/Properties/Resources.resx`
- Modify: `MatheoCaffieri_GestorCMB/Properties/Resources.en-US.resx`

**Interfaces:**
- Produces: claves `mnu_ayuda`, `cap_ayuda`, `err_manual_no_encontrado` resolubles vía `LanguageService.Current?.T("clave")` (Tasks 3 y 4 las consumen).

- [ ] **Step 1: Agregar las claves en `Resources.resx`**

Justo antes del `</root>` final, siguiendo el formato de las `<data>` existentes:

```xml
  <data name="mnu_ayuda" xml:space="preserve">
    <value>Ayuda</value>
  </data>
  <data name="cap_ayuda" xml:space="preserve">
    <value>Ayuda</value>
  </data>
  <data name="err_manual_no_encontrado" xml:space="preserve">
    <value>No se encontró el manual de ayuda. Reinstalá la aplicación o contactá al administrador.</value>
  </data>
```

- [ ] **Step 2: Agregar las claves en `Resources.en-US.resx`**

Justo antes del `</root>` final:

```xml
  <data name="mnu_ayuda" xml:space="preserve">
    <value>Help</value>
  </data>
  <data name="cap_ayuda" xml:space="preserve">
    <value>Help</value>
  </data>
  <data name="err_manual_no_encontrado" xml:space="preserve">
    <value>Help manual not found. Reinstall the application or contact your administrator.</value>
  </data>
```

- [ ] **Step 3: Compilar**

Run: `& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" MatheoCaffieri_GestorCMB.sln /p:Configuration=Debug /v:m /nologo`
Esperado: 0 errores (el resx se compila a .resources; `ResxLanguageRepository` resuelve por nombre, no hace falta regenerar `Resources.Designer.cs`).

- [ ] **Step 4: Commit**

```powershell
git add MatheoCaffieri_GestorCMB/Properties/Resources.resx MatheoCaffieri_GestorCMB/Properties/Resources.en-US.resx
git commit -m @'
29-7: claves de traduccion para el menu y errores de ayuda

Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>
'@
```

---

### Task 3: Filtro global de F1 + registro en `Program.cs`

**Files:**
- Create: `MatheoCaffieri_GestorCMB/AyudaMessageFilter.cs`
- Modify: `MatheoCaffieri_GestorCMB/MainForm.cs` (agregar método público chico)
- Modify: `MatheoCaffieri_GestorCMB/Program.cs:46-47`
- Modify: `MatheoCaffieri_GestorCMB/MatheoCaffieri_GestorCMB.csproj` (bloque de Compile, ~línea 304 donde está `PermisosUI.cs`)

**Interfaces:**
- Consumes: `AyudaService.AbrirManual(string)` (Task 1), claves `err_manual_no_encontrado` / `cap_ayuda` (Task 2), `MainForm.NombrePantallaActual()` (este mismo task).
- Produces: `AyudaMessageFilter : IMessageFilter` registrado en `Program.Main`; `MainForm.NombrePantallaActual() : string` (nombre del tipo del UserControl visible en `MainPanel`, o `null`).

- [ ] **Step 1: Agregar `NombrePantallaActual()` a `MainForm.cs`**

Debajo de `addUserControl` (después de la línea `userControl.BringToFront(); }`):

```csharp
        // Para la ayuda contextual: F1 necesita saber qué pantalla está viendo
        // el usuario, y acá el "form activo" siempre es MainForm.
        public string NombrePantallaActual()
        {
            return MainPanel.Controls.Count > 0
                ? MainPanel.Controls[0].GetType().Name
                : null;
        }
```

- [ ] **Step 2: Crear `MatheoCaffieri_GestorCMB/AyudaMessageFilter.cs`**

```csharp
using System;
using System.Windows.Forms;
using Services.Ayuda;
using Services.Language;

namespace MatheoCaffieri_GestorCMB
{
    // Escucha F1 en toda la app (se registra una sola vez en Program.Main)
    // y abre el manual en la sección de la pantalla activa. Filtro global
    // en vez de HelpRequested por form: cubre pantallas futuras sin tocarlas.
    public class AyudaMessageFilter : IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100;
        private const int VK_F1 = 0x70;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_KEYDOWN || m.WParam != (IntPtr)VK_F1)
                return false;

            // Bit 30 del lParam: la tecla ya estaba apretada (autorepeat).
            // Sin este filtro, mantener F1 abriría una pestaña por repetición.
            if ((m.LParam.ToInt64() & 0x40000000) != 0)
                return true;

            if (!AyudaService.AbrirManual(ResolverPantallaActiva()))
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_manual_no_encontrado")
                        ?? "No se encontró el manual de ayuda. Reinstalá la aplicación o contactá al administrador.",
                    LanguageService.Current?.T("cap_ayuda") ?? "Ayuda",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return true;
        }

        private static string ResolverPantallaActiva()
        {
            var form = Form.ActiveForm;
            if (form is MainForm main)
                return main.NombrePantallaActual() ?? nameof(MainForm);
            return form?.GetType().Name;
        }
    }
}
```

- [ ] **Step 3: Registrar el filtro en `Program.cs`**

Después de `Application.SetCompatibleTextRenderingDefault(false);` (línea 47):

```csharp
            Application.AddMessageFilter(new AyudaMessageFilter());
```

- [ ] **Step 4: Registrar el archivo en el csproj de la UI**

En `MatheoCaffieri_GestorCMB/MatheoCaffieri_GestorCMB.csproj`, en el ItemGroup de `<Compile>` (cerca de `<Compile Include="PermisosUI.cs" />`, ~línea 304):

```xml
    <Compile Include="AyudaMessageFilter.cs" />
```

- [ ] **Step 5: Compilar**

Run: `& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" MatheoCaffieri_GestorCMB.sln /p:Configuration=Debug /v:m /nologo`
Esperado: 0 errores.

- [ ] **Step 6: Smoke test de F1 (manual todavía ausente)**

Correr la app (`MatheoCaffieri_GestorCMB\bin\Debug\MatheoCaffieri_GestorCMB.exe`), presionar F1 en el Login.
Esperado: MessageBox "No se encontró el manual de ayuda…" (el HTML aún no existe) y la app sigue viva. Esto valida el camino de error del spec.

- [ ] **Step 7: Commit**

```powershell
git add MatheoCaffieri_GestorCMB/AyudaMessageFilter.cs MatheoCaffieri_GestorCMB/MainForm.cs MatheoCaffieri_GestorCMB/Program.cs MatheoCaffieri_GestorCMB/MatheoCaffieri_GestorCMB.csproj
git commit -m @'
29-7: F1 abre la ayuda contextual via filtro global de mensajes

Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>
'@
```

---

### Task 4: Menú "Ayuda" en MainForm

**Files:**
- Modify: `MatheoCaffieri_GestorCMB/MainForm.Designer.cs` (declaración ~línea 53, instanciación ~línea 50-53, `menuStrip1.Items.AddRange` ~línea 112-117, campo privado ~línea 271)
- Modify: `MatheoCaffieri_GestorCMB/MainForm.cs` (`AplicarTraducciones()` línea 55-59, nuevo handler)

**Interfaces:**
- Consumes: `AyudaService.AbrirManual()` (Task 1), clave `mnu_ayuda` (Task 2), patrón `AplicarTraducciones()` existente.
- Produces: item `ayudaToolStripMenuItem` visible siempre (sin permiso, `Tag = null` implícito).

- [ ] **Step 1: Declarar e instanciar el item en `MainForm.Designer.cs`**

Junto a las demás instanciaciones (después de la línea `this.configurarParametrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();`):

```csharp
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
```

En `menuStrip1.Items.AddRange`, agregar al final del array (después de `this.ajustesToolStripMenuItem`):

```csharp
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.proyectosToolStripMenuItem,
            this.inventarioToolStripMenuItem,
            this.personalToolStripMenuItem,
            this.ajustesToolStripMenuItem,
            this.ayudaToolStripMenuItem});
```

Después del bloque de configuración de `ajustesToolStripMenuItem` (que termina asignando `this.ajustesToolStripMenuItem.Name`), agregar el bloque del item nuevo. OJO: sin `resources.ApplyResources` — el form es Localizable pero este item no tiene entrada en el resx del form; el texto se setea en `AplicarTraducciones()` como ya se hace con `configurarParametrosToolStripMenuItem`:

```csharp
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Click += new System.EventHandler(this.ayudaToolStripMenuItem_Click);
```

Al final del archivo, junto al campo `private System.Windows.Forms.ToolStripMenuItem ajustesToolStripMenuItem;`:

```csharp
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
```

- [ ] **Step 2: Texto traducido + handler en `MainForm.cs`**

En `AplicarTraducciones()` (línea 55-59), agregar debajo de la asignación existente:

```csharp
            ayudaToolStripMenuItem.Text =
                LanguageService.Current?.T("mnu_ayuda") ?? "Ayuda";
```

Al final de la clase (después de `configurarParametrosToolStripMenuItem_Click`):

```csharp
        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // La ayuda es libre: no pasa por Require() a propósito.
            if (!Services.Ayuda.AyudaService.AbrirManual())
            {
                MessageBox.Show(
                    LanguageService.Current?.T("err_manual_no_encontrado")
                        ?? "No se encontró el manual de ayuda. Reinstalá la aplicación o contactá al administrador.",
                    LanguageService.Current?.T("cap_ayuda") ?? "Ayuda",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
```

Nota: `SetupMenuPermissionTags()` no se toca — el item queda con `Tag = null` y `ApplyMenuItemPermissions` lo deja habilitado porque no tiene hijos ni permiso propio.

- [ ] **Step 3: Compilar**

Run: `& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" MatheoCaffieri_GestorCMB.sln /p:Configuration=Debug /v:m /nologo`
Esperado: 0 errores.

- [ ] **Step 4: Smoke test del menú**

Correr la app, loguearse. Esperado: menú "Ayuda" visible al final de la barra para cualquier usuario; al clickearlo (manual aún ausente) sale el MessageBox traducido. Cambiar idioma a inglés y verificar que el item diga "Help".

- [ ] **Step 5: Commit**

```powershell
git add MatheoCaffieri_GestorCMB/MainForm.Designer.cs MatheoCaffieri_GestorCMB/MainForm.cs
git commit -m @'
29-7: item de menu ayuda en mainform, sin permiso requerido

Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>
'@
```

---

### Task 5: Extraer capturas del Manual de Usuario (PDF)

**Files:**
- Create: `<scratchpad>\extraer_imagenes.py` (herramienta descartable, NO va al repo)
- Output: `<scratchpad>\img\pNNN_iNN.png` (capturas candidatas)

`<scratchpad>` = `C:\Users\mcaff\AppData\Local\Temp\claude\C--Users-mcaff-source-repos-MatheoCaffieri-GestorCMB\b5c2482a-82fb-46b5-b3f6-9bb7e58e6e2e\scratchpad`

**Interfaces:**
- Produces: PNGs nombrados por página del PDF, para que Task 6 elija cuáles embeber. PyMuPDF 1.28 ya está instalado (`import fitz` — NO `pip install`).

- [ ] **Step 1: Escribir `extraer_imagenes.py`**

```python
# Extrae las imágenes embebidas del Manual de Usuario para reusarlas en la ayuda.
import os
import fitz  # PyMuPDF

PDF = r"C:\Users\mcaff\GestorCMB\Documentacion\Manual_Usuario_GestorCMB.pdf"
OUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "img")
os.makedirs(OUT, exist_ok=True)

doc = fitz.open(PDF)
total = 0
for pno in range(len(doc)):
    for i, info in enumerate(doc.get_page_images(pno, full=True)):
        xref = info[0]
        pix = fitz.Pixmap(doc, xref)
        if pix.n - pix.alpha > 3:          # CMYK u otro: convertir a RGB
            pix = fitz.Pixmap(fitz.csRGB, pix)
        if pix.width < 200 or pix.height < 120:
            continue                        # íconos y viñetas no sirven
        pix.save(os.path.join(OUT, f"p{pno + 1:03d}_i{i:02d}.png"))
        total += 1
print(f"{len(doc)} páginas, {total} imágenes útiles en {OUT}")
```

- [ ] **Step 2: Ejecutarlo y revisar el resultado**

Run: `python "<scratchpad>\extraer_imagenes.py"`
Esperado: mensaje final con N imágenes > 0. Después, listar `<scratchpad>\img` y abrir (Read) 4-5 PNGs de muestra para confirmar que son capturas de pantallas de la app y se leen bien.

Contingencia: si salen 0 imágenes o ilegibles (PDF con capturas vectoriales o recomprimidas), renderizar páginas enteras como fallback con `page.get_pixmap(dpi=150).save(...)` y recortar a mano las que hagan falta; si tampoco alcanza, correr la app y sacar capturas reales (última opción, más lenta).

- [ ] **Step 3: Mapear capturas a secciones**

Leer el PDF por páginas (herramienta Read, parámetro `pages`) para saber qué pantalla muestra cada página, y anotar en un archivo `<scratchpad>\mapa_capturas.md` qué PNG va a cada ancla (`m-login`, `m-proyectos`, etc.). Una captura representativa por módulo alcanza; máximo 2. No hace falta commit (nada de esto entra al repo).

---

### Task 6: El manual — `Ayuda/Manual-Ayuda-GestorCMB.html`

**Files:**
- Create: `MatheoCaffieri_GestorCMB/Ayuda/Manual-Ayuda-GestorCMB.html`
- Create: `<scratchpad>\embeber_imagenes.py` (herramienta descartable)
- Modify: `MatheoCaffieri_GestorCMB/MatheoCaffieri_GestorCMB.csproj` (ItemGroup con `<None Include="App.config" />`, ~línea 532)

**Interfaces:**
- Consumes: capturas y `mapa_capturas.md` de Task 5. Anclas EXACTAS de la tabla canónica (Task 1 ya las usa).
- Produces: el manual copiado a `bin\Debug\Ayuda\` en cada build (lo que `AyudaService.ArchivoManual` espera).

**Fuentes de contenido (leer antes de redactar):**
- Formato/estilo: `C:\Users\mcaff\Downloads\Manual-Ayuda-RedAceite.html` (copiar estructura de CSS, sidebar con buscador JS, cards, badges de permisos, tabla de mensajes, print CSS y dark mode — reescribiendo textos y datos para GestorCMB).
- Mensajes reales: `MatheoCaffieri_GestorCMB/Properties/Resources.resx` (todas las claves `err_*`, `msg_*`, `cap_*`) y `MatheoCaffieri_GestorCMB/Validaciones.cs`.
- Permisos por pantalla: `MainForm.cs` `SetupMenuPermissionTags()` + enum `TipoPermiso` (en Services/RoleService) → sección `#permisos`.
- Flujo de cada pantalla: el `.cs` de cada Control/Form (botones, validaciones, confirmaciones) + el texto del `Manual_Usuario_GestorCMB.pdf` (Read por páginas) como base de redacción.
- Reglas de dominio ya documentadas: convenciones IsActive (altas activas, pickers filtran), historial de informes con soft-delete por estado, integridad DVH/DVV, OTP — están en la memoria del proyecto y en el código citado.

- [ ] **Step 1: Redactar el HTML con referencias locales a imágenes**

Estructura obligatoria (mismo orden que el sidebar):

1. Header: "Manual de Ayuda — GestorCMB", subtítulo, meta-line "Alumno: Mateo Nicolás Caffieri · Universidad Abierta Interamericana · 2026".
2. `#inicio` — cómo usar la ayuda; explicar **F1 contextual** y el menú Ayuda.
3. `#mapa` — tiles con link a cada módulo.
4. Fichas de módulos, cada card con: ancla de la tabla canónica, badge del permiso requerido (`badge perm`) o "acceso libre" (`badge libre`), pasos de uso, reglas ("qué valida"), y `<figure>` con captura donde el mapa de Task 5 lo indique: `m-login`, `m-principal`, `m-proyectos`, `m-inventario`, `m-proveedores`, `m-informes`, `m-empleados`, `m-clientes`, `m-usuarios`, `m-parametros`, `m-logs`, `m-backup`, `m-integridad` (esta última sin F1: explica el aviso de inconsistencias al iniciar y qué hacer).
5. `#mensajes` — tabla: mensaje literal (es), dónde aparece, causa, solución.
6. `#permisos` — matriz pantalla → permiso `TipoPermiso`.
7. `#problemas` — problemas frecuentes (no conecta a la BD, olvidé la contraseña/OTP, no me aparece un menú → permisos, aviso de integridad).
8. `#glosario` — términos (proyecto, informe de compra, material faltante, DVH/DVV, OTP, rol/permiso, backup).
9. Footer.

Las imágenes se referencian primero como `<img src="img/pNNN_iNN.png">` (rutas al scratchpad copiadas relativas: usar `src="IMG::pNNN_iNN.png"` como marcador) — el paso siguiente las embebe.

- [ ] **Step 2: Escribir y correr `embeber_imagenes.py`**

```python
# Reemplaza los marcadores IMG::archivo.png por data URIs base64,
# achicando a 1000px de ancho máximo para que el HTML no explote.
import base64, io, os, re
from PIL import Image  # Pillow 11.1 ya está instalado en esta máquina

SCRATCH = os.path.dirname(os.path.abspath(__file__))
IMG_DIR = os.path.join(SCRATCH, "img")
HTML = r"C:\Users\mcaff\source\repos\MatheoCaffieri_GestorCMB\MatheoCaffieri_GestorCMB\Ayuda\Manual-Ayuda-GestorCMB.html"

def a_data_uri(nombre):
    ruta = os.path.join(IMG_DIR, nombre)
    img = Image.open(ruta)
    if img.width > 1000:
        img = img.resize((1000, int(img.height * 1000 / img.width)), Image.LANCZOS)
    buf = io.BytesIO()
    img.save(buf, format="PNG", optimize=True)
    return "data:image/png;base64," + base64.b64encode(buf.getvalue()).decode()

with open(HTML, encoding="utf-8") as f:
    contenido = f.read()

pendientes = re.findall(r'IMG::([\w.\-]+)', contenido)
for nombre in pendientes:
    contenido = contenido.replace(f"IMG::{nombre}", a_data_uri(nombre))

with open(HTML, "w", encoding="utf-8") as f:
    f.write(contenido)
print(f"{len(pendientes)} imágenes embebidas" if pendientes else "sin marcadores IMG:: — revisar")
```

Run: `python "<scratchpad>\embeber_imagenes.py"`
Esperado: "N imágenes embebidas", y `Grep "IMG::"` sobre el HTML no devuelve nada.

- [ ] **Step 3: Verificar el HTML en el navegador**

Abrir `MatheoCaffieri_GestorCMB\Ayuda\Manual-Ayuda-GestorCMB.html` en el navegador. Checklist:
- Sidebar navega a cada ancla; buscador filtra en vivo (probar "contraseña" y un texto de error).
- Todas las capturas se ven; ninguna referencia rota.
- Probar `...html#m-proyectos` a mano en la barra de direcciones: salta a la ficha.
- Vista previa de impresión: sale legible, sin sidebar.

- [ ] **Step 4: Registrar como Content en el csproj de la UI**

En `MatheoCaffieri_GestorCMB.csproj`, en el ItemGroup donde está `<None Include="App.config" />` (~línea 532):

```xml
    <Content Include="Ayuda\Manual-Ayuda-GestorCMB.html">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
```

- [ ] **Step 5: Compilar y verificar la copia**

Run: build MSBuild (comando de Global Constraints) y después `Test-Path "MatheoCaffieri_GestorCMB\bin\Debug\Ayuda\Manual-Ayuda-GestorCMB.html"`
Esperado: 0 errores y `True`.

- [ ] **Step 6: Commit**

```powershell
git add MatheoCaffieri_GestorCMB/Ayuda/Manual-Ayuda-GestorCMB.html MatheoCaffieri_GestorCMB/MatheoCaffieri_GestorCMB.csproj
git commit -m @'
29-7: manual de ayuda html con buscador, permisos y capturas por modulo

Co-Authored-By: Claude Fable 5 <noreply@anthropic.com>
'@
```

---

### Task 7: Verificación integral (spec, sección "Verificación")

**Files:** ninguno nuevo (solo correcciones si algo falla).

- [ ] **Step 1: Build limpio**

Run: comando MSBuild de Global Constraints.
Esperado: 0 errores, sin warnings nuevos.

- [ ] **Step 2: Recorrido F1 con la app corriendo**

Correr `MatheoCaffieri_GestorCMB\bin\Debug\MatheoCaffieri_GestorCMB.exe` y verificar:

1. F1 en **Login** → navegador en `#m-login`.
2. Loguearse; F1 en **Home** → `#m-principal`.
3. Menú Proyectos → Ver Proyectos; F1 → `#m-proyectos`.
4. Inventario → Ver Inventario; F1 → `#m-inventario`.
5. Ajustes → Gestionar Usuarios; F1 → `#m-usuarios`.
6. Abrir **Backup**; F1 con ese form activo → `#m-backup`.
7. Menú **Ayuda** → abre en `#inicio`.
8. Cambiar idioma a inglés → el menú dice "Help"; F1 sigue funcionando (manual en español, esperado).
9. Renombrar temporalmente `bin\Debug\Ayuda\Manual-Ayuda-GestorCMB.html`, F1 → MessageBox traducido, la app no se cae; verificar línea Warn `[Ayuda]` en el visor de logs. Restaurar el nombre.
10. Pantalla sin ficha → `#inicio` (spec, punto 3): presionar F1 con un `MessageBox` cualquiera abierto (por ejemplo el de confirmación al salir) — el form activo no está en el diccionario, así que debe abrir el manual en `#inicio` sin romperse.

- [ ] **Step 3: Chequeo de consistencia anclas**

Grep sobre el HTML: cada ancla del diccionario de `AyudaService` (`m-login` … `m-backup`, `inicio`) existe como `id="..."` en el manual. Cero diferencias.

- [ ] **Step 4: Commit final si hubo correcciones + cierre**

Si el recorrido obligó a tocar algo, commitear con el estilo del repo. Al cerrar, usar superpowers:finishing-a-development-branch NO aplica (se trabajó en master por decisión del repo — commits directos como viene haciendo el usuario).
