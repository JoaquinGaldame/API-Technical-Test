using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Technical_Test.Modelos
{
    public class Clientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? DNI { get; set; }
        public string? Domicilio { get; set; }
        public string? Provincia { get; set; }

        [Required]
        public double Credito { get; set; }
        public int Cuenta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
