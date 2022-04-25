using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_ETNIA")]
    public class Etnias
    {
        [Key]
        public int IdEtnia { get; set; }
        [Required(ErrorMessage = "El Campo Nombre Etnia Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreEtnias { get; set; }
        [Required(ErrorMessage = "El Campo  Descripción Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string Descripcion { get; set; }
    }
}