using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCVC.Models
{
    [Table("TBL_PERSONA")]
    public class TBL_Persona
    {        
        [Key]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El Campo Nombre Persona Es Necesario")]
        public string NombrePersona { get; set; }

        [Required(ErrorMessage = "El Campo CUI Persona Es Necesario")]
        public int CUI { get; set; }

        [Required(ErrorMessage = "El Campo Dirección Es Necesario")]
        public int IdDireccion { get; set; }

        [Required(ErrorMessage = "El Campo Genero Es Necesario")]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "El Campo Etnias Es Necesario")]
        public int IdEtnia { get; set; }

        [Required(ErrorMessage = "El Campo Edad Es Necesario")]
        public int IdEdad { get; set; }

        [Required(ErrorMessage = "El Campo Rol Es Necesario")]
        public int IdRol { get; set; }

        public int Estatus { get; set; }
        
        [Required(ErrorMessage = "El Campo Fecha Es Necesario")]
        public DateTime Fecha_Nacimiento { get; set; }

        //llaves Foraneas
        public virtual Direcciones Direcciones { get; set; }
        public virtual Generos Generos { get; set; }
        public virtual Etnias Etnias { get; set; }
        public virtual Edades Edades { get; set; }
        public virtual Roles Roles { get; set; }

    }
}