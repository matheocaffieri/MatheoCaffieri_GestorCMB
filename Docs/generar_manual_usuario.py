# -*- coding: utf-8 -*-
"""
Genera el Manual de Usuario de Gestor CMB en formato Word (.docx).
Replica la estructura y el estilo del manual de RedAceite usado como template:
portada con tabla de datos, indice automatico, encabezados azules, tablas con
cabecera azul, placeholders de imagenes y pie de pagina con numero de pagina.

Basado en la documentacion del proyecto (Docs/CasosDeUso_*, Patrones_Diseno.md)
y en el codigo de la aplicacion WinForms.
"""
import os
from docx import Document
from docx.shared import Pt, RGBColor, Cm, Inches
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.enum.section import WD_SECTION
from docx.oxml.ns import qn
from docx.oxml import OxmlElement

# ----------------------------------------------------------------------------
# Paleta y constantes de estilo
# ----------------------------------------------------------------------------
# Paleta institucional UAI (bordó / vino tinto)
AZUL_H1   = RGBColor(0x6E, 0x18, 0x2A)   # bordó intenso para titulos de nivel 1
AZUL_H2   = RGBColor(0x9B, 0x2D, 0x3E)   # bordó medio para subtitulos
AZUL_HDR  = "6E182A"                       # relleno cabecera de tablas (bordó)
GRIS_PH   = RGBColor(0x80, 0x80, 0x80)   # gris para placeholders
GRIS_TXT  = RGBColor(0x59, 0x59, 0x59)
NEGRO     = RGBColor(0x00, 0x00, 0x00)

FUENTE = "Calibri"

doc = Document()

# Estilo base
normal = doc.styles["Normal"]
normal.font.name = FUENTE
normal.font.size = Pt(11)
normal.paragraph_format.space_after = Pt(6)
normal.paragraph_format.line_spacing = 1.15

# Margenes
for section in doc.sections:
    section.top_margin = Cm(2.5)
    section.bottom_margin = Cm(2.5)
    section.left_margin = Cm(2.5)
    section.right_margin = Cm(2.5)

# ----------------------------------------------------------------------------
# Helpers de bajo nivel (OXML)
# ----------------------------------------------------------------------------

def _set_cell_bg(cell, hex_color):
    tcPr = cell._tc.get_or_add_tcPr()
    shd = OxmlElement("w:shd")
    shd.set(qn("w:val"), "clear")
    shd.set(qn("w:color"), "auto")
    shd.set(qn("w:fill"), hex_color)
    tcPr.append(shd)


def _set_table_borders(table, color="BFBFBF", sz="4"):
    tbl = table._tbl
    tblPr = tbl.tblPr
    borders = OxmlElement("w:tblBorders")
    for edge in ("top", "left", "bottom", "right", "insideH", "insideV"):
        el = OxmlElement(f"w:{edge}")
        el.set(qn("w:val"), "single")
        el.set(qn("w:sz"), sz)
        el.set(qn("w:space"), "0")
        el.set(qn("w:color"), color)
        borders.append(el)
    tblPr.append(borders)


def _shade_paragraph(paragraph, hex_color):
    pPr = paragraph._p.get_or_add_pPr()
    shd = OxmlElement("w:shd")
    shd.set(qn("w:val"), "clear")
    shd.set(qn("w:color"), "auto")
    shd.set(qn("w:fill"), hex_color)
    pPr.append(shd)


def _box_paragraph(paragraph, color="AAAAAA", sz="6"):
    """Dibuja un borde alrededor de un parrafo (para placeholders de imagen)."""
    pPr = paragraph._p.get_or_add_pPr()
    pbdr = OxmlElement("w:pBdr")
    for edge in ("top", "left", "bottom", "right"):
        el = OxmlElement(f"w:{edge}")
        el.set(qn("w:val"), "dashed")
        el.set(qn("w:sz"), sz)
        el.set(qn("w:space"), "8")
        el.set(qn("w:color"), color)
        pbdr.append(el)
    pPr.append(pbdr)


def _add_field(paragraph, instr):
    """Inserta un campo de Word (p. ej. PAGE, TOC)."""
    run = paragraph.add_run()
    fldBegin = OxmlElement("w:fldChar")
    fldBegin.set(qn("w:fldCharType"), "begin")
    run._r.append(fldBegin)

    instrText = OxmlElement("w:instrText")
    instrText.set(qn("xml:space"), "preserve")
    instrText.text = instr
    run._r.append(instrText)

    fldSep = OxmlElement("w:fldChar")
    fldSep.set(qn("w:fldCharType"), "separate")
    run._r.append(fldSep)

    fldEnd = OxmlElement("w:fldChar")
    fldEnd.set(qn("w:fldCharType"), "end")
    run._r.append(fldEnd)


# ----------------------------------------------------------------------------
# Helpers de contenido
# ----------------------------------------------------------------------------

def h1(text):
    p = doc.add_heading(level=1)
    run = p.add_run(text)
    run.font.color.rgb = AZUL_H1
    run.font.name = FUENTE
    run.font.size = Pt(18)
    p.paragraph_format.space_before = Pt(16)
    p.paragraph_format.space_after = Pt(8)
    return p


def h2(text):
    p = doc.add_heading(level=2)
    run = p.add_run(text)
    run.font.color.rgb = AZUL_H2
    run.font.name = FUENTE
    run.font.size = Pt(14)
    p.paragraph_format.space_before = Pt(12)
    p.paragraph_format.space_after = Pt(4)
    return p


def h3(text):
    p = doc.add_heading(level=3)
    run = p.add_run(text)
    run.font.color.rgb = AZUL_H2
    run.font.name = FUENTE
    run.font.size = Pt(12)
    run.font.bold = True
    p.paragraph_format.space_before = Pt(8)
    p.paragraph_format.space_after = Pt(2)
    return p


def para(text="", bold=False, italic=False, color=None, size=11, align=None, space_after=6):
    p = doc.add_paragraph()
    if text:
        run = p.add_run(text)
        run.bold = bold
        run.italic = italic
        run.font.size = Pt(size)
        if color is not None:
            run.font.color.rgb = color
    if align is not None:
        p.alignment = align
    p.paragraph_format.space_after = Pt(space_after)
    return p


def rich(parts, align=None, space_after=6):
    """parts: lista de (texto, {bold,italic,color,size})."""
    p = doc.add_paragraph()
    for text, fmt in parts:
        run = p.add_run(text)
        run.bold = fmt.get("bold", False)
        run.italic = fmt.get("italic", False)
        run.font.size = Pt(fmt.get("size", 11))
        if "color" in fmt:
            run.font.color.rgb = fmt["color"]
    if align is not None:
        p.alignment = align
    p.paragraph_format.space_after = Pt(space_after)
    return p


