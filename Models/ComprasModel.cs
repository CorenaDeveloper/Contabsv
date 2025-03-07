using System.ComponentModel.DataAnnotations;

namespace Contabsv_core.Models
{


    public class CompraModel
    {
        public int idDocCompra { get; set; } = 0;

        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        [DataType(DataType.Date)]
        public string fechaEmision { get; set; } 

        [Required(ErrorMessage = "La fecha de presentación es obligatoria.")]
        [DataType(DataType.Date)]
        public string fechaPresentacion { get; set; } 

        [Required(ErrorMessage = "Debe seleccionar una clase de documento.")]
        public int idclaseDocumento { get; set; } = 0;

        [Required(ErrorMessage = "Debe seleccionar un tipo de documento.")]
        public int idtipoDocumento { get; set; } = 0;

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número de documento no debe exceder los 50 caracteres.")]
        public string numeroDocumento { get; set; } = string.Empty;
        public decimal internasExentas { get; set; }= decimal.Zero;
        public decimal internacionalesExentasYONsujetas { get; set; } = decimal.Zero;
        public decimal importacionesYONsujetas { get; set; } = decimal.Zero;

        [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser positivo.")]
        public decimal internasGravadas { get; set; }=decimal.Zero;
        public decimal internacionesGravadasBienes { get; set; } = decimal.Zero;
        public decimal importacionesGravadasBienes { get; set; } =decimal.Zero;
        public decimal importacionesGravadasServicios { get; set; } = decimal.Zero;

        public decimal creditoFiscal { get; set; } = decimal.Zero;
        [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser positivo.")]
        public decimal totalCompras { get; set; } = decimal.Zero;

        [Required(ErrorMessage = "Debe seleccionar un tipo de operación.")]
        public int idTipoOperacion { get; set; } = 0;

        [Required(ErrorMessage = "Debe seleccionar una clasificación.")]
        public int idClasificacion { get; set; } = 0;

        [Required(ErrorMessage = "Debe seleccionar un tipo de costo o gasto.")]
        public int idTipoCostoGasto { get; set; } = 0;

        [Required(ErrorMessage = "Debe seleccionar un sector.")]
        public int idSector { get; set; } = 0;
        public string numeroAnexo { get; set; } = "3"; // Compras siempre sera anexo 3
        public bool posteado { get; set; } = false;
        public bool anulado { get; set; } = false;
        public bool eliminado { get; set; } = false;

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int idCliente { get; set; } = 0;
        public bool combustible { get; set; } = false;
        public string? numSerie { get; set; } = "";

        [Range(0, double.MaxValue, ErrorMessage = "El valor del IVA retenido debe ser positivo.")]
        public decimal ivaRetenido { get; set; } = decimal.Zero;

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        public int idProveedor { get; set; } = 0;
    }

    public class ListarCompras
    {
        public int idDocCompra { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string fechaEmision { get; set; }
        public string fechaPresentacion { get; set; }
        public int idclaseDocumento { get; set; }
        public int idtipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public decimal internasExentas { get; set; }
        public decimal internacionalesExentasYONsujetas { get; set; }
        public decimal importacionesYONsujetas { get; set; }
        public decimal internasGravadas { get; set; }
        public decimal internacionesGravadasBienes { get; set; }
        public decimal importacionesGravadasBienes { get; set; }
        public decimal importacionesGravadasServicios { get; set; }
        public decimal creditoFiscal { get; set; }
        public decimal totalCompras { get; set; }
        public int idTipoOperacion { get; set; }
        public int idClasificacion { get; set; }
        public int idTipoCostoGasto { get; set; }
        public int idSector { get; set; }
        public string numeroAnexo { get; set; }
        public bool posteado { get; set; }
        public bool anulado { get; set; }
        public bool eliminado { get; set; }
        public int idCliente { get; set; }
        public bool combustible { get; set; }
        public string numSerie { get; set; }
        public decimal ivaRetenido { get; set; }
        public int idProveedor { get; set; }
        public string razonProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public string apellidopProveedor { get; set; }
        public string nitProveedor { get; set; }
        public string nrcProveedor { get; set; }
        public string codigoClasificacion { get; set; }
        public string descripcionClasificacion { get; set; }
        public string codigoClaseDocumento { get; set; }
        public string descripcionClaseDocumento { get; set; }
        public string codigoSectors { get; set; }
        public string descripcionSector { get; set; }
        //public string codigoTipOperacion { get; set; }
        public string descripcionTipOperacion { get; set; }
        public string codigoOperacion { get; set; }
        public string descripcionOperacion { get; set; }
        public string codigoTipoCostofasto { get; set; }
        public string descripcionCostoGasto { get; set; }
    }
}
