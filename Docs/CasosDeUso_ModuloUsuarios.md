# Módulo Usuarios — Casos de Uso

Documento complementario al documento principal del proyecto. Incorpora los casos de uso del módulo de **Gestión de Usuarios**, **Autenticación**, **Internacionalización** y reescribe el caso de uso de **Configurar parámetros** para alinearlo a la implementación actual del sistema.

Formato y estilo idénticos a los CUs ya existentes en el documento principal (Actor / Descripción / Pre / Post / Flujo principal / Flujos alternativos).

---

## Actores

**Administrador**
Usuario con el permiso `GESTIONAR_USUARIOS`. Es el único actor con capacidad para crear, modificar, activar/desactivar usuarios y asignar roles y permisos. Por defecto, el rol **Administrador** (predefinido) tiene este permiso de forma protegida (no es posible modificar sus permisos desde la interfaz).

**Usuario autenticado (cualquier rol)**
Cualquier usuario que haya iniciado sesión exitosamente. Puede modificar sus propios datos básicos (idioma, contraseña) y recuperar su contraseña a través del flujo de OTP.

**Sistema**
Actor no humano que interviene en la generación y envío de códigos OTP por correo electrónico, hashing y verificación de contraseñas con BCrypt, y registro en bitácora.

---

## Gestión de Usuarios

### CU001: Crear usuario

**Actor Principal:** Administrador

**Descripción:** El administrador crea un nuevo usuario en el sistema con sus credenciales de acceso, idioma preferido y, opcionalmente, un rol inicial. La contraseña se almacena hasheada con BCrypt; nunca se persiste en texto plano.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.
- El correo del nuevo usuario no debe estar registrado previamente en el sistema.

**Postcondiciones:**
- Un nuevo usuario queda registrado en la base de datos de usuarios con `IsActive = true`.
- La contraseña queda almacenada como hash BCrypt.
- Si se seleccionó un rol, el usuario queda vinculado a dicho rol y hereda sus permisos.

**Flujo Principal:**
1. El administrador accede a la sección "Gestión de Usuarios".
2. El sistema muestra el formulario "Nuevo usuario" con los campos: Mail, Contraseña, Teléfono, Idioma y Rol.
3. El administrador ingresa los datos del nuevo usuario.
4. El administrador selecciona la opción "Crear usuario".
5. El sistema valida los datos ingresados:
   - Mail no vacío.
   - Contraseña no vacía.
   - Teléfono numérico (si fue ingresado).
   - Mail no duplicado.
6. El sistema hashea la contraseña con BCrypt.
7. El sistema persiste el nuevo usuario en la base de datos.
8. Si se seleccionó un rol, el sistema asigna el usuario al rol indicado.
9. El sistema muestra un mensaje de confirmación y refresca la lista de usuarios.

**Flujos Alternativos:**

*1: Datos incompletos o inválidos*
Si alguno de los campos obligatorios está vacío o el teléfono no es numérico, el sistema muestra un mensaje de error indicando el campo afectado y posiciona el foco en él. El administrador corrige los datos y vuelve al paso 4.

*2: Mail duplicado*
Si ya existe un usuario con el mismo mail, el sistema muestra el mensaje "Ya existe un usuario con ese mail" y aborta la operación. El administrador puede modificar el mail y reintentar.

*3: Error del sistema*
Si ocurre un error al persistir el usuario, el sistema muestra un mensaje de error genérico. El administrador puede reintentar o contactar al soporte.

---

### CU002: Modificar usuario

**Actor Principal:** Administrador

**Descripción:** El administrador modifica los datos generales de un usuario existente (mail, contraseña, teléfono, idioma).

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.
- El usuario a modificar debe existir en el sistema.

**Postcondiciones:**
- Los datos del usuario quedan actualizados en la base de datos.
- Si el usuario modificado es el mismo que está autenticado y cambió su idioma, el sistema reinicia la aplicación para aplicar el nuevo `CultureCode`.

**Flujo Principal:**
1. El administrador selecciona un usuario de la lista en la sección "Gestión de Usuarios".
2. El sistema abre el formulario "Editar usuario" con los datos actuales.
3. El administrador modifica los campos necesarios.
4. El administrador selecciona la opción "Editar usuario".
5. El sistema valida los datos:
   - Mail no vacío.
   - Contraseña no vacía.
   - Teléfono numérico.
