using Demo.WebApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.WebApp.DL
{
    public interface ICustomerRepository
    {
        int Insert(Customer customer);
    }
}
