using System.ComponentModel.DataAnnotations;

namespace WebApiUnique.Models
{
    public class Books
    {
        [Key]
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
    }
}
