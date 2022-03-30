using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Audit.EntityFramework;
using System.Threading.Tasks;

namespace ClassPort.Domain.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Place { get; set; }
        public string RoomNum { get; set; }

    }
}
