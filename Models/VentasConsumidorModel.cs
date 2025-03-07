namespace Contabsv_core.Models
{
    public class VentasConsumidorModel
    {
        public int idVentaConsumidor { get; set; }
        public string fechaPresentacion { get; set; }
        public string fechaEmision { get; set; }
        public int idClaseDocumento { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroResolucion { get; set; }
        public string serieDocumento { get; set; }
        public string? numeroControlInterno { get; set; }
        public string? numeroMaquinaRegistradora { get; set; } = "";
        public decimal? ventasExentas { get; set; } = 0;
        public decimal? ventasInternasExentasNoProporcionalidad { get; set; } = 0;
        public decimal? ventasNoSujetas { get; set; } = 0;
        public decimal? ventasGravadasLocales { get; set; } = 0;
        public decimal? exportacionesCentroamerica { get; set; } = 0;
        public decimal? exportacionesFueraCentroamerica { get; set; } = 0;
        public decimal? exportacionesServicio { get; set; } = 0;
        public decimal? ventasZonasFrancasDpa { get; set; } = 0;
        public decimal? ventasTercerosNoDomiciliados { get; set; } = 0;
        public decimal? totalVentas { get; set; } = 0;
        public int idTipoOperacionCg { get; set; }
        public int idOperacion { get; set; }
        public int numeroAnexo { get; set; }
        public int idCliente { get; set; }
        public int idClienteCit { get; set; }
    }

    public class ListVentasConsumidorModel
    {
        public int idVentaConsumidor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaEmision { get; set; }
        public DateTime fechaPresentacion { get; set; }
        public int idClaseDocumento { get; set; }
        public string codigoClaseDocumento { get; set; }
        public string detalleClaseDocumento { get; set; }
        public int idTipoDocumento { get; set; }
        public string codigoTipoDocumento { get; set; }
        public string detalleTipoDocumento { get; set; }
        public string numeroResolucion { get; set; }
        public string serieDocumento { get; set; }
        public string? numeroControlInterno { get; set; }
        public string? numeroMaquinaRegistradora { get; set; }
        public decimal? ventasExentas { get; set; } = 0;
        public decimal? ventasInternasExentasNoProporcionalidad { get; set; } = 0;
        public decimal? ventasGravadasLocales { get; set; } = 0;
        public decimal? exportacionesCentroamerica { get; set; } = 0;
        public decimal? exportacionesFueraCentroamerica { get; set; } = 0;
        public decimal? exportacionesServicio { get; set; } = 0;
        public decimal? ventasZonasFrancasDpa { get; set; } = 0;
        public decimal? ventasTercerosNoDomiciliados { get; set; } = 0;
        public decimal? totalVentas { get; set; } = 0;
        public int idTipoOperacionCg { get; set; } = 0;
        public string codigoTipoOperacionCg { get; set; }
        public string detalleTipoOperacionCg { get; set; }
        public int idOperacion { get; set; } = 0;
        public string codigoOperacion { get; set; }
        public string detalleOperacion { get; set; }
        public int numeroAnexo { get; set; } = 0;
        public bool posteado { get; set; }
        public bool anulado { get; set; }
        public bool eliminado { get; set; }
        public int idCliente { get; set; } = 0;
    }
}
