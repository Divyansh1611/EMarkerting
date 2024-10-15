using EMarketing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using EMarketing.CustomClass;

namespace EMarketing.Controllers
{
    public class UserController : Controller
    {
        dbemarketingEntities db = new dbemarketingEntities();
        // GET: User
        public ActionResult Index(int ? page)
        {
            int pageIndex = page.HasValue ? page.Value : 1;
            int pageSize = 7;

            var categories = db.tbl_category.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();

            return View(categories.ToPagedList(pageIndex, pageSize));
        }

        [HttpGet]
        public ActionResult Signup() 
        {
        
            return View();
        }

        [HttpPost]
        public ActionResult Signup(tbl_user model, HttpPostedFileBase imgFile) {

            var imgPath = uploadFile.uploadimage(imgFile, HttpContext);

            if (imgPath.Equals("-1"))
            {
                ViewBag.error = "Image Cannot be Uploaded.";

            }
            else
            {
                tbl_user user = new tbl_user();
                user.u_name = model.u_name;
                user.u_email = model.u_email;
                user.u_password = model.u_password;
                user.u_image = imgPath;
                user.u_contact = model.u_contact;
                db.tbl_user .Add(user);
                db.SaveChangesAsync();
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_user model)
        {

            var ad = db.tbl_user.Where(x => x.u_email == model.u_email && x.u_password == model.u_password).SingleOrDefault();

            if (ad != null)
            {
                Session["u_id"] = ad.u_id.ToString();
                Session["admin"] = false;
                return RedirectToAction("CreateAd");
            }
            ViewBag.error = "Either Username or Password is wrong.";
            return View();
        }

        public ActionResult CreateAd()
        {
            if (Session["u_id"] != null)
            {
                List<tbl_category> li = db.tbl_category.ToList();
                ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

                return View();
            }

            return RedirectToAction("Login");
/*
            List<tbl_category> li = db.tbl_category.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            return View();*/
        }

         
        [HttpPost]
        public ActionResult CreateAd(tbl_product model, HttpPostedFileBase imgfile)
         {
            //Response.Write("<script> window.alert(\" This is Done.\"); </script>");
            // it takes the file and the request Context.
            string imgPath = uploadFile.uploadimage(imgfile, HttpContext);

            if (imgPath.Equals("-1"))
            {
                List<tbl_category> li = db.tbl_category.ToList();
                ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");
                ViewBag.error = "Image could not be uploaded....";
                return View("CreateAd");
            }
            else
            {
                tbl_product new_product = new tbl_product();

                new_product.pro_name = model.pro_name;
                new_product.pro_price = model.pro_price;
                new_product.pro_desc = model.pro_desc;
                new_product.pro_image = imgPath;
                new_product.pro_fk_cat = model.pro_fk_cat;
                new_product.pro_fk_user = Convert.ToInt32(Session["u_id"].ToString()); 

                db.tbl_product.Add(new_product);
                db.SaveChangesAsync();

                
                return RedirectToAction("CreateAd");
            }
        }

        public ActionResult Ads(int ? id, int ? page)
        {

            int pageIndex = page.HasValue ? page.Value : 1;
            int pageSize = 7;

            var categories = db.tbl_product.Where(x => x.pro_fk_cat == id).ToList();

            return View(categories.ToPagedList(pageIndex, pageSize));

        }

        public ActionResult ViewAd(int ? id)
        {
            Addviewmodel ad = new Addviewmodel();

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
            ad.pro_id = p.pro_id;
            ad.pro_name = p.pro_name;
            ad.pro_image = p.pro_image;
            ad.pro_price = p.pro_price;

            tbl_category cat = db.tbl_category.Where(x => x.cat_id == p.pro_fk_cat).SingleOrDefault();
            ad.cat_name = cat.cat_name;

            tbl_user u = db.tbl_user.Where(x => x.u_id == p.pro_fk_user).SingleOrDefault();

            ad.u_name = u.u_name; 
            ad.u_image = u.u_image;
            ad.u_contact = u.u_contact;

            return View(ad);
        }

        public ActionResult SignOut()
        {

            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}