6. El sistema actualiza el usuario en la base de datos.
7. Si el usuario actualizado coincide con el usuario logueado y cambió el idioma, el sistema persiste el `CultureCode` (`es-AR` o `en-US`) en la configuración de la aplicación y reinicia el programa.
8. En caso contrario, el sistema muestra un mensaje de confirmación y cierra el formulario.

**Flujos Alternativos:**

*1: Datos inválidos*
Si el mail está vacío, la contraseña está vacía o el teléfono no es numérico, el sistema muestra un mensaje de aviso y mantiene abierto el formulario para corrección.

*2: Error del sistema*
Si ocurre un error al actualizar, el sistema muestra el mensaje "No se pudo actualizar el usuario". El administrador puede reintentar.

---

### CU003: Activar / Desactivar usuario

**Actor Principal:** Administrador

**Descripción:** El administrador habilita o deshabilita un usuario sin eliminarlo del sistema. Los usuarios inactivos no pueden iniciar sesión ni recibir códigos OTP de recuperación. La operación funciona como baja lógica mediante el flag `IsActive`.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.
- El usuario sobre el que se opera debe existir.

**Postcondiciones:**
- El estado `IsActive` del usuario queda actualizado en la base de datos.
- Si el usuario fue desactivado, sus próximos intentos de inicio de sesión serán rechazados con `LoginResult.UsuarioInactivo`.

**Flujo Principal:**
1. El administrador accede a la sección "Gestión de Usuarios".
2. El sistema muestra la lista de usuarios con un toggle de estado para cada uno.
3. El administrador pulsa el toggle del usuario deseado.
4. El sistema invoca la actualización del estado en la base de datos.
5. El sistema actualiza visualmente el toggle.

**Flujos Alternativos:**

*1: Error al actualizar el estado*
Si ocurre un error al guardar el cambio, el sistema revierte el toggle a su estado anterior y muestra el mensaje de error correspondiente.

---

### CU004: Consultar usuarios

**Actor Principal:** Administrador

**Descripción:** El administrador consulta la lista de usuarios registrados en el sistema, viendo su correo y estado (activo/inactivo).

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.

**Postcondiciones:**
- El sistema muestra la lista de usuarios.

**Flujo Principal:**
1. El administrador accede a la sección "Gestión de Usuarios".
2. El sistema muestra los primeros 4 usuarios en el panel izquierdo y un enlace "Ver N más..." si existen más usuarios.
3. El administrador puede expandir o contraer la lista pulsando el enlace.

**Flujos Alternativos:**

*1: Lista vacía*
Si no existen usuarios cargados, el sistema muestra el panel sin elementos.

---

### CU005: Asignar roles a un usuario

**Actor Principal:** Administrador

**Descripción:** El administrador asigna o quita roles a un usuario. Un usuario puede pertenecer a uno o más roles y hereda todos los permisos de los roles asignados.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.
- Debe existir el usuario y al menos un rol en el sistema.

**Postcondiciones:**
- La asignación entre el usuario y los roles queda actualizada en la base de datos.
- El usuario hereda los permisos de los nuevos roles en su próxima sesión.

**Flujo Principal:**
1. El administrador selecciona un usuario y abre el formulario "Editar usuario".
2. El sistema muestra el grilla con los roles actualmente asignados y un combo con los roles aún no asignados.
3. Para asignar: el administrador selecciona un rol del combo y pulsa "Agregar rol".
4. Para quitar: el administrador hace doble clic sobre un rol de la grilla y confirma la acción.
5. El sistema persiste la modificación y refresca la grilla y el combo.

**Flujos Alternativos:**

*1: Rol no seleccionado*
Si el administrador pulsa "Agregar rol" sin haber seleccionado uno, el sistema muestra el aviso "Elegí un rol de la lista".

*2: Error del sistema*
Si la operación falla, el sistema muestra el mensaje "Error al agregar el rol" o "No se pudo quitar el rol" según corresponda.

---

### CU006: Gestionar permisos de un rol

**Actor Principal:** Administrador

**Descripción:** El administrador asigna o revoca permisos a un rol mediante una matriz de toggles organizada por módulo del sistema (Proyectos, Inventario, Empleados, Clientes, Proveedores, Informes de compra, Logs, Configuración, Usuarios). El rol **Administrador** es protegido y se muestra en modo solo lectura.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.
- Debe existir al menos un rol en el sistema.

