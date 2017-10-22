using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Problend.Models
{
    public class ProblendDBContext : IdentityDbContext<ApplicationUser>
    {
        public ProblendDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Product> Products { get; set; }

        public virtual IDbSet<CartonBox> CartonBoxes { get; set; }

        public virtual IDbSet<PalletizationPicture> PalletizationPictures { get; set; }

        public virtual IDbSet<Client> Clients { get; set; }

        public static ProblendDBContext Create()
        {
            return new ProblendDBContext();
        }

    }
}