using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FortniteCompetition.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FortniteCompetition.Controllers
{
    [Route("news")]
    public class NewsController : Controller
    {
        private readonly NewsDataContext _db;

        public NewsController(NewsDataContext db)
        {
            _db = db;
        }

        [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pageSize = 3;
            var totalPosts = _db.NewsPosts.Count();
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts =
                _db.NewsPosts
                    .OrderByDescending(x => x.Posted)
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToArray();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView(posts);

            return View(posts);
        }

        [Route("{year:min(2000)}/{month:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            var post = _db.NewsPosts.FirstOrDefault(x => x.Key == key);
            return View(post);
        }

        [Authorize]
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult Create(NewsPost post)
        {
            if (!ModelState.IsValid)
                return View();

            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;

            _db.NewsPosts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Post", "News", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }
    }
}