**Postcondiciones:**
- Los permisos asignados al rol quedan actualizados en la base de datos.
- Los usuarios que pertenezcan al rol modificado verán reflejados los cambios en su próxima sesión.

**Flujo Principal:**
1. El administrador accede a la sección "Gestión de Usuarios".
2. El sistema muestra la lista de roles existentes como botones (chips) en la sección derecha.
3. El administrador selecciona un rol.
4. El sistema muestra una matriz con los módulos del sistema en filas y dos columnas de toggles (VER / GESTIONAR), reflejando los permisos actualmente asignados.
5. El administrador activa o desactiva los toggles para asignar o quitar permisos.
6. Por cada cambio, el sistema persiste la asignación inmediatamente (sin necesidad de "Guardar").

**Flujos Alternativos:**

*1: Rol protegido (Administrador)*
Si el administrador selecciona el rol "Administrador", el sistema muestra los permisos pero deshabilita todos los toggles, indicando que es un rol protegido en modo solo lectura.

*2: Error al actualizar*
Si la operación falla, el sistema revierte el toggle a su estado anterior y muestra el mensaje "No se pudo actualizar los permisos".

---

### CU007: Crear rol

**Actor Principal:** Administrador

**Descripción:** El administrador crea un nuevo rol con un nombre. Inicialmente el rol no tiene permisos asignados; deben asignarse posteriormente con el CU006.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.

**Postcondiciones:**
- El rol queda creado en la base de datos y disponible para ser asignado a usuarios y para recibir permisos.

**Flujo Principal:**
1. El administrador ingresa el nombre del rol en el campo "Nuevo rol" de la sección "Roles".
2. El administrador pulsa el botón "+ Crear rol".
3. El sistema valida que el nombre no esté vacío.
4. El sistema crea el rol y refresca la lista de roles.

**Flujos Alternativos:**

*1: Nombre vacío*
Si el campo está vacío, el sistema no realiza ninguna acción.

*2: Error al crear*
Si ocurre un error, el sistema muestra el mensaje "Error al crear el rol".

---

### CU008: Crear permiso (acceso)

**Actor Principal:** Administrador

**Descripción:** El administrador define una nueva entrada de permiso (Acceso) vinculada a un valor del enum `TipoPermiso`. Los permisos creados quedan disponibles para ser asignados a roles desde la matriz de permisos o desde el formulario de accesos.

**Precondiciones:**
- El administrador debe estar autenticado y poseer el permiso `GESTIONAR_USUARIOS`.

**Postcondiciones:**
- El nuevo permiso queda registrado en la base de datos.

**Flujo Principal:**
1. El administrador abre el formulario "Accesos".
2. El administrador ingresa un nombre descriptivo y selecciona el `TipoPermiso` del combo.
3. El administrador pulsa "Guardar permiso".
4. El sistema valida que el nombre no esté vacío.
5. El sistema crea el permiso y refresca la lista.

**Flujos Alternativos:**

*1: Nombre vacío*
El sistema muestra el aviso "El nombre es obligatorio" y mantiene el foco en el campo.

*2: Error al crear*
El sistema muestra el mensaje "Error al crear el permiso".

---

## Autenticación

### CU009: Iniciar sesión

**Actor Principal:** Usuario (cualquier rol)

**Descripción:** El usuario accede al sistema mediante autenticación con mail y contraseña. La validación se realiza comparando la contraseña ingresada contra el hash BCrypt almacenado, con compatibilidad retroactiva para hashes SHA-256 generados por versiones previas.

**Precondiciones:**
- El usuario debe tener una cuenta creada por el administrador.
- La cuenta debe estar activa (`IsActive = true`).

**Postcondiciones:**
- El sistema crea una sesión para el usuario, cargando sus permisos heredados de sus roles.
- El evento de inicio de sesión queda registrado en la bitácora.
- El sistema redirige al usuario a la pantalla principal correspondiente a sus permisos.

**Flujo Principal:**
1. El usuario abre la pantalla de inicio de sesión.
2. El usuario ingresa su mail y contraseña.
3. El usuario pulsa "Iniciar sesión".
4. El sistema busca al usuario por mail.
5. El sistema valida que el usuario esté activo.
6. El sistema verifica la contraseña comparándola contra el hash BCrypt almacenado.
7. El sistema crea la sesión, carga los permisos del usuario y registra el evento en la bitácora.
8. El sistema muestra la pantalla principal.

