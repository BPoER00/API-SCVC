using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
   [Table("TBL_ROLES")]
    public class Roles
    {
        [Key]
        public int IdRol { get; set; }
        [Required(ErrorMessage = "El Campo Rol Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreRol { get; set; }
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
    }
}