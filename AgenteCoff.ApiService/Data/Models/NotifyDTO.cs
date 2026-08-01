using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenteCoff.ApiService.Data.Models
{
    [Table("Notify")]
    public class NotifyDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        required public string PackageName { get; set; }

        [StringLength(200)]
        public string? AppName { get; set; }

        [StringLength(500)]
        public string? Title { get; set; }

        [StringLength(4000)]
        public string? Text { get; set; }

        [Required]
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}