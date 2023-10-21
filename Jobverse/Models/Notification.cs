using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserEmail { get; set; }

    [Required]
    [MaxLength(255)]
    public string Applier { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public bool Status { get; set; } = false;

    [Required]
    public DateTime Time { get; set; }
    //public User User { get; set; }
}
