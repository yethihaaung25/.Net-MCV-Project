using PagedList;
using PagedList.Mvc;
using SampleProjectPostTitle.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleProjectPostTitle.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnection _connection = new DbConnection();
        private int? page;

        // GET: Home
        public ActionResult Index()
        {
            List<Posts> posts = _connection.Posts.OrderByDescending(m => m.Created_Date).ToList();
            string postSearch = Request["postSearchKey"];
            List<Posts> search = _connection.Posts
                .Where(m => m.Title.Contains(postSearch) || m.Description.Contains(postSearch)).ToList();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));

            if (postSearch != null)
            {
                return View(search);
            }

            return View(posts);
        }


        [HttpPost]
        public ActionResult Create(Posts model)
        {
            string imgName = Guid.NewGuid() + "__" +Path.GetFileName(Request.Files["image"].FileName);
            string target_file = Server.MapPath("..\\Image\\");
            if(!Directory.Exists(target_file))
            {
                Directory.CreateDirectory(target_file);
            }

            if(imgName != "")
            {
                Request.Files["image"].SaveAs(target_file+imgName);
            }
            model.Image = imgName.ToString();
            model.Created_Date = DateTime.Now;
            model.Updated_Date = DateTime.Now;
            _connection.Posts.Add(model);
            _connection.SaveChanges();
            TempData["successMessage"] = "Post Create Successfully.";
            return RedirectToAction("index");
        }


        public ActionResult Detail(int ID)
        {
            Posts entity = _connection.Posts
                .Where(m => m.ID == ID)
                .Select(m => m)
                .FirstOrDefault();

            return View(entity);
        }

        public ActionResult Edit(int ID)
        {
            Posts entity = _connection.Posts
               .Where(m => m.ID == ID)
               .Select(m => m)
               .FirstOrDefault();

            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(Posts model, int ID)
        {
            Posts entity = _connection.Posts
               .Where(m => m.ID == ID)
               .Select(m => m)
               .FirstOrDefault();

            if (entity != null)
            {
                entity.Title = model.Title;
                entity.Description = model.Description;
                entity.Location = model.Location;
                entity.Price = model.Price;
                entity.Rate = model.Rate;
            }


            if (model.Image != null)
            {
                string oldImageName = entity.Image;
                var filePath = Server.MapPath("..\\Image\\") + oldImageName;
                if (oldImageName != null)
                {
                    FileInfo file = new FileInfo(filePath);
                    file.Delete();
                }

                string imgName = Guid.NewGuid() + "__" + Path.GetFileName(Request.Files["image"].FileName);
                string target_file = Server.MapPath("..\\Image\\");

                if (!Directory.Exists(target_file))
                {
                    Directory.CreateDirectory(target_file);
                }

                if (imgName != "")
                {
                    Request.Files["image"].SaveAs(target_file + imgName);
                }

                entity.Image = imgName.ToString();
            }
            
            _connection.SaveChanges();
            TempData["updateMessage"] = "Post Update Successfully.";
            return RedirectToAction("index");
        }

        public ActionResult Delete(int ID)
        {
            Posts entity = _connection.Posts
               .Where(m => m.ID == ID)
               .Select(m => m)
               .FirstOrDefault();

           if(entity != null)
            {
                _connection.Posts.Remove(entity);
                string oldImageName = entity.Image;
                var filePath = Server.MapPath("..\\Image\\") + oldImageName;
                if (oldImageName != null)
                {
                    FileInfo file = new FileInfo(filePath);
                    file.Delete();
                }
                _connection.SaveChanges();
            }
            return RedirectToAction("/");
        }
    }
}