﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneEchan.Core.Common.Extensions;
using OneEchan.Core.Models;
using OneEchan.Server.Common.Helpers;
using OneEchan.Server.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneEchan.Server.Controllers
{
    public class HomeController : Controller
    {
        private const int PAGESIZE = 20;

        private AnimateDatabaseContext _context;

        public HomeController(AnimateDatabaseContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(int page = 0)
        {
            ViewBag.CanBack = false;
            var language = Request.Headers["Accept-Language"];
            var model = _context.AnimateList.OrderByDescending(item => item.Updated_At).Select(item => new ListResult { ID = item.Id, Updated_At = item.Updated_At.ToUtc(), Name = LanguageHelper.GetLanguegeName(language, item) }).ToPagedList(page, PAGESIZE);
            return View(model);
        }

        public IActionResult Search(string keyword, int page = 0)
        {
            ViewBag.Title = $"OneEchan - {keyword}";
            ViewBag.SearchText = keyword;
            var language = Request.Headers["Accept-Language"];
            var model = _context.AnimateList.Where(item => (item.EnUs != null && item.EnUs.Contains(keyword)) || (item.JaJp != null && item.JaJp.Contains(keyword)) || (item.RuRu != null && item.RuRu.Contains(keyword)) || (item.ZhTw != null && item.ZhTw.Contains(keyword))).OrderByDescending(item => item.Updated_At).Select(item => new ListResult { ID = item.Id, Updated_At = item.Updated_At.ToUtc(), Name = LanguageHelper.GetLanguegeName(language, item) }).ToPagedList(page, PAGESIZE);
            return View("index", model);
        }
    }
}
