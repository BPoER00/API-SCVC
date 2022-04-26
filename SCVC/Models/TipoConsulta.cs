using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_TIPOCONSULTA")]
    public class TipoConsulta
    {
        [Key]
        public int IdTipoConsulta { get; set; }
        [Required(ErrorMessage = "El Campo Tratamiento Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreTratamiento { get; set; }
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string DescripcionTratamiento { get; set; }
        [Required(ErrorMessage = "El Campo cantidad Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public int Cantidad { get; set; }
    }
}