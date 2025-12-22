using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.EntityLayer.Entities
{
    public class Package
    {
        public int PackageId { get; set; }
        public string PackageTitle { get; set; }
        public string PackageDescription { get; set; }
        public decimal Price { get; set; }
        public int PackageLevel { get; set; }
        public ICollection<User>Users { get; set; }
    }
}
