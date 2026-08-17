using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenteCoff.ServiceDefaults.Models.Dragones;

[Table("Characters")]
public class Character
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string User { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    [Required]
    [StringLength(20)]
    public string Raze { get; set; }

    [Required]
    [StringLength(20)]
    public string Class { get; set; }

    [Required] 
    public int Age { get; set; }
}
