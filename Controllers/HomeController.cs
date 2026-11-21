using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TranVietHung_241230741.Models;

namespace TranVietHung_241230741.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLTaiSanContext _context;
        public HomeController(QLTaiSanContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.LoaiTaiSans = await _context.LoaiTaiSans
                .OrderBy(x => x.MaLoai)
                .ToListAsync();

            var danhSach = await _context.TaiSans
                .Include(t => t.MaLoaiNavigation)
                .Where(t => t.NguyenGia != null && t.NguyenGia >= 20)
                .ToArrayAsync();

            ViewBag.SectionTitle = "NGUYỄN VĂN AN";
            return View(danhSach);
        }

        [HttpGet]
        public async Task<IActionResult> LocTheoLoai(int maLoai, string ? tenLoai)
        {
            var query = _context.TaiSans
                .Include(t => t.MaLoaiNavigation)
                .AsQueryable();
            if(maLoai > 0)
            {
                query = query.Where(t => t.MaLoai == maLoai);
                ViewBag.SectionTitle = string.IsNullOrWhiteSpace(tenLoai)
                    ? "DANH SÁCH TÀI SẢN"
                    : tenLoai.ToUpper();
            }
            else
            {
                query = query.Where(t => t.NguyenGia != null && t.NguyenGia >= 20);
                ViewBag.SectionTitle = "NGUYỄN VĂN AN";
            }
            var ketQua = await query.OrderBy(t => t.MaTs).ToListAsync();
            return PartialView("_TranVietHung_MainContent", ketQua);

        }

      

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
