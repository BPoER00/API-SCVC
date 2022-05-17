using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_VACUNAS")]
    public class Vacunas
    {
        [Key]
        public int IdVacuna { get; set; }

        [Required(ErrorMessage = "El Campo Nombre Vacuna Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombreVacuna { get; set; }

        [Required(ErrorMessage = "El Campo Dosis Es Necesario")]
        public int Dosis { get; set; }

        [Required(ErrorMessage = "El Campo Generación Es Necesario")]
        public int IdGeneracion { get; set; }

        //Llaves Foraneas
        public virtual Generaciones Generaciones { get; set; }
    }   
}