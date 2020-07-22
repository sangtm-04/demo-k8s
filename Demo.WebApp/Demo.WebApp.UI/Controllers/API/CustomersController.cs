using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Demo.WebApp.DL;
using Demo.WebApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.UI.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public AjaxResult InsertCustomer([FromBody] Customer customer)
        {
            customer.CustomerId = Guid.NewGuid().ToString();
            var result = _customerRepository.Insert(customer);
            if (result > 0)
            {
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Thêm mới khách hàng thành công."
                };
            }
            else
            {
                return new AjaxResult
                {
                    Code = 1000,
                    Success = true,
                    Message = "Thêm mới khách hàng thất bại."
                };
            }
        } 
    }
}