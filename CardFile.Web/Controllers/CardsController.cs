﻿using AutoMapper;
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

namespace CardFile.Web.Controllers
{
    public class CardsController : Controller
    {
        readonly ICardsService _cardsService;
        readonly IMapper mapper;

        public CardsController(ICardsService serv)
        {
            _cardsService = serv;

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
        public async Task<ActionResult> Index(int page = 1, SortOptions sortOrder = SortOptions.None, PageFiltration searchFilter = null)
        {
            IEnumerable<CardDTO> dTOs = await _cardsService.GetAll();

            dTOs = CardMainPageTransformer.Transform(dTOs, sortOrder, searchFilter);

            var cards = mapper.Map<List<CardViewModel>>(dTOs);


            int pageSize = 8;
            IEnumerable<CardViewModel> cardsPerPage = cards.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfoModel pageInfo = new PageInfoModel { PageNumber = page, PageSize = pageSize, TotalItems = cards.Count };
            IndexViewModel<CardViewModel> ivm = new IndexViewModel<CardViewModel> { PageInfo = pageInfo, PageObjects = cardsPerPage };

            return View(ivm);
        }

        public async Task<ActionResult> Like(int id)
        {
            var card = await _cardsService.GetCard(id);

            card.LikeAmount++;

            bool isUpdated = await _cardsService.UpdateCard(card);

            return RedirectToAction("Details", new { id = id });
        }

            // GET: Cards/Details/5
        public async Task<ActionResult> Details(int id)
        {
            CardDTO cardDTO = await _cardsService.GetCard(id);

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
                    card.DateOfCreate = DateTime.Now;
                    await _cardsService.CreateCard(mapper.Map<CardDTO>(card));
                }
                else
                {
                    return View();
                }

                return RedirectToAction("Details", new { id = card.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: Cards/Edit/5
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Edit(int id)
        {
            CardDTO cardDTO = await _cardsService.GetCard(id);

            return View(mapper.Map<CardViewModel>(cardDTO));
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
            catch
            {
                return View();
            }
        }

        // GET: Cards/Delete/5
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _cardsService.GetCard(id));
        }

        // POST: Cards/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, CardViewModel card)
        {
            try
            {
                await _cardsService.DeleteCard(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
