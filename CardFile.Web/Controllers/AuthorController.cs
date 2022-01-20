using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
using CardFile.BLL.Interfaces;
using CardFile.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Controllers
{
    /// <summary>
    /// Контроллер для реализации функций по взаимодействию с профилями пользователей
    /// </summary>
    public class AuthorController : Controller
    {
        readonly IAuthorsService _authorService;
        readonly IIdentityService _identityService;
        readonly IMapper mapper;

        /// <summary>
        /// Поле для работы с OWIN-контекстом
        /// </summary>
        private IAuthenticationManager _authManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Свойство для получения никнейма текущего аутентифицированного пользователя
        /// </summary>
        private string CurrentUserUsername
        {
            get
            {
                return _authManager.User.Identity.Name;
            }
        }

        public AuthorController(IAuthorsService serv, IIdentityService identityServ)
        {
            _authorService = serv;
            _identityService = identityServ;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AuthorDTO, AuthorViewModel>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorViewModel, AuthorDTO>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<CardDTO, CardViewModel>();
                cfg.CreateMap<CardViewModel, CardDTO>();

                cfg.CreateMap<UserAuthInfoDTO, LoginViewModel>();
                cfg.CreateMap<LoginViewModel, UserAuthInfoDTO>();
            });

            mapper = config.CreateMapper();
        }

        // GET: Author
        public async Task<ActionResult> Index(int page = 1)
        {
            IEnumerable<AuthorDTO> dTOs = await _authorService.GetAll();

            var authors = mapper.Map<List<AuthorViewModel>>(dTOs);

            int pageSize = 5;
            IEnumerable<AuthorViewModel> authorsPerPage = authors.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfoModel pageInfo = new PageInfoModel { PageNumber = page, PageSize = pageSize, TotalItems = authors.Count };
            IndexViewModel<AuthorViewModel> ivm = new IndexViewModel<AuthorViewModel> { PageInfo = pageInfo, PageObjects = authorsPerPage };

            return View(ivm);
        }

        // GET: Author/Details/5
        [HttpGet]
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (User.Identity.IsAuthenticated)
                {
                    AuthorDTO authorDTO = await _authorService.GetAuthor(id.Value);

                    return View(mapper.Map<AuthorViewModel>(authorDTO));
                }
                return View("Index");
            }
            catch (ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            
        }

        public async Task<ActionResult> AuthorProfile(string username)
        {
            if (username == "" || username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AuthorDTO author = await _authorService.GetAuthor(a => a.Username == username);

            if (author == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View("Details", mapper.Map<AuthorViewModel>(author));
        }

        public async Task<ActionResult> Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Cards");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = await _identityService.GetUserClaims(mapper.Map<UserAuthInfoDTO>(user));
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);
                }
                catch (ValidationException ex)
                {
                    foreach (string errorMessage in GetErrorsArray(ex.Message))
                    {
                        ModelState.AddModelError("", errorMessage);
                    }
                    return View("Login");
                }
                return RedirectToAction("Index", "Cards");
            }
            return View("Login");
        }

        public async Task<ActionResult> Registration()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Cards");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel registerInfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                bool isCreated = await _identityService.CreateUser(new UserAuthInfoDTO()
                {
                    Username = registerInfo.Username,
                    Password = registerInfo.Password
                });

                if (isCreated)
                {
                    AuthorViewModel author = new AuthorViewModel()
                    {
                        Username = registerInfo.Username,
                        FirstName = registerInfo.FirstName,
                        SecondName = registerInfo.SecondName
                    };
                    await _authorService.CreateAuthor(mapper.Map<AuthorDTO>(author));
                }
            }
            catch (ValidationException ex)
            {
                foreach (string errorMessage in GetErrorsArray(ex.Message))
                {
                    ModelState.AddModelError("", errorMessage);
                }
                return View("Registration");
            }

            return await Login(new LoginViewModel() { Username = registerInfo.Username, Password = registerInfo.Password });
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                /*var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut();*/
                _authManager.SignOut();
            }

            return RedirectToAction("Index", "Cards");
        }

        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (!(await IsCurrentUserAnAuthor(id.Value)) && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                AuthorDTO authorDTO = await _authorService.GetAuthor(id.Value);

                return View(mapper.Map<AuthorViewModel>(authorDTO));
            }
            catch (ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            
        }

        // POST: Author/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AuthorViewModel author)
        {
            try
            {
                if (!(await IsCurrentUserAnAuthor(author.Id)) && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (ModelState.IsValid)
                {
                    await _authorService.UpdateAuthor(mapper.Map<AuthorDTO>(author));
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Details", new { id = author.Id });
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex);
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                await _authorService.DeleteAuthor(id.Value);

                return RedirectToAction("Index");
            }
            catch(ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Метод определения являеться ли текущий пользователь - редактируемым автором
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<bool> IsCurrentUserAnAuthor(int id)
        {
            var author = await _authorService.GetAuthor(id);
            return CurrentUserUsername == author.Username;
        }

        /// <summary>
        /// Метод для получения строчного массива всех ошибок валидации
        /// </summary>
        /// <param name="exceptionMessage">Строку всех ошибок</param>
        /// <returns>Массив строк с ошибками валидации</returns>
        [NonAction]
        private string[] GetErrorsArray(string exceptionMessage)
        {
            string[] errors = exceptionMessage.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return errors;
        }
    }
}
