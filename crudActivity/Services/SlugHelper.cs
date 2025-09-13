using System;
using System.Text.RegularExpressions;

namespace crudActivity.Services
{
    public static class SlugHelper
    {
        public static string Slugify(string s)
        {
            //ทำให้ URLfriendly => ทำข้อความให้อ่านง่ายบน url

            var slug = s.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, "-{2,}", "-").Trim('-');
            return string.IsNullOrWhiteSpace(slug)
                ? $"activity-{(Guid.NewGuid().ToString("N"))[..6]}"
                : slug;
        }
    }
}
