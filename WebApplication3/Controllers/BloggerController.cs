using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BloggerController : Controller
    {
        private SchoolContext _context;

        public BloggerController()
        {
            _context = new SchoolContext();
        }

        public ActionResult Index()
        {
            var list = _context.Blogs.ToList();
            var blogs = _context.Blogs.SqlQuery(@"SELECT * FROM Blogs
                                WHERE JSON_VALUE(Owner, '$.Name') = {0}", "dev")
                            .ToList();
            var blogs2 = _context.Blogs
                          .Where(b => b._Owner.Name == "dev")
                          .ToList();


            return View(_context.Blogs.ToList());
        }

        public ActionResult Search(string Owner)
        {
            // Option 1: .Net side filter using LINQ:
            var blogs = _context.Blogs
                            .Where(b => b._Owner.Name == Owner)
                            .ToList();

            // Option 2: SQL Server filter using T-SQL:
            //var blogs = _context.Blogs
            //                .FromSql<Blog>(@"SELECT * FROM Blogs
            //                    WHERE JSON_VALUE(Owner, '$.Name') = {0}", Owner)
            //                .ToList();

            return View("Index", blogs);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Blogs.Add(blog);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }
    }
}