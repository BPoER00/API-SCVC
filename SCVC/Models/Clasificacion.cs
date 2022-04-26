using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_CLASIFICACION")]
    public class Clasificacion
    {
        [Key]
        public int IdClasificacion { get; set; }
        [Required(ErrorMessage = "El Campo Clasificación Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreClasificacion { get; set; }
    }
}