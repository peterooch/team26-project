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
    public class BoardDatasController : Controller
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
        public BoardDatasController(DataContext context)
        {
            _context = context;
        }

        // GET: BoardDatas
        public async Task<IActionResult> Index()
        {
            if (!IsUserValid(out _))
                return RedirectToAction("Index", "Home");

            return View(await _context.BoardData.ToListAsync());
        }

        // GET: BoardDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardData = await _context.BoardData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (boardData == null)
            {
                return NotFound();
            }

            return View(boardData);
        }

        // GET: BoardDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoardDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TileIDs,ID,BoardName,IsPublic,BoardHeader,BackgroundColor,TextColor,FontSize,Spacing")] BoardData boardData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boardData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boardData);
        }

        // GET: BoardDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardData = await _context.BoardData.FindAsync(id);
            if (boardData == null)
            {
                return NotFound();
            }
            return View(boardData);
        }

        // POST: BoardDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TileIDs,ID,BoardName,IsPublic,BoardHeader,BackgroundColor,TextColor,FontSize,Spacing")] BoardData boardData)
        {
            if (id != boardData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boardData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardDataExists(boardData.ID))
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
            return View(boardData);
        }

        // GET: BoardDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardData = await _context.BoardData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (boardData == null)
            {
                return NotFound();
            }

            return View(boardData);
        }

        // POST: BoardDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boardData = await _context.BoardData.FindAsync(id);
            _context.BoardData.Remove(boardData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardDataExists(int id)
        {
            return _context.BoardData.Any(e => e.ID == id);
        }
    }
}
