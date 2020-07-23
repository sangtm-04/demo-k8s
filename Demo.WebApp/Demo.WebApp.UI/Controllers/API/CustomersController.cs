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

        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng</param>
        /// <returns></returns>
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
                    Message = "Thêm mới khách hàng thành công.",
                    Data = result
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

        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public AjaxResult GetCustomers()
        {
            var customers = _customerRepository.GetCustomers().ToList();
            if (customers.Count > 0)
            {
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Lấy dữ liệu thành công.",
                    Data = customers
                };
            }
            else
            {
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true,
                    Message = "Không có dữ liệu.",
                    Data = -1
                };
            }
        }
    }
}