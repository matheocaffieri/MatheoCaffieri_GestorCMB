using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Historial
{
    public static class SnapshotService
    {
        private static string CarpetaBase =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GestorCMB", "historial");

        private static string RutaArchivo(Guid idInforme) =>
            Path.Combine(CarpetaBase, idInforme.ToString("N") + ".txt");

        public static void Guardar(Guid idInforme, IEnumerable<MaterialFaltante> materiales)
        {
            Directory.CreateDirectory(CarpetaBase);
            var lines = new List<string>();
            foreach (var m in materiales)
                lines.Add($"{m.CantidadFaltante}|{Esc(m.DescripcionArticuloFaltante)}|{Esc(m.TipoMaterialFaltante)}|{Esc(m.TipoUnidadMaterialFaltante)}");
            File.WriteAllLines(RutaArchivo(idInforme), lines, Encoding.UTF8);
        }

        public static List<MaterialFaltante> Leer(Guid idInforme)
        {
            var ruta = RutaArchivo(idInforme);
            if (!File.Exists(ruta)) return null;

            var result = new List<MaterialFaltante>();
            foreach (var line in File.ReadAllLines(ruta, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split('|');
                if (p.Length < 4) continue;
                result.Add(new MaterialFaltante
                {
                    CantidadFaltante    = int.TryParse(p[0], out int c) ? c : 0,
                    DescripcionArticuloFaltante  = p[1],
                    TipoMaterialFaltante         = p[2],
                    TipoUnidadMaterialFaltante   = p[3]
                });
            }
            return result;
        }

        private static string Esc(string s) => (s ?? string.Empty).Replace("|", "-");
    }
}
