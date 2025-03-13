using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_Technical_Test.Modelos
{
    public class Empleados
    {
        //No se genera el ID automaticamente, lo generamos nosotros
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Codigo { get; set; }
        [Required]
        public int SucursalID { get; set; }
    }
}
