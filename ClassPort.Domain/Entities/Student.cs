using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace ClassPort.Domain.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public int Time { get; set; }
    }
}
