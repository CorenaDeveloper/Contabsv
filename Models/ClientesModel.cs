namespace Contabsv_core.Models
{
    public class ClientesModel
    {
        public int idClienteClt { get; set; }
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
        public string nitCliente { get; set; } = "";
        public string tipoContribuyente { get; set; } = "";
        public int idCliente { get; set; }
    }
}
