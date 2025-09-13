using crudActivity.Dtos;
using crudActivity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace crudActivity.Seed
{
    public static class SeedData
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ActivityRepository>();

            // สร้างข้อมูลเมื่อ total = 0
            var (items, total) = await repo.ListAsync(new ActivityQuery { Page = 1, PageSize = 1, Status = "" });
            if (total > 0) return;

            var now = DateTime.UtcNow;

            var demo = new[]
            {
            new ActivityCreateDto("Company Run 2025",
                "กิจกรรมวิ่งเพื่อสุขภาพ", "รายละเอียดงานวิ่ง...", "https://picsum.photos/seed/run/800/400",
                new(){"health","sport"},
                new(){ new("https://picsum.photos/seed/run1/600/400",0), new("https://picsum.photos/seed/run2/600/400",1)},
                "published", now),

            new ActivityCreateDto("Volunteer Day",
                "จิตอาสาพัฒนาชุมชน", "รายละเอียดจิตอาสา...", "https://picsum.photos/seed/vol/800/400",
                new(){"volunteer"},
                new(){ new("https://picsum.photos/seed/vol1/600/400",0)},
                "published", now.AddDays(-7)),

            new ActivityCreateDto("Monthly Townhall",
                "อัปเดตองค์กรประจำเดือน", "รายละเอียดทาวน์ฮอลล์...", "https://picsum.photos/seed/town/800/400",
                new(){"internal"},
                new(), "draft", null)
        };

            foreach (var d in demo) await repo.CreateAsync(d);
        }
    }
}
