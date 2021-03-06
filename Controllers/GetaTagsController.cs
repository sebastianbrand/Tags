﻿using System.Linq;
using System.Web.Mvc;
using Geta.Tags.Implementations;
using Geta.Tags.Interfaces;
using Geta.Tags.Models;

namespace Geta.Tags.Controllers
{
    public class GetaTagsController : Controller
    {
        private readonly ITagService _tagService;

        public GetaTagsController()
        {
            _tagService = new TagService();
        }

        public JsonResult Index(string name)
        {
            var normalizedName = Normalize(name);
            var tags = _tagService.GetAllTags();

            if (IsNotEmpty(normalizedName))
            {
                tags = tags.Where(t => t.Name.ToLower().StartsWith(normalizedName.ToLower()));
            }

            var items = tags.OrderBy(t => t.Name)
                .Take(10)
                .ToList()
                .Select(ToAutoComplete);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        private static string Normalize(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }
            return name.TrimEnd('*');
        }

        private static bool IsNotEmpty(string name)
        {
            return !string.IsNullOrEmpty(name);
        }

        private static object ToAutoComplete(Tag tag)
        {
            return new { name = tag.Name, id = tag.Name };
        }
    }
}