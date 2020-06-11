﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TasksController : Controller
    {
        private readonly AP16Context _context;

        public TasksController(AP16Context context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var aP16Context = _context.Task.Include(t => t.AssignedPerson)
                .Include(t => t.Status)
                .OrderBy(t => t.DueDate);
            return View(await aP16Context.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.AssignedPerson)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewData["AssignedPersonId"] = new SelectList(_context.Person, "Id", "DisplayName");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Description");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StatusId,DueDate,AssignedPersonId,Files")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                await AttachFiles(task);
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedPersonId"] = new SelectList(_context.Person, "Id", "DisplayName", task.AssignedPersonId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Description", task.StatusId);
            return View(task);
        }

        /// <summary>
        /// This is where we'll be uploading the filestreams for attachments and linking them to tasks
        /// </summary>
        /// <param name="task"></param>
        private async System.Threading.Tasks.Task AttachFiles(Models.Task task)
        {
            foreach (var formFile in task.Files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = System.IO.Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var attachment = new Models.Attachment
                    {
                        FileData = System.IO.File.ReadAllBytes(filePath),
                        FileName = formFile.FileName,
                        FileExtension = System.IO.Path.GetExtension(formFile.FileName)
                    };

                    task.TaskAttachment.Add(new TaskAttachment { Task = task, Attachment = attachment });
                    _context.Add(attachment);
                }
            }

            await _context.SaveChangesAsync();
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            ViewData["AssignedPersonId"] = new SelectList(_context.Person, "Id", "DisplayName", task.AssignedPersonId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Description", task.StatusId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StatusId,DueDate,AssignedPersonId")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            ViewData["AssignedPersonId"] = new SelectList(_context.Person, "Id", "DisplayName", task.AssignedPersonId);
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Description", task.StatusId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.AssignedPerson)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
