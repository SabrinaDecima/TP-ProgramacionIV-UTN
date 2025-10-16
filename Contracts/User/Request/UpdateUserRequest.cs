﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.User.Request
{
    public class UpdateUserRequest
    {

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Email { get; set; }

        public string Telefono { get; set; }

        [Required]
        public string Contraseña { get; set; }

        public int PlanId { get; set; }

    }
}
