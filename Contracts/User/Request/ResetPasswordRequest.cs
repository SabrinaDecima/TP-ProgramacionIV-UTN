using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.User.Request
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "El token es requerido")]
        public string Token {  get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string NewPassword { get; set; }
    }
}
