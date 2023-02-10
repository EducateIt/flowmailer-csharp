using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowmailer.Models
{
    public static class DeliveryNotificationTypes
    {
        public static string None = "NONE";

        public static string Failure = "FAILURE";

        public static string DeliveryAndFailure = "DELIVERY_AND_FAILURE";
    }
}