def bullets(items):
    for it in items:
        if isinstance(it, tuple):
            label, rest = it
            p = doc.add_paragraph(style="List Bullet")
            r = p.add_run(label)
            r.bold = True
            p.add_run(rest)
        else:
            p = doc.add_paragraph(it, style="List Bullet")
        p.paragraph_format.space_after = Pt(2)


def steps(items):
    for it in items:
        p = doc.add_paragraph(it, style="List Number")
        p.paragraph_format.space_after = Pt(2)


def image_placeholder(caption):
    p = doc.add_paragraph()
    p.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p.paragraph_format.space_before = Pt(8)
    p.paragraph_format.space_after = Pt(2)
    _box_paragraph(p)
    run = p.add_run(f"🖼  [ IMAGEN — {caption} ]")
    run.italic = True
    run.font.size = Pt(10)
    run.font.color.rgb = GRIS_PH
    # linea de instruccion debajo
    sub = doc.add_paragraph()
    sub.alignment = WD_ALIGN_PARAGRAPH.CENTER
    sub.paragraph_format.space_after = Pt(8)
    r = sub.add_run("(Reemplazar este recuadro por una captura de pantalla)")
    r.italic = True
    r.font.size = Pt(8)
    r.font.color.rgb = GRIS_PH


def make_table(headers, rows, widths=None):
    table = doc.add_table(rows=1, cols=len(headers))
    table.alignment = WD_TABLE_ALIGNMENT.CENTER
    table.autofit = True
    _set_table_borders(table)
    # cabecera
    hdr = table.rows[0].cells
    for i, htext in enumerate(headers):
        hdr[i].text = ""
        p = hdr[i].paragraphs[0]
        run = p.add_run(htext)
        run.bold = True
        run.font.color.rgb = RGBColor(0xFF, 0xFF, 0xFF)
        run.font.size = Pt(10)
        _set_cell_bg(hdr[i], AZUL_HDR)
    # filas
    for row in rows:
        cells = table.add_row().cells
        for i, val in enumerate(row):
            cells[i].text = ""
            p = cells[i].paragraphs[0]
            run = p.add_run(str(val))
            run.font.size = Pt(10)
    if widths:
        for row in table.rows:
            for i, w in enumerate(widths):
                row.cells[i].width = w
    doc.add_paragraph().paragraph_format.space_after = Pt(2)
    return table


def hrule():
    p = doc.add_paragraph()
    pPr = p._p.get_or_add_pPr()
    pbdr = OxmlElement("w:pBdr")
    bottom = OxmlElement("w:bottom")
    bottom.set(qn("w:val"), "single")
    bottom.set(qn("w:sz"), "6")
    bottom.set(qn("w:space"), "1")
    bottom.set(qn("w:color"), "BFBFBF")
    pbdr.append(bottom)
    pPr.append(pbdr)
    p.paragraph_format.space_after = Pt(4)


# ----------------------------------------------------------------------------
# PIE DE PAGINA con numero de pagina
# ----------------------------------------------------------------------------

def set_footer():
    section = doc.sections[0]
    footer = section.footer
    footer.is_linked_to_previous = False
    p = footer.paragraphs[0]
    p.alignment = WD_ALIGN_PARAGRAPH.CENTER
    r1 = p.add_run("Manual de Usuario — Gestor CMB     |     Página ")
    r1.font.size = Pt(9)
    r1.font.color.rgb = GRIS_TXT
    _add_field(p, "PAGE")
    # darle estilo al numero
    for run in p.runs:
        run.font.size = Pt(9)
        run.font.color.rgb = GRIS_TXT


# ============================================================================
# 0. PORTADA
# ============================================================================

def portada():
    # Logo / placeholder
    p = doc.add_paragraph()
    p.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p.paragraph_format.space_before = Pt(40)
    _box_paragraph(p)
    r = p.add_run("🖼  [ LOGO — Gestor CMB ]")
    r.italic = True
    r.font.size = Pt(11)
    r.font.color.rgb = GRIS_PH

    for _ in range(6):
        doc.add_paragraph()

    t = doc.add_paragraph()
    t.alignment = WD_ALIGN_PARAGRAPH.CENTER
    run = t.add_run("Proyecto Gestor CMB – Manual de Usuario")
    run.bold = True
    run.underline = True
    run.font.size = Pt(20)
    run.font.color.rgb = AZUL_H1

    for _ in range(10):
        doc.add_paragraph()

    # tabla de datos
    datos = [
        ("Alumno:", "Mateo Nicolás Caffieri"),
        ("Proyecto:", "Gestor CMB - Manual de Usuario"),
        ("Legajo:", "[Completar legajo]"),
        ("Email alumno:", "tatecaffieri@gmail.com"),
        ("Año:", "2026"),
        ("Docentes:", "[Completar nombres de docentes]"),
    ]
    table = doc.add_table(rows=0, cols=2)
    table.alignment = WD_TABLE_ALIGNMENT.CENTER
    _set_table_borders(table, color="808080")
    for k, v in datos:
        cells = table.add_row().cells
        cells[0].text = ""
        pk = cells[0].paragraphs[0]
        rk = pk.add_run(k)
        rk.font.size = Pt(11)
        cells[1].text = ""
        pv = cells[1].paragraphs[0]
        rv = pv.add_run(v)
        rv.font.size = Pt(11)
        cells[0].width = Cm(4.5)
        cells[1].width = Cm(9.5)

    doc.add_page_break()


# ============================================================================
# INDICE
# ============================================================================

def indice():
    p = doc.add_heading(level=1)
    run = p.add_run("Índice")
    run.font.color.rgb = AZUL_H1
    run.font.size = Pt(18)

    para("Para actualizar el índice: hacé clic derecho sobre él y elegí "
         "«Actualizar campos» (o seleccionalo y presioná F9).",
         italic=True, color=GRIS_TXT, size=9)

    tocp = doc.add_paragraph()
    _add_field(tocp, 'TOC \\o "1-3" \\h \\z \\u')
    doc.add_page_break()


# ============================================================================
# CONTENIDO
# ============================================================================

