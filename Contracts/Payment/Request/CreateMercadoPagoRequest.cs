﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Payment.Request
{
    public class CreateMercadoPagoRequest
    {
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
    }
}
