using FortniteCompetition.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortniteCompetition.Controllers
{
    [Route("competition")]
    public class CompetitionController : Controller
    {
        private readonly CompetitionDataContext _db;

        public CompetitionController(CompetitionDataContext db)
        {
            _db = db;
        }

        [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pageSize = 3;
            var totalPosts = _db.CompetitionPost.Count();
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts =
                _db.CompetitionPost
                    .Where(x => x.Date >= DateTime.Now) // for date
                    .OrderByDescending(x => x.Date)
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
            var post = _db.CompetitionPost.FirstOrDefault(x => x.Key == key);
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
        public IActionResult Create(CompetitionPost post)
        {
            if (!ModelState.IsValid)
                return View();

            post.Author = User.Identity.Name;

            _db.CompetitionPost.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Post", "Competition", new
            {
                year = post.Date.Year,
                month = post.Date.Month,
                key = post.Key
            });
        }

    }
}
