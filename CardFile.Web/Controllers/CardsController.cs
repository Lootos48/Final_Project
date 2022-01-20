using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Web.Enums;
using CardFile.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CardFile.Web.Util;
using CardFile.BLL.Services;
using Microsoft.Owin.Security;
using CardFile.BLL.Infrastructure;
using System.Net;

namespace CardFile.Web.Controllers
{
    public class CardsController : Controller
    {
        readonly ICardsService _cardsService;
        readonly IAuthorsService _authorsService;
        readonly IMapper mapper;
        readonly ILikeService _likeService;

        /// <summary>
        /// Поле для работы с OWIN-контекстом
        /// </summary>
        private IAuthenticationManager _authManger
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
                return _authManger.User.Identity.Name;
            }
        }

        public CardsController(ICardsService serv, IAuthorsService authorsServ, ILikeService _likeServ)
        {
            _likeService = _likeServ;
            _cardsService = serv;
            _authorsService = authorsServ;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CardDTO, CardViewModel>()
                    .ForMember(s => s.Author, o => o.UseDestinationValue());

                cfg.CreateMap<CardViewModel, CardDTO>()
                    .ForMember(s => s.Author, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorDTO, AuthorViewModel>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorViewModel, AuthorDTO>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());
            });

            mapper = config.CreateMapper();
        }

        // GET: Cards
        public async Task<ActionResult> Index(int page = 1, PageFilter searchFilter = null, SortOptions sortOrder = SortOptions.Newer)
        {
            IEnumerable<CardDTO> dTOs = await _cardsService.GetAll();

            dTOs = PageFiltration.Transform(dTOs, sortOrder, searchFilter);

            var cards = mapper.Map<List<CardViewModel>>(dTOs);

            int pageSize = 8;
            return View(Pagination<CardViewModel>.PaginateObjects(cards, page, pageSize, searchFilter, sortOrder));
        }

        [Authorize(Roles = "RegisteredUser")]
        public async Task<ActionResult> Like(int id)
        {
            var author = await _authorsService.GetAuthor(a => a.Username == CurrentUserUsername);
            var result = await _likeService.LikeCard(id, author.Id);
            if (result)
            {
                return RedirectToAction("Details", new { id = id });
            }
            return new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                CardDTO cardDTO = await _cardsService.GetCard(id.Value);

                if (CurrentUserUsername != "")
                {
                    var author = await _authorsService.GetAuthor(a => a.Username == CurrentUserUsername);
                    ViewBag.IsAuthorAlreadyLikeCard = _likeService.IsAuthorAlreadyLikeCard(id.Value, author.Id);
                }
                return View(mapper.Map<CardViewModel>(cardDTO));
            }
            catch (ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: Cards/Create
        [Authorize(Roles = "Admin, RegisteredUser")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CardViewModel card)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var author = await _authorsService.GetAuthor(a => a.Username == CurrentUserUsername);
                    card.AuthorId = author.Id;
                    card.DateOfCreate = DateTime.Now;
                    card = mapper.Map<CardViewModel>(await _cardsService.CreateCard(mapper.Map<CardDTO>(card)));
                }
                else
                {
                    return View();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View("Create");
            }

            return RedirectToAction("Details", new { id = card.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else if (!await IsOwnerOfCard(id.Value) && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }


                CardDTO cardDTO = await _cardsService.GetCard(id.Value);
                return View(mapper.Map<CardViewModel>(cardDTO));
            }
            catch (ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // POST: Cards/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CardViewModel card)
        {
            try
            {
                if (!await IsOwnerOfCard(card.Id) && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (ModelState.IsValid)
                {
                    await _cardsService.UpdateCard(mapper.Map<CardDTO>(card));
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Details", new { id = card.Id });
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else if (!await IsOwnerOfCard(id.Value) && !User.IsInRole("Admin"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                await _cardsService.DeleteCard(id.Value);

                return RedirectToAction("Index");
            }
            catch(ObjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Метод для проверки является ли текущий пользователь владельцем редактируемой карточки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<bool> IsOwnerOfCard(int id)
        {
            var card = await _cardsService.GetCard(id);
            return CurrentUserUsername == card.Author.Username;
        }
    }
}
