using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_MOTIVOCONSULTA")]
    public class MotivoConsulta
    {
       [Key]
       public int IdMotivoConsulta { get; set; }
       
       [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
       [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
       public string Descripcion { get; set; } 
    }
}