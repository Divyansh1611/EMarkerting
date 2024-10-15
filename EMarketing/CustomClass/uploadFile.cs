using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Mvc;
namespace EMarketing.CustomClass
{
    public class uploadFile
    {
        public static string uploadimage(HttpPostedFileBase file, HttpContextBase context)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();

            if (file != null && file.ContentLength > 0)
            {

                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(context.Server.MapPath("/Content/upload/"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    context.Response.Write("<script> alert('Only jpg, jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                context.Response.Write("<script> alert ('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }
    }
}