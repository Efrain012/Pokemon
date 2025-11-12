using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniSocialMediaApp.Data;
using MiniSocialMediaApp.Models;
using System.Diagnostics;

namespace MiniSocialMediaApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var posts = _context.Posts.ToList();

            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == id);

            if (post == null) { return NotFound(); }

            else { return View(post); }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel postView)
        {
            if (postView == null) { return BadRequest(); }

            else
            {
                var author = await _userManager.GetUserAsync(User);

                if (author == null) { return BadRequest(); }

                Post newPost = new Post
                {
                    AuthorId = author!.Id,
                    Title = postView.Title,
                    Content = postView.Content,
                    CreatedAt = DateTimeOffset.UtcNow,
                };

                await _context.Posts.AddAsync(newPost);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostViewModel postView)
        {
            if (id != postView.Id) { return BadRequest(); }

            if (!ModelState.IsValid) { return View(postView); }

            var post = await _context.Posts.FindAsync(id);
            if (post == null) { return NotFound(); }

            var currentUserId = _userManager.GetUserId(User);
            if (post.AuthorId != currentUserId) { return Forbid(); }

            var timeSinceCreation = DateTimeOffset.UtcNow - post.CreatedAt;
            if (timeSinceCreation.TotalMinutes < 5) { return View(post); }

            post.Title = postView.Title;
            post.Content = postView.Content;
            post.EditedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) { return NotFound(); }

            var currentUserId = _userManager.GetUserId(User);
            if (post.AuthorId != currentUserId) { return Forbid(); }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
