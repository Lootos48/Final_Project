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

namespace CardFile.Web.Controllers
{
    public class CardsController : Controller
    {
        readonly ICardsService _cardsService;
        readonly IAuthorsService _authorsService;
        readonly IMapper mapper;
        readonly ILikeService _likeService;

        private IAuthenticationManager _authManger
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
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
            return View(Pagination<CardViewModel>.PaginateObjects(cards, page, pageSize));
        }

        [Authorize(Roles = "RegisteredUser,Admin")]
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

        public async Task<ActionResult> Details(int id)
        {
            CardDTO cardDTO = await _cardsService.GetCard(id);
            if (CurrentUserUsername != "")
            {
                var author = await _authorsService.GetAuthor(a => a.Username == CurrentUserUsername);
                ViewBag.IsAuthorAlreadyLikeCard = _likeService.IsAuthorAlreadyLikeCard(id, author.Id);
            }
            return View(mapper.Map<CardViewModel>(cardDTO));
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
            catch (Exception ex)
            {
                return View();
            }


            return RedirectToAction("Details", new { id = card.Id });
        }

        // GET: Cards/Edit/5
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Edit(int id)
        {
            if (!await IsOwnerOfCard(id) && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }

            CardDTO cardDTO = await _cardsService.GetCard(id);
            return  View(mapper.Map<CardViewModel>(cardDTO));
        }

        // POST: Cards/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CardViewModel card)
        {
            try
            {
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
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await IsOwnerOfCard(id) && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }

            try
            {
                await _cardsService.DeleteCard(id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [NonAction]
        private async Task<bool> IsOwnerOfCard(int id)
        {
            var card = await _cardsService.GetCard(id);
            return CurrentUserUsername == card.Author.Username;
        }
    }
}
