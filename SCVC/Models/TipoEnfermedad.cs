using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_TIPOENFERMEDAD")]
    public class TipoEnfermedad
    {
        [Key]
        public int IdTipoEnfermedad { get; set; }
        [Required(ErrorMessage = "El Campo Enfermedad Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreEnfermedad { get; set; }
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
    }
}