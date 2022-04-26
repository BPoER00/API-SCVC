using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_GENERACIONES")]
    public class Generaciones
    {
        [Key]
        public int IdGeneracion { get; set; }
        [Required(ErrorMessage = "El Campo Nombre Generación Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Generacion { get; set; }
        [Required(ErrorMessage = "El Campo Descripción  Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
    }
}