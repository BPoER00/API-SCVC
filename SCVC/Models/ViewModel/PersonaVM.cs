using System;
using System.ComponentModel.DataAnnotations;

namespace SCVC.Models.ViewModel
{
    public class PersonaVM
    {
        [Key]
        public int IdPersona { get; set; }
        [Required(ErrorMessage = "El Campo Nombre Persona Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string NombrePersona { get; set; }
        
        [Required(ErrorMessage = "El Campo CUI Persona Es Necesario")]
        [StringLength(13, ErrorMessage = "El Campo No Puede Ser Mayor a 13")]
        public int CUI { get; set; }
        
        [Required(ErrorMessage = "El Campo Dirección Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string IdDireccionFK { get; set; }
        
        [Required(ErrorMessage = "El Campo Genero Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string IdGeneroFK { get; set; }
        
        [Required(ErrorMessage = "El Campo Etnias Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string IdEtniasFK { get; set; }

        [Required(ErrorMessage = "El Campo Edad Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string IdEdadesFK { get; set; }
        
        [Required(ErrorMessage = "El Campo Rol Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        public string IdRolFK { get; set; }
        
        public int Estatus { get; set; }
        
        [Required(ErrorMessage = "El Campo Fecha Es Necesario")]
        [StringLength(100, ErrorMessage = "El Campo No Puede Ser Mayor A 100")]
        [DataType(DataType.DateTime, ErrorMessage = "El campo fecha es obligatorio ")]
        public DateTime Fecha_Nacimiento { get; set; }
    }
}