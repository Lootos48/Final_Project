﻿using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Controllers
{
    public class AuthorController : Controller
    {
        readonly IAuthorsService _authorService;
        readonly IIdentityService identityService;
        readonly IMapper mapper;

        public AuthorController(IAuthorsService serv, IIdentityService identityServ)
        {
            _authorService = serv;
            identityService = identityServ;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AuthorDTO, AuthorViewModel>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorViewModel, AuthorDTO>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<CardDTO, CardViewModel>();
                cfg.CreateMap<CardViewModel, CardDTO>();

                cfg.CreateMap<UserAuthInfoDTO, UserAuthInfoViewModel>();
                cfg.CreateMap<UserAuthInfoViewModel, UserAuthInfoDTO>();
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
        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Details(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthorDTO authorDTO = await _authorService.GetAuthor(id);

                return View(mapper.Map<AuthorViewModel>(authorDTO));
            }
            return View("Index");
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
        public async Task<ActionResult> Login([Bind(Include = "Username,Password")] UserAuthInfoViewModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = await identityService.GetUserClaims(mapper.Map<UserAuthInfoDTO>(user));
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties() { }, claimsIdentity);
                }
                catch (ValidationException ex)
                {
                    ViewBag.UserExist = ex.Message;
                    return View("Login");
                }
                return RedirectToAction("Index", "Cards");
            }
            return View("Login");
        }

        public async Task<ActionResult> Registration()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Cards");
        }

        [HttpPost]
        public async Task<ActionResult> Registration([Bind(Include = "Username,Password")] UserAuthInfoViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            var result = await identityService.CreateUser(mapper.Map<UserAuthInfoDTO>(user));
            if (!result)
            {
                return RedirectToAction("Login");
            }
            return await Login(user);
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut();
            }

            return RedirectToAction("Index", "Cards");
        }

        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Edit(int id)
        {
            AuthorDTO authorDTO = await _authorService.GetAuthor(id);

            return View(mapper.Map<AuthorViewModel>(authorDTO));
        }

        // POST: Author/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
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

        [Authorize(Roles = "Admin, RegisteredUser")]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _authorService.GetAuthor(id));
        }

        // POST: Author/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin, RegisteredUser")]
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
