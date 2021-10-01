using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public enum BookStatus
    {
        Available,
        Lost,
        Checked_Out,
        Reserved
    }
}