**Flujos Alternativos:**

*1: Credenciales inválidas (`LoginResult.CredencialesInvalidas`)*
Si el mail no existe o la contraseña no coincide, el sistema muestra el mensaje "Mail o contraseña incorrectos" sin distinguir cuál de los dos falló, para no revelar la existencia de cuentas.

*2: Usuario inactivo (`LoginResult.UsuarioInactivo`)*
Si el usuario existe pero su flag `IsActive` es falso, el sistema muestra un mensaje específico indicando que la cuenta está deshabilitada y que debe contactar al administrador.

*3: Campos vacíos*
Si el mail o la contraseña están vacíos, el sistema rechaza el intento con `LoginResult.CredencialesInvalidas` sin consultar la base de datos.

---

### CU010: Recuperar contraseña

**Actor Principal:** Usuario (cualquier rol)
**Actor Secundario:** Sistema (envío SMTP)

**Descripción:** El usuario olvidó su contraseña y solicita un código de un solo uso (OTP) que le es enviado por correo electrónico. Con el código válido puede establecer una nueva contraseña.

**Precondiciones:**
- El usuario debe tener una cuenta activa en el sistema con un mail registrado.
- El servidor SMTP debe estar accesible (parametrizado en `App.config`).

**Postcondiciones:**
- Un código OTP de 6 dígitos queda registrado en el usuario junto con su fecha de expiración (15 minutos desde la generación).
- El sistema envía el OTP al mail del usuario.
- Una vez que el usuario establece la nueva contraseña, el OTP se invalida (se borra de la base de datos).

**Flujo Principal:**

*Paso 1 — Solicitar código:*
1. El usuario abre la pantalla de recuperación de contraseña desde el formulario de login.
2. El usuario ingresa su mail y pulsa "Enviar código".
3. El sistema busca al usuario y verifica que esté activo.
4. El sistema genera un OTP de 6 dígitos aleatorio con expiración a 15 minutos.
5. El sistema persiste el OTP y la fecha de expiración asociados al usuario.
6. El sistema envía el OTP por correo electrónico (asunto: "Código de recuperación de contraseña").
7. El sistema muestra el segundo paso del formulario.

*Paso 2 — Establecer nueva contraseña:*
8. El usuario ingresa el OTP recibido, la nueva contraseña y la confirmación.
9. El usuario pulsa "Cambiar contraseña".
10. El sistema valida que el OTP coincida con el almacenado y no haya expirado.
11. El sistema hashea la nueva contraseña con BCrypt y la persiste, limpiando el OTP y su expiración.
12. El sistema muestra el mensaje "Contraseña cambiada exitosamente. Ya puede iniciar sesión." y cierra la pantalla.

**Flujos Alternativos:**

*1: Mail inexistente o usuario inactivo*
Si el mail no corresponde a un usuario activo, el sistema muestra el mensaje "No se encontró una cuenta activa con ese correo" y mantiene el primer paso.

*2: Campos incompletos en el paso 2*
Si el OTP, la nueva contraseña o la confirmación están vacíos, el sistema muestra el aviso "Complete todos los campos".

*3: Contraseñas no coinciden*
Si la nueva contraseña no coincide con la confirmación, el sistema muestra "Las contraseñas no coinciden", limpia el campo de confirmación y posiciona el foco en él.

*4: Contraseña muy corta*
Si la nueva contraseña tiene menos de 6 caracteres, el sistema muestra "La contraseña debe tener al menos 6 caracteres".

*5: OTP incorrecto o expirado*
Si el OTP no coincide o ya expiró, el sistema muestra "El código es incorrecto o ha expirado", limpia el campo del OTP y solicita un nuevo intento (el usuario puede volver al paso 1 si lo necesita).

*6: Error al enviar el correo*
Si el envío SMTP falla, el sistema muestra el mensaje "Error al enviar el código" con el detalle de la excepción. El usuario puede reintentar.

---

## Internacionalización

### CU011: Cambiar idioma

**Actor Principal:** Usuario autenticado (cualquier rol)

**Descripción:** El usuario cambia el idioma de la interfaz del sistema. La preferencia se persiste a nivel del usuario y se aplica como `CultureCode` en la próxima sesión (o tras un reinicio, si se está modificando al propio usuario logueado).

