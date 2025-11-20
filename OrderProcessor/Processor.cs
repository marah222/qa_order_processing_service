using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor
{
    public class Processor
    {
        private readonly ICustomerRepository _customerRepository;
        public Processor(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public OrderResult ProcessOrder(int customerId, Order order)
        {
            var customer = _customerRepository.GetCustomer(customerId);

            // BUG 1: Should be >= not >
            if (customer.AccountBalance > order.Amount)
            {
                //// BUG 2: Premium customers should get rush orders for free
                if (order.IsRush)
                {
                    // Charge rush order fee only for non premium users
                    bool rushOrderFee = !customer.IsPremium;

                    customer.AccountBalance -= order.Amount + 20 * (rushOrderFee ? 1 : 0); // $20 rush fee
                }
                else // Charge normal order amount
                    customer.AccountBalance -= order.Amount;

                return new OrderResult { IsApproved = true, Message = "Order approved." };
            }

            // BUG 3: The logic here is completely missing.
            // It should approve for premium customers even with insufficient funds.
            if (customer.IsPremium) {
                return new OrderResult { IsApproved = true, Message = "Order approved." };
            }

            return new OrderResult { IsApproved = false, Message = "Insufficient funds." };
        }
    }
}
