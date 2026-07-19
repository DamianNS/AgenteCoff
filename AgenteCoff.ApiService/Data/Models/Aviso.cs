using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenteCoff.ApiService.Data.Models
{
    public class Aviso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Auto-numérico

        required public DateTime FechaHora { get; set; }

        [Required]
        [StringLength(500)]
        required public string Texto { get; set; }
    }
}
