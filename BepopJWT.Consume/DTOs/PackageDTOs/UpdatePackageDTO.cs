using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.Consume.PackageDTOs
{
    public class UpdatePackageDTO
    {
        public int PackageId { get; set; }
        public string PackageTitle { get; set; }
        public string PackageDescription { get; set; }
        public decimal Price { get; set; }
        public int PackageLevel { get; set; }
    }
}
