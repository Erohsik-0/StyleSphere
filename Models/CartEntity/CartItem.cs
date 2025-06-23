using System.ComponentModel.DataAnnotations;
using StyleSphere.Models.ProductEntity;
using StyleSphere.Models.User;

namespace StyleSphere.Models.CartEntity
{
    public class CartItem
    {

        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string userId { get; set; }
        //public User User { get; set; }

        public int Quantity { get; set; }
    }
}
