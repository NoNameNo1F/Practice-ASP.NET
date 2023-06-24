using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace razorweb.models{
    public class AppUser : IdentityUser{
        
        public virtual string? PublicKey { get; set; }

        public virtual string? PrivateKey { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public virtual string? HomeAddress { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime? BirthDate { get; set; }
    }
}
