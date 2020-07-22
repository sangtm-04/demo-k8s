using Demo.WebApp.Common;
using Demo.WebApp.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.WebApp.DL
{
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng</param>
        /// <returns></returns>
        public int Insert(Customer customer)
        {
            string storeName = "Proc_InsertCustomer";
            return ExecuteNonQuery(new Object[] { customer.CustomerId,customer.CustomerName }, storeName) ;
        }

        /// <summary>
        /// ExecuteNonQuery -> sử dụng để thực thi một stored procedure theo các tham số truyền vào
        /// </summary>
        /// <param name="parameters">Các tham số truyền vào</param>
        /// <param name="storeName">Tên store</param>
        /// <returns>Số bản ghi bị ảnh hưởng (Thêm mới, sửa, xóa)</returns>
        /// <returns></returns>
        public int ExecuteNonQuery(Customer customer, string storeName)
        {
            using (DatabaseContext dataContext = new DatabaseContext())
            {
                var parameters = dataContext.GetParamFromStore(storeName);
                foreach (MySqlParameter parameter in parameters)
                {
                    var paramName = parameter.ToString().Replace("@", string.Empty);
                    var property = customer.GetType().GetProperty(paramName);
                    if (property != null)
                    {
                        var paramValue = property.GetValue(customer);
                        parameter.Value = paramValue != null ? paramValue : DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                return dataContext.ExecuteNonQuery();
            }
        }

        public virtual int ExecuteNonQuery(object[] parameters, string storeName)
        {
            using (DatabaseContext dataContext = new DatabaseContext())
            {
                dataContext.MapValueToStorePram(storeName, parameters);
                return dataContext.ExecuteNonQuery();
            }
        }
    }
}
