using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDistribution
{
    public enum OrderServiceErrorCodeEnum
    {
        NORMAL=0,
		GETMODE=1,
		GETALLOW=2,
		CHECKONBEGIN=3,
		SETPRODUCTTYPE=4,
		SETQUANTITY=5,
		CHECKCONFIRM=6,
		SETCONFIRRM=7,
    }
}
