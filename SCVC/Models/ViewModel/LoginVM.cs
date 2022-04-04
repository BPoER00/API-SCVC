using System.ComponentModel.DataAnnotations;

namespace SCVC.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El Usuario Es Obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "El Clave Es Obligatorio.")]
        public string Clave { get; set; }
    }
}