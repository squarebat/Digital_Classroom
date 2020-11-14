using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using DigiClassroom.Models;
using DigiClassroom.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Web;
using System.IO;
using DigiClassroom.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace DigiClassroom.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly IClassroomRepository _classRepo;
        private readonly IClassroomUserRepository _classUserRepo;
        private readonly IBlackBoardRepository _boardRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        //private readonly System.Web.Mvc.HtmlHelper _htmlHelper;
        public ClassroomController(IClassroomRepository classRepo,IClassroomUserRepository classUser, 
            IBlackBoardRepository boardRepo, IHostingEnvironment hostingEnvironment,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager/*, System.Web.Mvc.HtmlHelper htmlHelper*/)
        {
            _classRepo = classRepo;
            _classUserRepo = classUser;
            _boardRepo = boardRepo;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            //_htmlHelper = htmlHelper;
        }
        public ViewResult Index()
        {
            var model = _classRepo.GetAllClassrooms();
            return View(model);
        }
        public ViewResult Details(int id)
        {
            Classroom Classroom = _classRepo.GetClassroom(id);
            if (Classroom == null)
            {
                Response.StatusCode = 404;
                return View("NotFound"); 
            }
            return View(Classroom);
        }
        [Authorize]

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Classroom model)
        {
            string Id = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                Classroom newClass = new Classroom
                {
                    title = model.title,
                    description = model.description,
                    AppUserID = Id,
                    time_created = DateTime.Now
                };
                _classRepo.Add(newClass);

                ClassroomUser newClassUser = new ClassroomUser
                {
                    ClassroomId = newClass.ID,
                    AppUserId = Id,
                    Role = "Mentor"
                };
                _classUserRepo.Add(newClassUser);

                return RedirectToAction("Home", new { id = newClass.ID });
            }
            return View();
        }
        [Authorize]

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Classroom Class = _classRepo.GetClassroom(id);
            Classroom newClass = new Classroom
            {
                ID = Class.ID,
                title = Class.title,
                description = Class.description,
                AppUserID = Class.AppUserID
            };
            return View(newClass);
        }
        [HttpPost]
        public IActionResult Edit(Classroom model)
        {
            if (ModelState.IsValid)
            {
                Classroom Class = _classRepo.GetClassroom(model.ID);
                Class.title = model.title;
                Class.description = model.description;
                Classroom updatedClass = _classRepo.Update(Class);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Classroom Class = _classRepo.GetClassroom(id);
            if (Class == null)
            {
            }
            return View(Class);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var Class = _classRepo.GetClassroom(id);
            _classRepo.Delete(Class.ID);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Home(int id)
        {
            Classroom Classroom = _classRepo.GetClassroom(id);
            if (Classroom == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            ClassroomHomeViewModel chvm = new ClassroomHomeViewModel();
            chvm.Classroom = Classroom;
            chvm.BlackBoards = _boardRepo.GetClassBlackBoards(id);
            chvm.ClassroomMentors = _classUserRepo.GetClassroomMentors(id);
            chvm.ClassroomStudents = _classUserRepo.GetClassroomStudents(id);
            return View(chvm);
        }
        [HttpGet]
        public IActionResult BlackBoard(int id)
        {
            ViewBag.ClassId = id;
            return View();
        }
        [HttpPost]
        public IActionResult BlackBoard(ClassroomHomeViewModel model)
        {
            string Id = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                string filename = null;
                List<string> files = new List<string>();
                if (model.BlackBoardViewModel.Files != null)
                {
                    foreach (IFormFile file in model.BlackBoardViewModel.Files)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "blackboard");
                        filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                        files.Add(filename);
                        string filePath = Path.Combine(uploadsFolder, filename);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                BlackBoard newBoard = new BlackBoard
                {
                    ClassroomId = Convert.ToInt32(model.BlackBoardViewModel.ClassId),
                    AppUserId = Id,
                    content = model.BlackBoardViewModel.content,
                    FilesPaths = string.Join(",", files)
                };
                _boardRepo.Add(newBoard);
            }
            return RedirectToAction("Home", new { id = model.BlackBoardViewModel.ClassId });
        }
        public ViewResult BlackBoardIndex()
        {
            var model = _boardRepo.GetAllBlackBoards();
            return View(model);
        }

        public IActionResult DeleteBlackBoard(int id)
        {
            BlackBoard bb = _boardRepo.GetBlackBoard(id);
            if (bb == null)
            {
                return View("NotFound");
            }
            return View("BlackBoardDelete", bb);
        }
        [HttpPost, ActionName("DeleteBlackBoard")]
        public IActionResult BlackBoardDeleteConfirmed(int id)
        {
            BlackBoard bb = _boardRepo.GetBlackBoard(id);
            _boardRepo.Delete(bb.Id);
            return RedirectToAction("Home", new { id = bb.ClassroomId });
        }
    }
}
