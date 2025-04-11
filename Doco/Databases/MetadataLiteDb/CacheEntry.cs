using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doco.Databases.MetadataLiteDb
{
    internal class CacheEntry
    {
        public object Result { get; set; }
        public DateTime LastModified { get; set; }
    }

}
