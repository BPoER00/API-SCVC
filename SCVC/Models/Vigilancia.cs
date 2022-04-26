using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_VIGILANCIA")]
    public class Vigilancia
    {
        [Key]
        public int IdVigilancia { get; set; }
        [Required(ErrorMessage = "El Campo Vigilancia Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreVigilancia { get; set; }
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
    }
}