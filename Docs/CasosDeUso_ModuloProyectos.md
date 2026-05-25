# Casos de Uso — Módulo Proyectos (agregados)

Este archivo agrupa los casos de uso del módulo **Proyectos** que no se encuentran cubiertos en la documentación original o que reflejan funcionalidad incorporada posteriormente.

---

## CU – Visualizar análisis de costos del proyecto

**Actor Principal:** Usuario autenticado con permiso `VER_PROYECTOS`.

**Descripción:** El usuario consulta dos gráficos comparativos sobre la evolución de los costos del proyecto: el gasto en compras de materiales y el costo total acumulado (materiales + sueldos de empleados asignados). Puede agrupar la información por días, meses o años.

**Precondiciones:**
- El usuario está autenticado en el sistema.
- El usuario tiene asignado el permiso `VER_PROYECTOS`.
- Existe al menos un proyecto creado y el usuario está visualizando su detalle.

**Postcondiciones:**
- Se muestran los gráficos de análisis del proyecto en una ventana modal.
- No se modifica información del proyecto ni de sus recursos asignados.

**Flujo Principal:**
1. El usuario, posicionado en la pantalla *Detalle del Proyecto*, hace clic en el enlace **"Ver análisis"**.
2. El sistema abre la ventana *Análisis — {descripción del proyecto}*.
3. El sistema obtiene los materiales y empleados asignados al proyecto desde la base de datos.
4. El sistema aplica por defecto el filtro de agrupación **Meses**.
5. El sistema construye y muestra el gráfico **"Análisis de compras"** (columnas), que representa la sumatoria de `costo unitario × cantidad` de los materiales asignados, agrupada por la fecha de ingreso del material según el período seleccionado.
6. El sistema construye y muestra el gráfico **"Costo del proyecto"** (línea), que representa por período la suma del costo de los materiales más los sueldos de los empleados asignados.
7. El usuario puede cambiar el filtro de agrupación seleccionando **Días**, **Meses** o **Años**, y el sistema regenera ambos gráficos en tiempo real.
8. El usuario cierra la ventana de análisis y regresa al detalle del proyecto.

**Flujos Alternativos:**
- **3a. El proyecto no tiene materiales ni empleados asignados:** el sistema muestra los gráficos vacíos sin generar errores.
- **3b. Error al acceder a la base de datos:** el sistema asume listas vacías y muestra los gráficos sin datos, evitando que la pantalla se rompa.
- **7a. El usuario cambia el filtro antes de que finalice la carga inicial:** el sistema espera a tener los datos cargados y luego actualiza los gráficos con el filtro elegido.

---

## Observaciones de cobertura

- **CU nuevo.** No existe equivalente en la documentación original.
- **Ubicación sugerida en el documento principal:** dentro de la sección **"8.6 Casos de Uso – Proyectos"**, como último caso de uso del módulo (después del CU de *Eliminar Proyecto* / *Modificar Proyecto*, según el orden actual del documento).
- **Pantalla de origen:** la acción se dispara desde el enlace **"Ver análisis"** del control `DetalleProyectoControl`, por lo que el CU asume que el actor ya consultó previamente el detalle del proyecto (esto puede mencionarse explícitamente en el documento si se considera necesario, encadenando con el CU *Consultar Proyecto*).
- **Aclaración terminológica:** el formulario muestra **costos** del proyecto (egresos en materiales y sueldos), no ventas ni ganancias. La utilidad estimada del proyecto se visualiza en el propio `DetalleProyectoControl` mediante los totales recalculados, no en esta ventana.