def contenido():
    # ---------------------------------------------------------------- 1
    h1("1. Introducción")
    para("Gestor CMB es una aplicación de escritorio para Windows que utiliza la "
         "empresa constructora para gestionar todo el ciclo de sus obras: el "
         "registro de clientes, la administración del personal (empleados) y sus "
         "sueldos, el inventario de materiales y sus proveedores, la creación y el "
         "seguimiento de proyectos de obra con sus empleados y materiales asignados, "
         "la generación de informes de compra para los materiales faltantes y el "
         "cálculo del costo y la utilidad de cada proyecto a partir de parámetros "
         "configurables.")
    para("Este manual está dirigido al personal que opera el sistema día a día "
         "(empleados administrativos, encargados y administradores). La idea es que "
         "después de leerlo puedas ingresar al sistema, cargar clientes, empleados, "
         "materiales y proveedores, crear proyectos y asignarles recursos, generar "
         "informes de compra y consultar el análisis de costos. Si además tenés "
         "permisos de administrador, vas a poder gestionar usuarios, roles y "
         "permisos, configurar los parámetros de cálculo y consultar la bitácora "
         "del sistema.")
    para("El sistema corre en Windows como aplicación de escritorio. Los datos se "
         "guardan en un servidor SQL Server (local o de red). La recuperación de "
         "contraseña por código requiere conexión a internet, ya que el código se "
         "envía por correo electrónico.")

    # ---------------------------------------------------------------- 2
    h1("2. Requisitos del sistema")
    para("Los requisitos siguientes son para la computadora donde se utiliza la "
         "aplicación (no para el servidor de base de datos).")

    h2("2.1 Hardware")
    make_table(
        ["Componente", "Mínimo", "Recomendado"],
        [
            ["Procesador", "Intel Core i3 o equivalente", "Intel Core i5 o superior"],
            ["Memoria RAM", "4 GB", "8 GB"],
            ["Espacio en disco", "1 GB libre", "5 GB libres o más"],
            ["Pantalla", "1366×768", "1920×1080 o superior"],
            ["Teclado y mouse", "Estándar", "Estándar"],
        ],
    )

    h2("2.2 Software")
    make_table(
        ["Componente", "Versión"],
        [
            ["Sistema operativo", "Windows 10 o superior (64 bits)"],
            ["Componentes de la aplicación", ".NET Framework 4.7.2 o superior (suele venir con Windows)"],
            ["Base de datos", "SQL Server 2017 o superior (en la misma PC o en un servidor accesible por red)"],
            ["Conexión a internet", "Necesaria para recibir el código de verificación al recuperar la contraseña (envío por correo electrónico)"],
        ],
    )

    h2("2.3 Antes de usar la aplicación por primera vez")
    bullets([
        "El administrador debe haber dado de alta tu usuario en el sistema. Si todavía no tenés usuario, pedíselo al responsable del sistema.",
        "Para recibir el código de recuperación de contraseña, tu usuario debe tener el correo electrónico cargado correctamente. Verificá eso con el administrador.",
        "La primera vez que se abre la aplicación en una computadora, puede tardar unos segundos más. Es normal.",
    ])

    # ---------------------------------------------------------------- 3
    h1("3. Acceso al sistema")
    para("Cuando abrís Gestor CMB aparece la pantalla de inicio de sesión. Desde "
         "acá vas a poder entrar al sistema o iniciar el proceso de recuperación de "
         "contraseña si te la olvidaste.")

    h2("3.1 Pantalla de inicio de sesión")
    image_placeholder("Pantalla de inicio de sesión")
    h3("Elementos de la pantalla")
    bullets([
        ("Mail: ", "tu correo electrónico, que es el dato con el que iniciás sesión."),
        ("Contraseña: ", "la contraseña que te asignó el administrador o la que cambiaste vos. Se muestra oculta con puntos."),
        ("Botón «Iniciar sesión»: ", "valida tus credenciales y, si son correctas, abre la ventana principal. También podés presionar Enter."),
        ("Enlace «¿Olvidó su contraseña?»: ", "abre el asistente de recuperación de contraseña."),
    ])
    h3("Pasos para iniciar sesión")
    steps([
        "Escribí tu correo electrónico en el campo «Mail».",
        "Escribí tu contraseña en el campo «Contraseña».",
        "Hacé clic en «Iniciar sesión» (o presioná Enter).",
        "Si los datos son correctos, se abre la ventana principal del sistema.",
    ])
    h3("Problemas comunes")
    bullets([
        ("«Mail o contraseña incorrectos»: ", "revisá que no esté activada la tecla Bloq Mayús y que el correo esté bien escrito. Por seguridad, el sistema no aclara cuál de los dos datos falló."),
        ("«Este usuario no está activo»: ", "tu cuenta fue deshabilitada. Contactá al administrador para que la rehabilite."),
        ("«Por favor, ingrese su mail y contraseña»: ", "dejaste algún campo vacío."),
        ("Si la pantalla no responde al iniciar sesión, ", "puede ser que el servidor de base de datos esté apagado o desconectado. Avisá al responsable del sistema."),
    ])

    h2("3.2 Recuperar contraseña")
    para("Si te olvidaste la contraseña, podés recuperarla vos mismo. El sistema te "
         "envía un código de verificación de 6 dígitos a tu correo electrónico, y "
         "con ese código vas a poder definir una contraseña nueva. El proceso tiene "
         "dos pasos.")
    image_placeholder("Recuperar contraseña — paso 1: ingreso de correo")
    h3("Paso 1 — Solicitar el código")
    steps([
        "En la pantalla de inicio de sesión, hacé clic en «¿Olvidó su contraseña?».",
        "Escribí tu correo electrónico y hacé clic en «Enviar código».",
        "El sistema envía un código de 6 dígitos a tu correo. El código es válido por 15 minutos.",
    ])
    image_placeholder("Recuperar contraseña — paso 2: código y nueva contraseña")
    h3("Paso 2 — Establecer la nueva contraseña")
    steps([
        "Revisá tu correo y copiá el código recibido en el campo «Código de verificación».",
        "Escribí tu nueva contraseña en «Nueva contraseña» y repetila en «Confirmar contraseña».",
        "Hacé clic en «Cambiar contraseña».",
        "Si todo es correcto, vas a ver el mensaje «Contraseña cambiada exitosamente. Ya puede iniciar sesión.» y la ventana se cierra.",
    ])
    h3("Problemas comunes")
    bullets([
        ("«No se encontró una cuenta activa con ese correo»: ", "el correo no está registrado o la cuenta está deshabilitada. Verificá el correo o consultá al administrador."),
        ("«El código es incorrecto o ha expirado»: ", "el código vale 15 minutos. Si pasó ese tiempo, repetí el proceso desde el paso 1 para generar uno nuevo."),
        ("«Las contraseñas no coinciden»: ", "escribí exactamente la misma contraseña en los dos campos."),
        ("«La contraseña debe tener al menos 6 caracteres»: ", "elegí una contraseña más larga."),
        ("«Complete todos los campos»: ", "quedó algún campo vacío en el paso 2."),
    ])

    # ---------------------------------------------------------------- 4
    h1("4. Ventana principal")
    para("Una vez que iniciás sesión accedés a la ventana principal. Es la pantalla "
         "central del sistema: tiene un menú lateral para ir a los distintos "
         "módulos y un área central donde se carga la pantalla del módulo que elijas. "
         "Al entrar, se muestra el panel de Inicio (dashboard).")
    image_placeholder("Ventana principal con el menú lateral y el panel de Inicio")

    h2("4.1 El menú lateral")
    para("El menú lateral agrupa todas las funciones del sistema. Algunas opciones "
         "despliegan un submenú al hacer clic. Las opciones que no veas pueden estar "
         "ocultas o deshabilitadas porque tu rol no tiene permiso para acceder a "
         "ellas; si necesitás una, pedile al administrador que te asigne el permiso "
         "correspondiente.")
    make_table(
        ["Opción del menú", "Submenú", "Lleva a"],
        [
            ["Inicio", "—", "Panel de Inicio / Dashboard (sección 5)"],
            ["Proyectos", "Ver Proyectos", "Listado de proyectos (sección 6)"],
            ["", "Agregar Proyectos", "Alta de un proyecto nuevo (sección 6.2)"],
            ["Inventario", "Ver Inventario", "Listado de materiales (sección 8)"],
            ["", "Agregar Materiales", "Alta de un material (sección 8.2)"],
            ["", "Consultar Informes", "Informes de compra (sección 9)"],
            ["", "Agregar Proveedores", "Gestión de proveedores (sección 10)"],
            ["Personal", "Ver Empleados", "Listado de empleados (sección 7)"],
            ["", "Cargar Empleados", "Alta de un empleado (sección 7.2)"],
            ["Ajustes", "Ver Logs", "Bitácora del sistema (sección 13)"],
            ["", "Gestionar Usuarios", "Usuarios, roles y permisos (sección 12)"],
            ["", "Configurar Parámetros", "Parámetros de cálculo (sección 11)"],
        ],
    )
    para("Nota: la gestión de Clientes se abre desde el acceso rápido «Clientes» del "
         "panel de Inicio (ver sección 5).", italic=True, color=GRIS_TXT, size=10)

    h2("4.2 Cerrar sesión")
    para("Para cerrar tu sesión, en el panel de Inicio hacé doble clic sobre tu "
         "correo de usuario (se muestra en la pantalla). El sistema te pregunta "
         "«¿Querés cerrar sesión?»; si confirmás, volvés a la pantalla de inicio de "
         "sesión y otra persona puede ingresar con sus propias credenciales sin "
         "reiniciar la aplicación.")
    bullets([
        "Conviene cerrar sesión cuando termina tu turno de trabajo.",
        "También si te alejás de una computadora compartida.",
        "Y siempre que otro usuario vaya a operar con su propia cuenta en la misma máquina.",
    ])

    # ---------------------------------------------------------------- 5
    h1("5. Inicio (Dashboard)")
    para("El panel de Inicio es la primera pantalla que ves al ingresar. Ofrece "
         "accesos rápidos a los módulos principales y un resumen con algunos "
         "indicadores del negocio.")
    image_placeholder("Panel de Inicio con accesos rápidos e indicadores")
    h2("5.1 Accesos rápidos")
    para("En la parte superior hay cuatro botones grandes que te llevan directo a "
         "los módulos más usados:")
    bullets([
        ("Proyectos: ", "abre el listado de proyectos."),
        ("Inventario: ", "abre el listado de materiales."),
        ("Personal: ", "abre el listado de empleados."),
        ("Clientes: ", "abre la gestión de clientes."),
    ])
    para("Si tu rol no tiene permiso para un módulo, su botón aparece atenuado y no "
         "se puede usar.")
    h2("5.2 Indicadores")
    bullets([
        ("Empleados activos: ", "cantidad de empleados habilitados en el sistema. Con doble clic vas al listado de empleados."),
        ("Informes de compra: ", "cantidad de informes de compra registrados. Con doble clic vas al módulo de informes."),
        ("Próximo proyecto: ", "muestra el proyecto más próximo con su fecha. Con doble clic se abre su detalle."),
    ])

    # ---------------------------------------------------------------- 6
    h1("6. Gestión de Proyectos")
    para("El módulo de Proyectos es el corazón del sistema. Desde acá vas a poder "
         "ver todos los proyectos, crear nuevos, abrir el detalle de cada uno, "
         "asignarle empleados y materiales, consultar su análisis de costos, "
         "modificarlo y generar el informe de compra de los materiales faltantes.")

    h2("6.1 Listado de proyectos")
    para("Se abre desde «Proyectos ▸ Ver Proyectos» o desde el acceso rápido del "
         "Inicio. Muestra los proyectos como tarjetas y permite buscarlos, "
         "ordenarlos y filtrarlos por estado.")
    image_placeholder("Listado de proyectos en tarjetas, con búsqueda y filtros")
    h3("Elementos de la pantalla")
    bullets([
        ("Buscador: ", "campo «Buscar…» para filtrar por descripción del proyecto. El enlace «Limpiar» quita la búsqueda."),
        ("Filtros (panel «Filtros»): ", "orden de la lista (Más recientes, Más antiguos, Descripción A-Z, Cliente A-Z, Más faltantes) y filtro por estado (Todos, En proceso, Suspendido, Finalizado)."),
        ("Tarjetas de proyecto: ", "cada una muestra el número, la descripción, el cliente, la ubicación, el estado, la fecha de inicio y la cantidad de materiales faltantes (si hay)."),
    ])
    para("El estado se distingue por color: En proceso (azul), Suspendido (naranja) "
         "y Finalizado (verde). Al hacer clic en una tarjeta se abre el detalle del "
         "proyecto.")

    h2("6.2 Crear un proyecto nuevo")
    para("Se abre desde «Proyectos ▸ Agregar Proyectos». El proyecto nuevo se crea "
         "en estado «En proceso».")
    image_placeholder("Formulario de alta de proyecto")
    make_table(
        ["Campo", "Descripción", "Obligatorio"],
        [
            ["Descripción", "Nombre o descripción del proyecto de obra.", "Sí"],
            ["Cliente", "Cliente para el que se hace la obra. Se elige de un combo con los clientes cargados.", "Sí"],
            ["Ubicación", "Dirección o ubicación de la obra.", "Sí"],
            ["Fecha de inicio", "Fecha en que comienza el proyecto.", "Sí"],
        ],
    )
    h3("Pasos")
    steps([
        "Completá la descripción y la ubicación.",
        "Elegí el cliente en el combo. Si todavía no existe, primero cargalo desde el módulo de Clientes (sección 5.1 / acceso rápido «Clientes»).",
        "Elegí la fecha de inicio.",
        "Hacé clic en «Agregar Proyecto». El proyecto se crea en estado «En proceso».",
    ])
    para("Si dejás la descripción o la ubicación vacías, o no elegís cliente, el "
         "sistema te avisa con un mensaje de validación.")

    h2("6.3 Detalle del proyecto")
    para("Al hacer clic en una tarjeta del listado se abre el detalle. Es la "
         "pantalla donde se concentra la gestión del proyecto.")
    image_placeholder("Detalle del proyecto con empleados, materiales y totales")
    h3("Qué muestra")
    bullets([
        ("Encabezado: ", "número, descripción, cliente, ubicación, estado, fecha de inicio y fecha de cierre."),
        ("Totales: ", "total de empleados asignados, total de materiales y la utilidad estimada de la empresa para el proyecto (calculada con los parámetros — ver sección 11)."),
        ("Empleados asignados: ", "lista de empleados del proyecto con su sueldo."),
        ("Materiales utilizados: ", "lista de materiales con cantidad, precio unitario y subtotal."),
    ])
    h3("Acciones disponibles")
    bullets([
        ("Modificar: ", "abre el formulario para editar los datos del proyecto (ver 6.4)."),
        ("Agregar empleado: ", "asigna un empleado al proyecto (ver 6.5)."),
        ("Agregar material: ", "asigna un material al proyecto (ver 6.6)."),
        ("Ver análisis: ", "abre los gráficos de costos del proyecto (ver 6.7)."),
        ("Generar informe de compra: ", "crea el informe con los materiales faltantes del proyecto (ver sección 9)."),
        ("Volver: ", "regresa al listado de proyectos."),
    ])

    h2("6.4 Modificar un proyecto")
    para("Desde el detalle, el botón «Modificar» abre el formulario de edición.")
    image_placeholder("Formulario de modificación de proyecto")
    bullets([
        "Podés cambiar la descripción, el cliente, la ubicación, la fecha de inicio, la fecha de cierre y el estado (En proceso, Suspendido o Finalizado).",
        "La fecha de cierre no puede ser anterior a la fecha de inicio; si lo es, el sistema te avisa.",
        "Al guardar, el detalle se actualiza con los nuevos datos.",
    ])

    h2("6.5 Asignar un empleado al proyecto")
    image_placeholder("Ventana para asignar un empleado al proyecto")
    steps([
        "En el detalle del proyecto, hacé clic en «Agregar Empleado».",
        "Elegí al empleado en el combo (se listan los empleados disponibles).",
        "Hacé clic en «Agregar».",
    ])
    para("Si el empleado ya estaba asignado a ese proyecto, el sistema te avisa y no "
         "lo duplica. El sueldo del empleado se incorpora al cálculo del costo del "
         "proyecto.")

    h2("6.6 Asignar un material al proyecto")
    image_placeholder("Ventana para asignar un material al proyecto")
    steps([
        "En el detalle del proyecto, hacé clic en «Agregar Material».",
        "Elegí el material en el combo (se listan los materiales del inventario).",
        "Indicá la cantidad y verificá el precio unitario; el subtotal se calcula solo.",
        "Hacé clic en «Agregar».",
    ])
    para("Si el material ya estaba asignado, el sistema te avisa. Si la cantidad o "
         "el precio no son válidos, te pide corregirlos.")

    h2("6.7 Ver análisis del proyecto")
    para("El enlace «Ver análisis» del detalle abre una ventana con dos gráficos "
         "que muestran la evolución de los costos del proyecto.")
    image_placeholder("Ventana de análisis del proyecto con sus gráficos")
    bullets([
        ("Análisis de compras: ", "representa el gasto en materiales (costo unitario × cantidad) agrupado por período."),
        ("Costo del proyecto: ", "representa, por período, la suma del costo de los materiales más los sueldos de los empleados asignados."),
    ])
    para("Con el filtro «Días / Meses / Años» cambiás la agrupación y los gráficos "
         "se regeneran al instante. Por defecto se agrupan por meses. Si el proyecto "
         "no tiene materiales ni empleados, los gráficos se muestran vacíos sin "
         "errores.")

    # ---------------------------------------------------------------- 7
    h1("7. Gestión de Personal (Empleados)")
    para("Este módulo administra a los empleados de la empresa, que después se "
         "asignan a los proyectos. Se abre desde «Personal ▸ Ver Empleados» o desde "
         "el acceso rápido «Personal» del Inicio.")

    h2("7.1 Listado de empleados")
    image_placeholder("Listado de empleados")
    bullets([
        ("Buscador: ", "filtra por nombre, apellido o DNI."),
        ("Listado: ", "cada empleado muestra nombre y apellido, DNI, sueldo y estado (activo/inactivo)."),
        ("Acciones por empleado: ", "«Editar» para modificar sus datos y un interruptor para activarlo o desactivarlo."),
        ("Botón «Agregar empleado»: ", "abre el formulario de alta."),
    ])

    h2("7.2 Crear un empleado")
    image_placeholder("Formulario de alta de empleado")
    make_table(
        ["Campo", "Descripción", "Obligatorio"],
        [
            ["Nombre", "Nombre de la persona.", "Sí"],
            ["Apellido", "Apellido de la persona.", "Sí"],
            ["Documento (DNI)", "Número de documento. Debe ser un número entero.", "Sí"],
            ["Sueldo", "Sueldo del empleado. Debe ser un número (admite decimales).", "Sí"],
        ],
    )
    h3("Pasos")
    steps([
        "Hacé clic en «Agregar empleado» (o usá «Personal ▸ Cargar Empleados»).",
        "Completá nombre, apellido, documento y sueldo.",
        "Hacé clic en «Agregar».",
    ])
    para("Si falta algún campo, o el documento o el sueldo no son números válidos, "
         "el sistema te avisa para que corrijas.")

    h2("7.3 Modificar y activar/desactivar")
    para("El botón «Editar» abre el formulario para cambiar nombre, apellido, "
         "documento y sueldo. Los empleados no se eliminan: se desactivan con el "
         "interruptor de estado, lo que preserva su historial de asignaciones. Un "
         "empleado inactivo no se ofrece para asignar a nuevos proyectos.")

    # ---------------------------------------------------------------- 8
    h1("8. Gestión de Inventario (Materiales)")
    para("El inventario reúne los materiales que se usan en los proyectos. Se abre "
         "desde «Inventario ▸ Ver Inventario» o desde el acceso rápido del Inicio. "
         "Desde la cabecera de esta pantalla también se llega a Informes de Compra y "
         "a Proveedores.")

    h2("8.1 Listado de materiales")
    image_placeholder("Listado de materiales del inventario")
    bullets([
        ("Listado: ", "cada material muestra descripción, cantidad disponible, precio unitario y categoría."),
        ("Acciones por material: ", "«Editar» para modificarlo y un interruptor para activarlo o desactivarlo."),
        ("Botones de la cabecera: ", "«Informe de compra» (abre la sección 9) y «Proveedores» (abre la sección 10)."),
    ])

    h2("8.2 Crear un material")
    image_placeholder("Formulario de alta de material")
    make_table(
        ["Campo", "Descripción", "Obligatorio"],
        [
            ["Descripción", "Nombre del material.", "Sí"],
            ["Cantidad", "Cantidad disponible. Debe ser un número mayor o igual a 0.", "Sí"],
            ["Precio unitario", "Precio por unidad. Debe ser un número mayor o igual a 0.", "Sí"],
            ["Categoría", "Categoría del material, elegida de un combo.", "Sí"],
        ],
    )
    h3("Pasos")
    steps([
        "Abrí «Inventario ▸ Agregar Materiales» (o usá el alta desde el listado).",
        "Completá descripción, cantidad, precio unitario y categoría.",
        "Hacé clic en «Agregar».",
    ])
    para("Si algún dato no es válido, el sistema te avisa. Los materiales tampoco se "
         "eliminan: se desactivan con su interruptor de estado.")

    # ---------------------------------------------------------------- 9 (informes)
    h1("9. Informes de Compra")
    para("Un informe de compra agrupa los materiales faltantes de un proyecto para "
         "gestionar su compra. Cada informe pasa por tres estados:")
    bullets([
        ("Pendiente: ", "recién generado, espera la confirmación de la compra."),
        ("Finalizado: ", "la compra se aplicó al proyecto; los materiales se sumaron a su detalle y se guardó una copia histórica (snapshot)."),
        ("Cancelado: ", "el informe se descartó sin aplicarse; los faltantes del proyecto siguen disponibles para generar otro informe más adelante."),
    ])
    para("Importante: cancelar (eliminar) un informe no lo borra físicamente; lo "
         "deja registrado en el historial con estado «cancelado». Esto preserva la "
         "trazabilidad.")

    h2("9.1 Generar un informe de compra")
    para("Se genera desde el detalle de un proyecto (botón «Generar informe de "
         "compra»). El sistema toma todos los materiales faltantes pendientes del "
         "proyecto. Si ya existe un informe de ese proyecto con la fecha de hoy, lo "
         "reutiliza en lugar de crear uno nuevo.")
    bullets([
        "Si el proyecto no tiene materiales faltantes, el sistema avisa «No hay materiales faltantes para generar el informe» y no hace nada.",
        "Al terminar, el informe queda disponible (en estado pendiente) en la pantalla de Informes de Compra.",
    ])

    h2("9.2 Consultar informes pendientes")
    para("Se abre desde «Inventario ▸ Consultar Informes» o desde el botón "
         "«Informe de compra» del inventario. Muestra los informes pendientes "
         "agrupados por proyecto (el más reciente de cada uno) con sus materiales "
         "faltantes.")
    image_placeholder("Listado de informes de compra pendientes")
    bullets([
        ("Buscar: ", "filtra por descripción del proyecto o razón social del cliente (escribí y presioná Buscar o Enter)."),
        ("Agregar compra: ", "aplica la compra del informe (ver 9.3)."),
        ("Eliminar: ", "cancela el informe sin aplicarlo (ver 9.4)."),
        ("Historial: ", "abre el historial de informes cerrados (ver 9.5)."),
    ])
    para("Si no tenés el permiso para ver informes, el sistema muestra «No tenés "
         "permisos para acceder a esta pantalla» y vuelve al Inicio.",
         color=GRIS_TXT, size=10, italic=True)

    h2("9.3 Aplicar una compra")
    para("El botón «Agregar compra» confirma que los materiales del informe se "
         "compraron. El sistema:")
    bullets([
        "Suma los materiales al detalle del proyecto (buscándolos en el inventario por descripción, tipo y unidad).",
        "Guarda una copia histórica (snapshot) de lo aplicado.",
        "Marca el informe como «finalizado» y, si había otros informes pendientes del mismo proyecto, los pasa a «cancelado».",
        "Quita los materiales faltantes del proyecto, ya que pasaron a estar comprados.",
    ])
    para("Si algún material faltante no se encuentra en el inventario, se omite y se "
         "continúa con los demás. Toda la operación es transaccional: si algo falla, "
         "no queda nada aplicado a medias.")

    h2("9.4 Cancelar un informe")
    para("El botón «Eliminar» descarta un informe pendiente sin aplicarlo. El "
         "informe queda con estado «cancelado» (no se borra) y los materiales "
         "faltantes del proyecto se conservan para poder generar otro informe más "
         "adelante.")

    h2("9.5 Historial de informes")
    para("El botón «Historial» muestra los informes ya cerrados (finalizados y "
         "cancelados). Para cada uno se ve la fecha, el proyecto, el estado final y "
         "los materiales involucrados. Es una consulta de solo lectura, útil para "
         "auditoría. Podés filtrar por proyecto o cliente y volver con el botón ««».")
    image_placeholder("Historial de informes de compra")

    # ---------------------------------------------------------------- 10 (proveedores)
    h1("10. Gestión de Proveedores")
    para("Los proveedores son las empresas o personas que abastecen los materiales. "
         "Se abre desde «Inventario ▸ Agregar Proveedores» o desde el botón "
         "«Proveedores» del inventario.")
    image_placeholder("Gestión de proveedores")
    h2("10.1 Listado y alta")
    bullets([
        ("Listado: ", "cada proveedor muestra su descripción (nombre/razón social), teléfono y mail, con «Editar» y un interruptor de estado."),
        ("Alta: ", "completá descripción, teléfono y mail y hacé clic en «Agregar Proveedor»."),
    ])
    h2("10.2 Modificar y desactivar")
    para("El botón «Editar» abre el formulario para cambiar la descripción, el "
         "teléfono y el mail del proveedor. Como en el resto del sistema, los "
         "proveedores no se eliminan: se desactivan con su interruptor de estado, "
         "preservando el historial.")

    # ---------------------------------------------------------------- 11 (parametros)
    h1("11. Configurar parámetros")
    para("Se abre desde «Ajustes ▸ Configurar Parámetros». Acá se definen los tres "
         "porcentajes que el sistema usa para calcular el costo total y la utilidad "
         "de cada proyecto.")
    image_placeholder("Pantalla de configuración de parámetros")
    make_table(
        ["Parámetro", "Qué representa"],
        [
            ["Margen empleados", "Porcentaje que se aplica sobre el costo de los sueldos de los empleados asignados."],
            ["Margen materiales", "Porcentaje que se aplica sobre el costo de los materiales."],
            ["Utilidad empresa", "Porcentaje de utilidad esperada por la empresa sobre el proyecto."],
        ],
    )
    h3("Pasos")
    steps([
        "Ingresá cada valor como porcentaje (por ejemplo, 20 para un 20 %).",
        "Hacé clic en «Guardar».",
        "Si todo sale bien, aparece «Parámetros guardados correctamente» en verde.",
    ])
    para("Los nuevos valores se aplican de inmediato a los próximos cálculos de "
         "monto y utilidad de los proyectos. Internamente, el sistema guarda los "
         "porcentajes como decimales (20 % se guarda como 0,20) junto con la fecha "
         "del cambio y el usuario que lo hizo.")

    # ---------------------------------------------------------------- 12 (usuarios)
    h1("12. Gestión de Usuarios, Roles y Permisos")
    para("Se abre desde «Ajustes ▸ Gestionar Usuarios». Es el panel administrativo "
         "para crear y administrar los usuarios que entran al sistema, los roles y "
         "los permisos de cada rol. Esta tarea está reservada a quienes tienen el "
         "permiso GESTIONAR_USUARIOS (normalmente, el rol Administrador).")
    image_placeholder("Pantalla de gestión de usuarios, roles y permisos")

    h2("12.1 Crear un usuario")
    make_table(
        ["Campo", "Descripción", "Obligatorio"],
        [
            ["Mail", "Correo con el que el usuario inicia sesión. No puede estar repetido.", "Sí"],
            ["Contraseña", "Contraseña inicial. Se guarda cifrada (hash) y nunca en texto plano.", "Sí"],
            ["Teléfono", "Teléfono de contacto. Si se carga, debe ser numérico.", "No"],
            ["Idioma", "Idioma de la interfaz para ese usuario: Español o Inglés.", "Sí"],
            ["Rol", "Rol inicial del usuario, del que hereda sus permisos.", "Sí"],
        ],
    )
    h3("Pasos")
    steps([
        "Completá el formulario «Nuevo usuario» (mail, contraseña, teléfono, idioma y rol).",
        "Hacé clic en «Crear usuario».",
        "El usuario queda creado y activo, y hereda los permisos del rol elegido.",
    ])
    bullets([
        ("«Ya existe un usuario con ese mail»: ", "elegí otro correo."),
        ("Datos incompletos o teléfono no numérico: ", "el sistema señala el campo a corregir."),
    ])

    h2("12.2 Modificar usuarios y asignar roles")
    para("El botón «Editar» de cada usuario abre el formulario «Editar usuario», "
         "donde podés cambiar el mail, la contraseña, el teléfono y el idioma, y "
         "administrar los roles del usuario (un usuario puede tener uno o más roles "
         "y hereda los permisos de todos).")
    image_placeholder("Formulario de edición de usuario y asignación de roles")
    bullets([
        ("Asignar un rol: ", "elegí un rol del combo y hacé clic en «Agregar rol»."),
        ("Quitar un rol: ", "hacé doble clic sobre el rol en la grilla y confirmá."),
    ])
    para("Si cambiás el idioma de tu propio usuario (el que está logueado), el "
         "sistema reinicia la aplicación para aplicar el nuevo idioma. Para otros "
         "usuarios, el cambio se aplica en su próxima sesión.")

    h2("12.3 Activar / desactivar usuarios")
    para("Los usuarios no se eliminan: se desactivan con su interruptor de estado. "
         "Un usuario inactivo no puede iniciar sesión ni recibir códigos de "
         "recuperación. Esto preserva el historial de sus acciones.")

    h2("12.4 Roles y matriz de permisos")
    para("En la sección de Roles podés crear roles nuevos y, al seleccionar un rol, "
         "el sistema muestra una matriz de permisos organizada por módulo. Cada "
         "módulo tiene hasta dos permisos:")
    bullets([
        ("VER: ", "permite ver/consultar ese módulo."),
        ("GESTIONAR: ", "permite crear, modificar y dar de baja en ese módulo."),
    ])
    image_placeholder("Matriz de permisos por rol (VER / GESTIONAR por módulo)")
    para("Cada cambio en un interruptor de la matriz se guarda al instante (no hace "
         "falta un botón «Guardar»). Los usuarios del rol verán los nuevos permisos "
         "en su próxima sesión.")
    h3("El rol Administrador está protegido")
    para("El rol Administrador tiene todos los permisos y se muestra en modo solo "
         "lectura: sus interruptores están deshabilitados para que no se lo pueda "
         "dejar sin acceso por error. Es una protección intencional del sistema.")
    h3("Crear un rol")
    steps([
        "Escribí el nombre en el campo «Nuevo rol».",
        "Hacé clic en «+ Crear rol».",
        "El rol se crea sin permisos; asignáselos después desde la matriz.",
    ])

    # ---------------------------------------------------------------- 13 (logs)
    h1("13. Bitácora (Logs)")
    para("Se abre desde «Ajustes ▸ Ver Logs». Muestra el registro de eventos del "
         "sistema (acciones, advertencias y errores) para auditoría y diagnóstico. "
         "Requiere el permiso VER_LOGS.")
    image_placeholder("Visor de la bitácora / logs del sistema")
    h2("13.1 Elementos de la pantalla")
    bullets([
        ("Filtrar: ", "busca por texto dentro del mensaje o la excepción."),
        ("Nivel: ", "filtra por tipo de evento (Todos, Info, Warning, Error, Debug)."),
        ("Top N: ", "cantidad de registros recientes a cargar (por defecto 500)."),
        ("Grilla: ", "columnas Fecha, Nivel, Mensaje y Excepción. Los errores se ven en rojo, las advertencias en naranja y los eventos de depuración en gris."),
        ("Ver archivo: ", "abre la carpeta de logs en el Explorador de Windows, seleccionando el archivo .log más reciente."),
    ])
    para("La bitácora se alimenta automáticamente: el sistema registra los eventos "
         "según su importancia (información, advertencia o error). No requiere "
         "ninguna acción de tu parte.")

    # ---------------------------------------------------------------- 14 (permisos por rol)
    h1("14. Permisos del sistema")
    para("La tabla siguiente resume los permisos que existen en Gestor CMB y qué "
         "habilita cada uno. Un rol agrupa permisos, y cada usuario hereda los "
         "permisos de los roles que tenga asignados.")
    make_table(
        ["Permiso", "Módulo", "Qué habilita"],
        [
            ["VER_PROYECTOS", "Proyectos", "Ver el listado y el detalle de proyectos"],
            ["GESTIONAR_PROYECTOS", "Proyectos", "Crear y modificar proyectos; asignar empleados y materiales"],
            ["VER_INVENTARIO", "Inventario", "Ver el listado de materiales"],
            ["GESTIONAR_MATERIALES", "Inventario", "Crear y modificar materiales"],
            ["GESTIONAR_PROVEEDORES", "Inventario", "Crear y modificar proveedores"],
            ["VER_INFORMES_COMPRA", "Inventario", "Ver informes de compra y su historial"],
            ["GESTIONAR_INFORMES_COMPRA", "Inventario", "Generar, aplicar y cancelar informes de compra"],
            ["VER_EMPLEADOS", "Personal", "Ver el listado de empleados"],
            ["GESTIONAR_EMPLEADOS", "Personal", "Crear, modificar y desactivar empleados"],
            ["VER_CLIENTES", "Clientes", "Ver el listado de clientes"],
            ["GESTIONAR_CLIENTES", "Clientes", "Crear, modificar y desactivar clientes"],
            ["VER_LOGS", "Ajustes", "Consultar la bitácora del sistema"],
            ["GESTIONAR_USUARIOS", "Ajustes", "Crear/editar usuarios, roles y permisos"],
            ["CONFIGURAR_PARAMETROS", "Ajustes", "Modificar los parámetros de cálculo"],
        ],
    )

    # ---------------------------------------------------------------- 15 (FAQ)
    h1("15. Preguntas frecuentes (FAQ)")

    faq = [
        ("¿Cómo recupero mi contraseña si me la olvidé?",
         "Desde la pantalla de inicio de sesión, hacé clic en «¿Olvidó su contraseña?». "
         "El sistema te pide el correo, te envía un código de 6 dígitos por mail, lo "
         "validás y elegís una nueva contraseña. El detalle está en la sección 3.2."),
        ("¿Qué hago si el código de recuperación no me llegó?",
         "Revisá la carpeta de correo no deseado (spam). El código vale 15 minutos; si "
         "pasó ese tiempo, repetí el proceso para generar uno nuevo. Si aun así no "
         "llega, puede que tu correo esté mal cargado: avisá al administrador."),
        ("¿Por qué no veo algún módulo en el menú?",
         "Porque tu rol no tiene el permiso correspondiente. Pedile al administrador "
         "que te lo asigne desde «Gestionar Usuarios»."),
        ("¿Puedo eliminar un cliente, empleado, material o proveedor?",
         "No se eliminan: se desactivan con su interruptor de estado. Esto preserva "
         "el historial. Un registro inactivo deja de ofrecerse para nuevas operaciones."),
        ("¿Cómo cambio el idioma de la aplicación?",
         "El idioma es una preferencia de cada usuario. Se cambia desde «Editar "
         "usuario» eligiendo Español o Inglés. Si cambiás el de tu propia cuenta, la "
         "aplicación se reinicia para aplicarlo."),
        ("¿Qué pasa si genero un informe de compra y el proyecto no tiene faltantes?",
         "El sistema avisa «No hay materiales faltantes para generar el informe» y no "
         "crea nada."),
        ("Eliminé un informe de compra por error, ¿se perdió?",
         "No. «Eliminar» un informe lo deja en estado «cancelado», no lo borra. Queda "
         "en el historial y los materiales faltantes del proyecto se conservan."),
        ("¿El sistema guarda mi contraseña en texto plano?",
         "No. Las contraseñas se guardan con un hash unidireccional (BCrypt). Ni "
         "siquiera el administrador puede ver tu contraseña; por eso, si la olvidás, "
         "hay que generar una nueva."),
        ("¿Quién puede crear usuarios o cambiar permisos?",
         "Solo los usuarios con el permiso GESTIONAR_USUARIOS (típicamente el rol "
         "Administrador). El rol Administrador está protegido y no puede quedar sin "
         "permisos."),
        ("¿Dónde se ven los costos de un proyecto?",
         "En el detalle del proyecto (totales) y en «Ver análisis», que grafica el "
         "gasto en materiales y el costo total (materiales + sueldos) por período."),
    ]
    for q, a in faq:
        h3(q)
        para(a)

    # ---------------------------------------------------------------- 16 (glosario)
    h1("16. Glosario del dominio")
    para("Definiciones cortas de los términos que aparecen en el sistema.",
         italic=True, color=GRIS_TXT)
    glos = [
        ("Proyecto", "Obra que la empresa ejecuta para un cliente. Tiene descripción, "
         "ubicación, fechas, estado y recursos asignados (empleados y materiales)."),
        ("Estado del proyecto", "Situación del proyecto: En proceso, Suspendido o "
         "Finalizado."),
        ("Cliente", "Empresa o persona para la que se realiza una obra."),
        ("Empleado", "Persona del personal de la empresa, con su sueldo, que puede "
         "asignarse a proyectos."),
        ("Material", "Insumo del inventario que se usa en los proyectos, con "
         "descripción, cantidad, precio unitario y categoría."),
        ("Proveedor", "Empresa o persona que abastece los materiales."),
        ("Material faltante", "Material que un proyecto necesita y todavía no tiene, "
         "y que se agrupa en un informe de compra."),
        ("Informe de compra", "Documento que agrupa los materiales faltantes de un "
         "proyecto para gestionar su compra. Puede estar pendiente, finalizado o "
         "cancelado."),
        ("Snapshot (copia histórica)", "Foto de los materiales aplicados cuando un "
         "informe se finaliza, que se conserva para auditoría aunque luego cambien "
         "los datos del proyecto."),
        ("Parámetros", "Porcentajes (margen de empleados, margen de materiales y "
         "utilidad de la empresa) usados para calcular el costo y la utilidad de un "
         "proyecto."),
        ("Rol (familia)", "Conjunto de permisos que se asigna a los usuarios. El rol "
         "Administrador está protegido."),
        ("Permiso", "Habilitación para ver (VER) o gestionar (GESTIONAR) un módulo "
         "del sistema."),
        ("Usuario activo / inactivo", "Un usuario inactivo no puede iniciar sesión. "
         "Los usuarios no se eliminan, se desactivan."),
        ("Código de verificación / OTP", "Código de un solo uso de 6 dígitos que el "
         "sistema envía por correo para recuperar la contraseña. Vale 15 minutos."),
        ("Bitácora (logs)", "Registro de los eventos del sistema (información, "
         "advertencias y errores) para auditoría y diagnóstico."),
    ]
    for term, desc in glos:
        h3(term)
        para(desc)


# ============================================================================
# Construccion del documento
# ============================================================================
portada()
indice()
contenido()
set_footer()

out = os.path.join(os.path.dirname(os.path.abspath(__file__)), "Manual_Usuario_GestorCMB.docx")
doc.save(out)
print("OK ->", out)
