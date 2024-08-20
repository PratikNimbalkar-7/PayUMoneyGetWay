using System.ComponentModel.DataAnnotations;

namespace ProductDemo.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public int Pro_Prize { get; set; }
        public int Quantity { get; set; }
        
    }
}
