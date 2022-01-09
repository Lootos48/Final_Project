using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Controllers
{
    public class AuthorController : Controller
    {
        readonly IAuthorsService _authorService;
        readonly IMapper mapper;

        public AuthorController(IAuthorsService serv)
        {
            _authorService = serv;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AuthorDTO, AuthorViewModel>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorViewModel, AuthorDTO>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<CardDTO, CardViewModel>();
                cfg.CreateMap<CardViewModel, CardDTO>();
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
        public async Task<ActionResult> Details(int id)
        {
            AuthorDTO authorDTO = await _authorService.GetAuthor(id);

            return View(mapper.Map<AuthorViewModel>(authorDTO));
        }

        // GET: Author/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {

            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AuthorViewModel author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AuthorDTO authorDTO = await _authorService.CreateAuthor(mapper.Map<AuthorDTO>(author));
                    author.Id = authorDTO.Id;
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Details", new { id = author.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Cards/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            AuthorDTO authorDTO = await _authorService.GetAuthor(id);

            return View(mapper.Map<AuthorViewModel>(authorDTO));
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AuthorViewModel author)
        {
            try
            {
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
            catch
            {
                return View();
            }
        }

        // GET: Author/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _authorService.GetAuthor(id));
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, AuthorViewModel author)
        {
            try
            {
                await _authorService.DeleteAuthor(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
