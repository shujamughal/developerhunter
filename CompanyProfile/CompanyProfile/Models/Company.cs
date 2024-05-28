using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CompanyProfile.Models
{
    public class Company
    {
        [Key]
        [Required]
        public string Email { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [JsonIgnore]
        public CompanyProfile CompanyProfile { get; set; }
        [JsonIgnore]
        public CompanyInsights CompanyInsights { get; set; }
        [JsonIgnore]
        public virtual CompanyDepartments? CompanyDepartments { get; set; }
        [JsonIgnore]
        public virtual List<CompanyReview>? CompanyReviews { get; set; }
        public Company()
        {
            //CompanyProfile = new CompanyProfile();
            //CompanyInsights = new CompanyInsights();
        }
        
    }
}
