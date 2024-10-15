using EMarketing.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EMarketing.Controllers
{
    public class AdminController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(tbl_admin model)
        {
            tbl_admin ad = db.tbl_admin.Where(x => x.ad_username == model.ad_username && x.ad_password == model.ad_password).SingleOrDefault();
            if (ad != null ) 
            {
                Session["ad_id"] = ad.ad_id.ToString();
                Session["admin"] = true;
                return RedirectToAction("Create");            
            }

            ViewBag.error = "Invalid username or password";
            
            return View();
        }

        [HttpGet]  
        public ActionResult Create()
        {
            if(Session["ad_id"] != null)
            {
                return View();
            }

            return RedirectToAction("login");
        }

        [HttpPost]
        public ActionResult Create(tbl_category model,HttpPostedFileBase imgFile) 
        {
            string path = uploadimage(imgFile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "image could not be uploaded....";

            }
            else
            {
                tbl_category cat = new tbl_category();
                cat.cat_name = model.cat_name;
                cat.cat_image = path;
                cat.cat_status = 1;
                cat.cat_fk_ad = Convert.ToInt32(Session["ad_id"].ToString());

                db.tbl_category.Add(cat);
                db.SaveChangesAsync();
                return RedirectToAction("ViewCategory");
            }
                
            return View(); 
        }


        public ActionResult ViewCategory(int ? page)
        {
            /*if (Session["ad_id"] == null)
            {
                return RedirectToAction("login");
            }*/

            int pageIndex = page.HasValue ? page.Value : 1;
            int pageSize = 7;

            var categories = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();

            return View(categories.ToPagedList(pageIndex,pageSize));
        }


        public string uploadimage(HttpPostedFileBase file)
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
                        path = Path.Combine(Server.MapPath("/Content/upload/"), random + Path.GetFileName(file.FileName));
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
                    Response.Write("<script> alert('Only jpg, jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script> alert ('Please select a file'); </script>");
                path = "-1";
            }

            return path;
        }

    }

}