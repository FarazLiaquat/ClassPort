using System.ComponentModel.DataAnnotations;

namespace ClassPort.Domain.Entities
{
    public class Widget
    {
        [Key]
        public int WidgetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
