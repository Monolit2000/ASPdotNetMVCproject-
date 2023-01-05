
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class AnonymousUser : User   
    {
        [Key]
        public int Id { get; set; } 



    }
}
