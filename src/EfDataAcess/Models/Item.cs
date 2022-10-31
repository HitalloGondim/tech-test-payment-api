using System.ComponentModel.DataAnnotations;

namespace EfDataAcess.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string NomeProduto { get; set; }

    }
}
