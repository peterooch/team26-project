using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BoardProject.Data;
using BoardProject.Models;
using Microsoft.AspNetCore.Http;

namespace BoardProject.Controllers
{
    public class TileDatasController : Controller
    {
        private readonly DataContext _context;
        private bool IsUserValid(out UserData user)
        {
            user = null;
            int userID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (userID == default)
                return false;

            user = _context.UserData.Find(userID);

            if (user != null && (user.IsPrimary || user.IsManager))
                return true;

            return false;
        }
        public TileDatasController(DataContext context)
        {
            _context = context;
        }

        // GET: TileDatas
        public async Task<IActionResult> Index()
        {
            if (!IsUserValid(out _))
                return RedirectToAction("Index", "Home");

            return View(await _context.TileData.ToListAsync());
        }

        // GET: TileDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tileData = await _context.TileData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tileData == null)
            {
                return NotFound();
            }

            return View(tileData);
        }

        // GET: TileDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TileDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SourceID,ID,TileName,IsPublic,TileText,ActionType,ActionContext,BackgroundColor")] TileData tileData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tileData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tileData);
        }

        // GET: TileDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tileData = await _context.TileData.FindAsync(id);
            if (tileData == null)
            {
                return NotFound();
            }
            return View(tileData);
        }

        // POST: TileDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SourceID,ID,TileName,IsPublic,TileText,ActionType,ActionContext,BackgroundColor")] TileData tileData)
        {
            if (id != tileData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tileData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TileDataExists(tileData.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tileData);
        }

        // GET: TileDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tileData = await _context.TileData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tileData == null)
            {
                return NotFound();
            }

            return View(tileData);
        }

        // POST: TileDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tileData = await _context.TileData.FindAsync(id);
            _context.TileData.Remove(tileData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TileDataExists(int id)
        {
            return _context.TileData.Any(e => e.ID == id);
        }
    }
}