**Idiomas soportados:** Español Argentina (`es-AR`) e Inglés Estados Unidos (`en-US`).

**Precondiciones:**
- El usuario debe estar autenticado.

**Postcondiciones:**
- El campo `Idioma` del usuario queda actualizado en la base de datos (`es` o `en`).
- Si el usuario modificado es el mismo que está autenticado, el `CultureCode` queda persistido en la configuración de la aplicación y el sistema se reinicia para aplicarlo.

**Flujo Principal:**
1. El usuario (o el administrador modificando otra cuenta) abre el formulario "Editar usuario".
2. El usuario selecciona el idioma deseado del combo "Idioma" ("Español" o "Inglés").
3. El usuario pulsa "Editar usuario".
4. El sistema actualiza el campo `Idioma` del usuario en la base de datos.
5. Si el usuario actualizado coincide con el logueado, el sistema:
   - Persiste `Properties.Settings.Default.CultureCode = "es-AR"` o `"en-US"` según corresponda.
   - Reinicia la aplicación mediante `Application.Restart()`.
6. En caso contrario, el sistema muestra el mensaje de confirmación y cierra el formulario.

**Flujos Alternativos:**

*1: Error al persistir*
Si ocurre un error al actualizar el usuario, el sistema muestra "No se pudo actualizar el usuario" y mantiene abierto el formulario.

---

## Configuración del sistema

### CU012: Configurar parámetros (reescritura)

> **Nota:** Este CU reemplaza al "CU002: Configurar parámetros" original del documento principal, que describía el cambio de tema y cierre de sesión automático. El sistema actual implementa una configuración distinta, vinculada a los cálculos del informe de monto de obra.

**Actor Principal:** Usuario con permiso `CONFIGURAR_PARAMETROS` (típicamente Administrador o Gerente).

**Descripción:** El usuario configura los parámetros globales utilizados para calcular el monto total de un proyecto: el **margen aplicado al costo de empleados**, el **margen aplicado al costo de materiales** y la **utilidad esperada de la empresa**. Los tres valores se ingresan como porcentaje y se almacenan internamente como decimal (por ejemplo, 20% se persiste como `0.20`).

**Precondiciones:**
- El usuario debe estar autenticado y poseer el permiso `CONFIGURAR_PARAMETROS`.

**Postcondiciones:**
- Los nuevos valores quedan persistidos en la entidad `Parametros` con su fecha de modificación actualizada (`UltimaModificacion`) y el usuario que la realizó (`ModificadoPor`).
- Los próximos cálculos de monto del proyecto usarán los nuevos parámetros.

**Flujo Principal:**
1. El usuario accede a la sección "Configurar parámetros".
2. El sistema carga los valores actuales y los muestra como porcentaje (multiplicados por 100) en tres campos numéricos: "Margen empleados", "Margen materiales" y "Utilidad empresa".
3. El usuario modifica los valores deseados.
4. El usuario pulsa "Guardar".
5. El sistema convierte los porcentajes a decimal (divide por 100) y persiste los `Parametros`.
6. El sistema muestra el mensaje "Parámetros guardados correctamente" en color verde.

**Flujos Alternativos:**

*1: Error al cargar*
Si no se pueden obtener los parámetros desde la base de datos, el sistema muestra "Error al acceder a la base de datos" y deja los campos en blanco.

*2: Error al guardar*
Si ocurre un error durante la persistencia, el sistema muestra el mensaje "Error al guardar" en color rojo.

---

## Observaciones de cobertura

- Los CUs **CU001 a CU008** quedan agrupados bajo el módulo **Gestión de Usuarios** del documento principal (a sumar como nuevo módulo en la sección "3. Casos de uso").
- El CU009 "Iniciar sesión" debería **reemplazar** al CU001 actual del apartado "Usuarios" del documento principal (que no contempla los resultados `UsuarioInactivo` ni la verificación BCrypt).
- Los CUs **CU010 y CU011** son completamente nuevos y no estaban en el documento original (cubren los RF 11 "Autenticación segura" y RF 12 "Soporte multi-idioma" respectivamente).
- El CU012 **reescribe** el CU002 actual del apartado "Usuarios" del documento principal, alineándolo al RF 14 ("Configuración de parámetros del sistema") y a la implementación efectiva del `ConfigurarParametrosControl`.
