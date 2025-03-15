using System.ComponentModel.DataAnnotations;

namespace API_Technical_Test.Modelos.DTO
{
    public class ClienteCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string? nombre { get; set; }
        public string? dni { get; set; }
        public string? domicilio { get; set; }
        public string? provincia { get; set; }
        [Required]
        public double credito { get; set; }
        public int cuenta { get; set; }
    }
}
