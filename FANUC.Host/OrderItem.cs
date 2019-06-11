using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FANUC.Host
{
    public enum OrderItemStateEnum
    {
        NEW,
        DOWORK,
        DONE
    }

    public class OrderItem
    {
        public int Type { get; set; }

        public int Quantity { get; set; }

        public int ActualQuantity { get; set; }

        public OrderItemStateEnum State { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
