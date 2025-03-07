namespace Contabsv_core.Models
{
    public class VentasContribuyentes
    {
        public int idVentaContribuyentes { get; set; }
        public string fechaPresentacion { get; set; }
        public string fechaEmisionDocumento { get; set; }
        public int idClaseDocumento { get; set; }
        public int idTipoDocumento { get; set; }
        public string? numeroResolucion { get; set; }
        public string? serieDocumento { get; set; }
        public string? numeroDocumento { get; set; }
        public string? numeroControlInterno { get; set; }
        public decimal? ventasExentas { get; set; }
        public decimal? ventasNoSujetas { get; set; }
        public decimal? ventasGravadasLocales { get; set; }
        public decimal? debitoFiscal { get; set; }
        public decimal? ventasTercerosNoDomiciliados { get; set; }
        public decimal? debitoFiscalVentasTerceros { get; set; }
        public decimal? totalVentas { get; set; }
        public int idTipoOperacionCg { get; set; }
        public int idOperacion { get; set; }
        public int idCliente { get; set; }
        public int idClienteCit { get; set; }
    }
}
