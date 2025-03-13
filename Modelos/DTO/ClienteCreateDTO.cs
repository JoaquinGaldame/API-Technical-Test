﻿using System.ComponentModel.DataAnnotations;

namespace API_Technical_Test.Modelos.DTO
{
    public class ClienteCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string? Nombre { get; set; }
        public string? Detalle { get; set; }
        [Required]
        public double Credito { get; set; }
        public int Cuenta { get; set; }
    }
}
