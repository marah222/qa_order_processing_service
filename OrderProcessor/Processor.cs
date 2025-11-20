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

            var chargerAmount = order.Amount;
            if (order.IsRush)
            {
                chargerAmount += 20;
            }


            if (customer.AccountBalance > chargerAmount)//pre
            {

                customer.AccountBalance -= chargerAmount;

                return new OrderResult { IsApproved = true, Message = "Order approved." };
            }

            // BUG 3: The logic here is completely missing.
            // It should approve for premium customers even with insufficient funds.

            return new OrderResult { IsApproved = false, Message = "Insufficient funds." };
        }
    }
}
