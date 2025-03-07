namespace Contabsv_core.Models
{
    public class ProveedoresModel
    {
        public int idProveedor { get; set; } = 0;
        public string nombres { get; set; } = "";
        public string apellidos { get; set; } = "";
        public string nombreRazonSocial { get; set; } = "";
        public string nombreComercial { get; set; } = "";
        public bool personaJuridica { get; set; } = false;
        public string duiCliente { get; set; } = "";
        public string representanteLegal { get; set; } = "";
        public string duiRepresentanteLegal { get; set; } = "";
        public string telefonoCliente { get; set; } = "";
        public string celular { get; set; } = "";
        public string nrc { get; set; } = "";
        public string nitProveedor { get; set; } = "";
        public int idCliente { get; set; } = 0;
        public string email { get; set; } = "";
        public string direccion { get; set; } = "";
        public int idSector { get; set; } = 0;
        public DateTime creado { get; set; } = DateTime.Now;
        public string tipoContribuyente { get; set; } = "";
        public string descripcionSector { get; set; } = "";
        public string codigoSector { get; set; } = "";
    }
}
