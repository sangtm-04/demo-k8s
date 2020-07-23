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
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            string storeName = "Proc_SelectAllCustomers";
            return GetEntities(storeName);
        }

        /// <summary>
        /// ExecuteNonQuery -> sử dụng để thực thi một stored procedure theo các tham số truyền vào
        /// </summary>
        /// <param name="customer">Đối tượng khách hàng</param>
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

        /// <summary>
        /// ExecuteNonQuery -> sử dụng để thực thi một stored procedure theo các tham số truyền vào
        /// </summary>
        /// <param name="parameters">Các tham số truyền vào</param>
        /// <param name="storeName">Tên store</param>
        /// <returns>Số bản ghi bị ảnh hưởng (Thêm mới, sửa, xóa)</returns>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(object[] parameters, string storeName)
        {
            using (DatabaseContext dataContext = new DatabaseContext())
            {
                dataContext.MapValueToStorePram(storeName, parameters);
                return dataContext.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lấy dữ liệu theo tên store truyền vào
        /// </summary>
        /// <param name="storeName">Tên store</param>
        /// <returns>IEnumerable</returns>
        /// Create By: NVMANH (20/10/2019)
        public IEnumerable<Customer> GetEntities(string storeName)
        {
            using (DatabaseContext databaseContext = new DatabaseContext())
            {
                MySqlDataReader sqlDataReader = databaseContext.ExecuteReader(storeName);
                while (sqlDataReader.Read())
                {
                    var entity = Activator.CreateInstance<Customer>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        var fieldType = sqlDataReader.GetDataTypeName(i);
                        var fieldName = sqlDataReader.GetName(i);
                        var fieldValue = sqlDataReader.GetValue(i);
                        var property = entity.GetType().GetProperty(fieldName);
                        if (fieldValue != System.DBNull.Value && property != null)
                        {
                            if (fieldType == "BIT")
                            {
                                if ((UInt64)fieldValue == 0) property.SetValue(entity, false);
                                else if ((UInt64)fieldValue == 1) property.SetValue(entity, true);
                                continue;
                            }
                            //if (property.PropertyType == typeof(Guid) && fieldValue.GetType() == typeof(String))
                            if ((property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?)) && fieldValue.GetType() == typeof(String))
                            {
                                property.SetValue(entity, Guid.Parse((string)fieldValue));
                            }
                            else
                            {
                                property.SetValue(entity, fieldValue);
                            }
                        }
                    }
                    yield return entity;
                }
            }
        }
    }
}
