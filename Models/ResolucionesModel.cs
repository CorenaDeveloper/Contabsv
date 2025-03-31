namespace Contabsv_core.Models
{
    public class ResolucionesModel
    {
        public int id { get; set; }
        public int idTipoDocumento { get; set; }
        public string documento { get; set; }
        public string nombreCorto { get; set; }
        public string numeroResolucion { get; set; }
        public string numeroSerie { get; set; }
        public bool activo { get; set; }
    }

    public class CrudResolucionesModel
    {
        public int id { get; set; }
        public int idTipoDocumento { get; set; }
        public int idCliente { get; set; }
        public string numeroResolucion { get; set; }
        public string numeroSerie { get; set; }
        public bool activo { get; set; }
    }
}
