using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    [RoutePrefix("api")]
    public class EmployeesController : ApiController
    {
        ManageEmployee objmodel = new ManageEmployee();
        public DataTable dt;

        //api/employees
        [HttpGet]
        public DataTable GetAllEmployee()
        {
            dt = objmodel.GetAllEmployee();
            return dt;
        }

        //api/employees/id
        [HttpGet]
        public DataTable GetEmployeeById(int id) // Here Parameter name must be id
        {
            dt = objmodel.GetEmployeeById(id);
            return dt;
        }

        //api/employees/id
        [HttpDelete]
        public int DeleteEmployeeById(int id)
        {
            int cnt = objmodel.DeleteEmployeeById(id);
            return cnt;
        }

        //Use when use body to send data from API Call
        //api/employees
        [HttpPost]
        public int SaveEmployee(ManageEmployee emp)
        {
            int id = objmodel.SaveEmployee(emp);
            return id;
        }

        //api/employees
        [HttpPut]
        public int UpdateEmployee(ManageEmployee emp)
        {
            int id = objmodel.SaveEmployee(emp);
            return id;
        }

        //Use When use FormData to send data from API Cal
        //[HttpPost]
        //[Route("SaveEmployee")]
        //public string SaveEmployee()
        //{
        //    string Name = HttpContext.Current.Request.Form["Name"];
        //    //ManageEmployee Emp = (ManageEmployee)emp;
        //    return Name;
        //    //int id = objmodel.SaveEmployee(emp);
        //    //return id;
        //}

        //api/GetAllCountry
        [HttpGet]
        [Route("GetAllCountry")]
        public DataTable GetAllCountry()
        {
            dt = objmodel.GetAllCountry();
            return dt;
        }

        //api/GetStateByCountry/id
        [HttpGet]
        [Route("GetStateByCountry/{id}")]
        public DataTable GetStateByCountry(int id)
        {
            dt = objmodel.GetStateByCountry(id);
            return dt;
        }

        [HttpPost]
        [Route("Upload")]
        //public string UploadFile()
        //{
        //    var allowedExtensions = new[] { ".png", ".jpg", "jpeg" };

        //    var file = HttpContext.Current.Request.Files[0];

        //    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
        //    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
        //    if (allowedExtensions.Contains(ext)) //check what type of extension  
        //    {
        //        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
        //        string myfile = name + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond;
        //        // store the file inside ~/project folder(Img)  
        //        var path = file.SaveAs( Path.Combine(Server.MapPath("~/Img"), myfile);
        //        file.SaveAs(path);

        //        //FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Images/") + fileName);
        //        //Response.Redirect(Request.Url.AbsoluteUri);
        //        return "";
        //    }
        //}
        public string UploadFile()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            var fileName = string.Empty;
            if (httpRequest.Files.Count > 0)
            {

                foreach (string file in httpRequest.Files)
                {
                    int empid = Convert.ToInt32(HttpContext.Current.Request.Form["id"]);
                    int length = httpRequest.Files[0].ContentLength;
                    byte[] pic = new byte[length];
                    httpRequest.Files[0].InputStream.Read(pic, 0, length);
                    bool isUpdated = objmodel.UploadImageAsImage(pic, empid);

                    var postedFile = httpRequest.Files[file];
                    fileName = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond + "_" + postedFile.FileName;
                    var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + fileName);
                    postedFile.SaveAs(filePath);
                    bool isUpdated2= objmodel.UpdateImageName(fileName, empid);


                    byte[] buffer = File.ReadAllBytes(filePath);
                    bool isUpdated1 = objmodel.UploadImageAsBinary(buffer, empid);

                    
                }
            }
            return fileName;
        }

        [HttpPost]
        [Route("SendEmail")]
        public string SendEmail(ManageEmployee emp)
        {
            try
            {
                using (MailMessage mm = new MailMessage("Jitendra <jeetsharma8390@gmail.com>", "jeetusharma.jee@gmail.com"))
                {
                    mm.Subject = "Registration Successful!!";
                    mm.Body = emp.Name + " is registered successfully!!";
                    //if (fuAttachment.HasFile)
                    //{
                    //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                    //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                    //}
                    System.Net.Mail.Attachment attachment;
                    string path = HttpContext.Current.Server.MapPath("~/UploadFile/" + emp.EmpImage);
                    attachment = new System.Net.Mail.Attachment(path);
                    mm.Attachments.Add(attachment);
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("jeetsharma8390@gmail.com", "Sh123Jeetxxx@$");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }
    }
}