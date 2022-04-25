using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_EDADES")]
    public class Edades
    {
        [Key]
        public int IdEdad { get; set; }
        [Required(ErrorMessage = "El Campo Nombre Edad Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreEdad { get; set; }
        [Required(ErrorMessage = "El Campo Descripción  Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
        
    }
}