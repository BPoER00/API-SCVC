using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_USUARIO")]
    public partial class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El Campo Usuario Es Obligatorio")]
        public string Usuario { get; set; }
        
        [Required(ErrorMessage = "El Password No Puede Estar Vacio")]
        public string PassUser { get; set; }
        
        [Compare("PassUser", ErrorMessage = "Las Password No Coinciden")]
        [NotMapped]
        public string ConfirmarPass { get; set; }
        public int Estatus { get; set; }

        public string Sal { get; set; }

        public int IdRol { get; set; }

        //llaves Foraneas

        public virtual RolUsuario RolUsuario { get; set; }
    }
}