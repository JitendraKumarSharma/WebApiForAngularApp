using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using WebApiForAngular.Global;
using System.Configuration;
using Newtonsoft.Json;

namespace WebApiForAngular.Models
{
    public class ManageEmployee
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DateFormatHandling = DateFormatHandling.IsoDateFormat };
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataTable dt = new DataTable();

        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public bool IsMarried { get; set; }
        public DateTime DOB { get; set; }
        public string EmpImage { get; set; }
        public ManageEmployee()
        {
            EmpId = Age = CountryId = StateId = 0;
            Name = Email = City = Gender = EmpImage = Mobile = ZipCode = string.Empty;
            DOB = new DateTime(1990, 01, 01);
            IsMarried = false;
        }

        public DataTable GetAllEmployee()
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = new SqlCommand("GetAllEmployee", cmd.Connection);
                sda = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                sda.Fill(dt);
                cmd.Connection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public DataTable GetEmployeeById(int EmpId)
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = new SqlCommand("SELECT * from Employee where EmpId=" + EmpId, cmd.Connection);
                sda = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                sda.Fill(dt);
                cmd.Connection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public int DeleteEmployeeById(int EmpId)
        {
            int cnt = 0;
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = new SqlCommand("Delete from Employee where EmpId=" + EmpId, cmd.Connection);
                sda = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                cnt = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return cnt;
            }
            catch (Exception ex)
            {
                return cnt;
            }
        }

        public int SaveEmployee(ManageEmployee Emp)
        {
            int id = 0;
            try
            {
                cmd = new SqlCommand("InserOrtUpdateEmployee", cmd.Connection);
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", Emp.EmpId);
                cmd.Parameters.AddWithValue("@Name", Emp.Name);
                cmd.Parameters.AddWithValue("@Age", Emp.Age);
                cmd.Parameters.AddWithValue("@CountryId", Emp.CountryId);
                cmd.Parameters.AddWithValue("@StateId", Emp.StateId);
                cmd.Parameters.AddWithValue("@Email", Emp.Email);
                cmd.Parameters.AddWithValue("@City", Emp.City);
                cmd.Parameters.AddWithValue("@ZipCode", Emp.ZipCode);
                cmd.Parameters.AddWithValue("@Mobile", Emp.Mobile);
                cmd.Parameters.AddWithValue("@Gender", Emp.Gender);
                cmd.Parameters.AddWithValue("@IsMarried", Emp.IsMarried);
                cmd.Parameters.AddWithValue("@DOB", Emp.DOB);
                cmd.Connection.Open();
                id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Connection.Close();
                return id;
            }
            catch (Exception ex)
            {
                return id;
            }
        }

        public DataTable GetAllCountry()
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd = new SqlCommand("GetAllCountry", cmd.Connection);
                sda = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                sda.Fill(dt);
                cmd.Connection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public DataTable GetStateByCountry(int id)
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.Text;
                cmd = new SqlCommand("Select StateId, StateName from StateMaster where CountryId=" + id, cmd.Connection);
                sda = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                sda.Fill(dt);
                cmd.Connection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }

        public bool UploadImageAsImage(byte[] pic, int EmpId)
        {
            try
            {
                cmd.Connection = DbConnection.CreateConnection();
                cmd = new SqlCommand("update employee set  EmpImage_Image=@pic where EmpId=@id", cmd.Connection);
                cmd.Parameters.AddWithValue("@pic", pic);
                cmd.Parameters.AddWithValue("@id", EmpId);
                cmd.Connection.Open();
                int cnt = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UploadImageAsBinary(byte[] pic, int EmpId)
        {
            try
            {
                cmd = new SqlCommand("UpdateImage", cmd.Connection);
                cmd.Connection = DbConnection.CreateConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.Parameters.AddWithValue("@EmpImage_Binary", pic);
                cmd.Connection.Open();
                int cnt = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateImageName(string fileName, int EmpId)
        {
            try
            {
                cmd.Connection = DbConnection.CreateConnection();
                cmd = new SqlCommand("update employee set  EmpImage=@pic where EmpId=@id", cmd.Connection);
                cmd.Parameters.AddWithValue("@pic", fileName);
                cmd.Parameters.AddWithValue("@id", EmpId);
                cmd.Connection.Open();
                int cnt = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (cnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}