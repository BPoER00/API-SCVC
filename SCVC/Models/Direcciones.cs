using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
   [Table("TBL_DIRECCIONES")]
    public class Direcciones
    {
        [Key]
        public int IdDireccion { get; set; } 

        [Required(ErrorMessage = "El Campo Direccion Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]  
        
        public string NombreDireccion { get; set; }
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        
        public string Descripcion { get; set; }
    }
}