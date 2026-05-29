# Casos de Uso — Sistema (reescritos)

Este archivo reescribe los casos de uso de la sección **"Sistema"** del documento original, reemplazando los CUs genéricos *"Encriptar contraseña"* / *"Desencriptar contraseña"* por los CUs reales que efectivamente implementa el sistema: **hash de contraseñas con BCrypt** (no encriptación reversible), **verificación de contraseñas** y **gestión del código OTP de recuperación**.

> **Nota terminológica importante:** el sistema **no encripta ni desencripta contraseñas**. Aplica una **función de hash unidireccional (BCrypt, *work factor* 12)**, lo que implica que las contraseñas almacenadas **no pueden ser recuperadas** ni siquiera por el propio sistema. La verificación se realiza re-hasheando la contraseña ingresada y comparándola contra el hash almacenado. Esta distinción es fundamental para el modelo de seguridad y debe mantenerse a lo largo del documento.

---

## CU – Hashear contraseña

**Actor Principal:** Sistema (invocado internamente por otros casos de uso: *Crear usuario*, *Modificar usuario*, *Recuperar contraseña*).

**Descripción:** El sistema toma una contraseña en texto plano y la transforma en un hash criptográfico utilizando el algoritmo BCrypt con *work factor* 12, para luego almacenarlo en la base de datos. El hash incluye internamente un salt aleatorio único por contraseña, por lo que dos contraseñas iguales producen hashes diferentes.

**Precondiciones:**
- Se recibe una contraseña en texto plano desde un caso de uso invocador.

**Postcondiciones:**
- Se obtiene una cadena con el hash BCrypt, lista para persistirse en el campo `Contraseña` del usuario.
- La contraseña original en texto plano **no se almacena** y se descarta de la memoria al finalizar el flujo.

**Flujo Principal:**
1. El sistema recibe la contraseña en texto plano desde el caso de uso invocador.
2. El sistema invoca a `PasswordHasher.Hash(plainText)`.
3. El módulo BCrypt genera un salt aleatorio y aplica el algoritmo con *work factor* **12** (aproximadamente 250 ms por hash, balance entre seguridad y experiencia de usuario).
4. El sistema devuelve la cadena resultante (formato `$2a$...`) al caso de uso invocador.

**Flujos Alternativos:**
- **1a. La contraseña recibida es nula o vacía:** el caso de uso invocador es responsable de la validación previa; este CU asume que ya fue validada.

---

## CU – Verificar contraseña

**Actor Principal:** Sistema (invocado internamente por: *Iniciar sesión*).

**Descripción:** El sistema compara una contraseña ingresada por el usuario contra el hash almacenado en la base de datos. Soporta dos formatos: el formato actual **BCrypt** (hashes que comienzan con `$2a$`, `$2b$` o `$2y$`) y, por compatibilidad retroactiva, el formato legacy **SHA-256 en Base64** (utilizado por usuarios creados en versiones anteriores del sistema).

**Precondiciones:**
- Se recibe un hash almacenado y una contraseña en texto plano.

**Postcondiciones:**
- Se devuelve un valor booleano: **verdadero** si la contraseña coincide con el hash, **falso** en caso contrario.
- No se modifica información en la base de datos.

**Flujo Principal:**
1. El sistema recibe el hash almacenado y la contraseña ingresada.
2. El sistema valida que ambos parámetros no sean nulos ni vacíos; si lo son, devuelve **falso**.
3. El sistema inspecciona el prefijo del hash:
   - Si comienza con `$2`, lo trata como hash **BCrypt** e invoca `BCrypt.Verify` para comparar.
   - Si no, lo trata como hash **SHA-256 legacy**, re-hashea la contraseña ingresada con SHA-256, la codifica en Base64 y compara cadenas.
4. El sistema devuelve el resultado de la comparación al caso de uso invocador.

**Flujos Alternativos:**
- **2a. Hash o contraseña vacíos:** se devuelve **falso** sin lanzar excepción.
- **3a. Hash con formato no reconocido:** se trata como SHA-256 legacy; si la comparación falla, se devuelve **falso**.

**Observación:** BCrypt no permite *desencriptar* el hash. La verificación se realiza siempre re-hasheando la contraseña ingresada (BCrypt extrae internamente el salt del hash almacenado para repetir el proceso). El sistema **nunca expone, registra ni transmite la contraseña en texto plano** más allá del flujo de verificación.

---

## CU – Generar y enviar código OTP

**Actor Principal:** Sistema (invocado internamente por: *Recuperar contraseña* — paso 1).

**Descripción:** El sistema genera un código numérico de un solo uso (OTP) de 6 dígitos, lo asocia al usuario con una fecha de expiración de 15 minutos y lo envía a la dirección de correo electrónico del usuario mediante SMTP.

**Precondiciones:**
- Se recibe una dirección de correo electrónico desde el caso de uso *Recuperar contraseña*.
- El servicio SMTP se encuentra configurado con host, puerto, usuario y contraseña en el archivo de configuración de la aplicación.

**Postcondiciones:**
- Si el correo corresponde a un usuario activo, se persiste el OTP y su fecha de expiración en los campos `Otp` y `OtpExpiry` del usuario, y se envía un correo con el código.
- Si el correo no corresponde a ningún usuario activo, no se realiza ninguna acción y se devuelve **falso** (sin revelar al solicitante si el correo existe).

**Flujo Principal:**
1. El sistema busca el usuario por correo electrónico en la base.
2. El sistema valida que el usuario exista y esté activo (`IsActive = true`).
3. El sistema genera un número aleatorio entre 100000 y 999999 (6 dígitos).
4. El sistema calcula la fecha de expiración como **DateTime.Now + 15 minutos**.
5. El sistema persiste `Otp` y `OtpExpiry` en el registro del usuario.
6. El sistema envía un correo mediante SMTP con asunto *"Código de recuperación de contraseña"* y cuerpo que incluye el código y su fecha/hora de expiración.
7. El sistema devuelve **verdadero** al caso de uso invocador.

