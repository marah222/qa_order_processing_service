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
            if (customer.AccountBalance >= order.Amount)
            {
                // BUG 2: Premium customers should get rush orders for free
                if (order.IsRush)
                {
                    // This is always charging, which is wrong.
                    if(customer.AccountBalance>=order.Amount+20)
                       customer.AccountBalance -= (order.Amount + 20); // $20 rush fee
                    else return new OrderResult { IsApproved = false, Message = "Insufficient funds for order + fees." };
                }

                return new OrderResult { IsApproved = true, Message = "Order approved." };
            }

            // BUG 3: The logic here is completely missing.
            // It should approve for premium customers even with insufficient funds.

            return new OrderResult { IsApproved = false, Message = "Insufficient funds." };
        }
    }
}
