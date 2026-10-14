using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUnique.Models
{
    [Table("tblEmp")]
    public class Emp
    {
        [Key]
        public int Id { get; set; }
        public string EmpName { get; set; }
        public string EMail { get; set; }
        public string Dept { get; set; }
    }
}
