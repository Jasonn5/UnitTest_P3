using libraryApp.Data.Models;
using libraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace libraryApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService) 
        {
            _bookService = bookService;
        }

        //Get: Shopping
        public ActionResult Index()
        {
            var result = _bookService.GetAll();

            return View(result);
        }

        //Get: Shopping/Details/5
        public ActionResult Details(Guid id)
        {
            var result = _bookService.GetById(id);
            if (result == null) 
            {
                return NotFound();
            }

            return View(result);
        }

        //Get: Shipping/Create
        public ActionResult Create() 
        {
            return View();
        }

        //Post: Shopping/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book) 
        {
            try 
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Get Shopping/Delete/5
        public ActionResult Delete(Guid id) 
        {
            var result = _bookService.GetById(id);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection) 
        {
            try
            {
                _bookService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
