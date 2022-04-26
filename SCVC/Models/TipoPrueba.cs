using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_TIPOPRUEBA")]
    public class TipoPrueba
    {
        [Key]
        public int IdTipoPrueba { get; set; }
        
        [Required(ErrorMessage = "El Campo Prueba Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombrePrueba { get; set; }
        
        [Required(ErrorMessage = "El Campo Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
        
        public int Estatus { get; set; }
    }
}