using System.ComponentModel.DataAnnotations;

namespace ProductDemo.Models
{
    public class Products
    {
        [Key]
       public int Pro_Id { get; set; }
       public string Pro_Name { get; set; }
       public int Pro_Prize { get; set; }
    }
}