**Flujos Alternativos:**
- **2a. Correo no corresponde a un usuario activo:** el sistema devuelve **falso** sin enviar correo ni modificar la base. La pantalla invocadora muestra un mensaje genérico (*"Si el mail es válido, recibirás un código"*) para no filtrar información de usuarios existentes.
- **6a. Falla la conexión SMTP:** el caso de uso invocador captura la excepción y muestra al usuario un mensaje de error al usuario, sin exponer detalles del servidor.

---

## CU – Validar código OTP

**Actor Principal:** Sistema (invocado internamente por: *Recuperar contraseña* — paso 2).

**Descripción:** El sistema verifica que un código OTP ingresado por el usuario sea válido para el correo asociado: que exista, que coincida exactamente con el almacenado y que no haya expirado.

**Precondiciones:**
- Se recibe un correo electrónico y un código OTP desde el caso de uso *Recuperar contraseña*.

**Postcondiciones:**
- Se devuelve un valor booleano: **verdadero** si el OTP es válido y vigente, **falso** en caso contrario.
- No se modifica información en la base de datos.

**Flujo Principal:**
1. El sistema busca el usuario por correo electrónico.
2. El sistema valida que el usuario exista y posea un OTP y una fecha de expiración registrados.
3. El sistema verifica que la fecha actual sea anterior o igual a la fecha de expiración.
4. El sistema compara el OTP ingresado (con espacios al inicio/fin removidos) contra el almacenado, usando comparación binaria estricta (`StringComparison.Ordinal`).
5. El sistema devuelve el resultado al caso de uso invocador.

**Flujos Alternativos:**
- **2a. El usuario no posee OTP o fecha de expiración:** se devuelve **falso**.
- **3a. El OTP expiró:** se devuelve **falso**. El usuario debe solicitar un nuevo OTP desde el inicio del flujo de recuperación.
- **4a. El OTP ingresado no coincide:** se devuelve **falso**.

**Observación:** la limpieza del OTP (es decir, dejarlo nulo en la base) **no** ocurre en este CU, sino en el CU *Cambiar contraseña por OTP* una vez que el cambio se aplica con éxito. Esto permite reintentos al usuario en caso de error tipográfico.

---

## CU – Cambiar contraseña por OTP

**Actor Principal:** Sistema (invocado internamente por: *Recuperar contraseña* — paso 2 final).

**Descripción:** Tras validar el OTP correctamente, el sistema reemplaza la contraseña del usuario por una nueva (hasheada con BCrypt) y limpia el OTP para evitar que pueda reutilizarse.

**Precondiciones:**
- El OTP ya fue validado correctamente para el correo dado (ver CU *Validar código OTP*).
- Se recibe el correo y la nueva contraseña en texto plano.

**Postcondiciones:**
- La contraseña del usuario queda reemplazada por el nuevo hash BCrypt.
- Los campos `Otp` y `OtpExpiry` quedan en `null`, invalidando el código.

**Flujo Principal:**
1. El sistema busca el usuario por correo.
2. El sistema invoca al CU *Hashear contraseña* con la nueva contraseña en texto plano.
3. El sistema actualiza el campo `Contraseña` con el nuevo hash, y los campos `Otp` y `OtpExpiry` con `null`.
4. El sistema persiste los cambios en la base de datos.

**Flujos Alternativos:**
- **1a. El usuario no existe:** el sistema lanza una excepción `InvalidOperationException("Usuario no encontrado.")`. El caso de uso invocador la captura y muestra un mensaje de error al usuario.

---

## Observaciones de cobertura

### CUs que se eliminan / reemplazan

- **"Encriptar contraseña"** del documento original → **se elimina**. La terminología es incorrecta; reemplazado por **CU – Hashear contraseña**.
- **"Desencriptar contraseña"** del documento original → **se elimina** (no existe tal operación en el sistema). Reemplazado por **CU – Verificar contraseña**, que es lo que efectivamente ocurre durante el login.

### CUs nuevos

- **CU – Generar y enviar código OTP**
- **CU – Validar código OTP**
- **CU – Cambiar contraseña por OTP**

Estos tres CUs documentan el flujo de recuperación de contraseña, una funcionalidad que existe en el sistema pero que no estaba documentada como un caso de uso de Sistema (el CU de cara al usuario *Recuperar contraseña* está en el módulo Usuarios, este lado documenta el lado interno del sistema).

### Compatibilidad retroactiva (SHA-256 legacy)

Se documenta explícitamente que el verificador acepta hashes legacy SHA-256 (sin prefijo `$2`). Esto está incluido en el flujo del **CU – Verificar contraseña** y es relevante para auditoría: si en producción aún quedaran usuarios pre-BCrypt, sus credenciales seguirían funcionando sin migración manual. Una migración progresiva podría implementarse re-hasheando al hacer login exitoso con SHA-256 (no implementado actualmente).

### Ubicación sugerida en el documento principal

- Sección **"8.7 Casos de Uso – Sistema"** (o donde estén los CUs de Sistema en tu documento).
- **Reemplazá** los CUs *"Encriptar contraseña"* y *"Desencriptar contraseña"* por los cinco CUs anteriores.
- Si el documento ya tiene un CU genérico de "Recuperar contraseña" en Sistema, conviene también reemplazarlo: el CU de cara al usuario va en el módulo Usuarios; los CUs de soporte van en Sistema.
