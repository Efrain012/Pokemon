using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Proyecto.Models;

namespace API_Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .ToListAsync();
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(Guid id)
        {
            var taskItem = await _context.TaskItems
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        // POST: api/TaskItems
        [HttpPost]
        public async Task<ActionResult<TaskItem>> PostTaskItem(TaskItem taskItem)
        {
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskItem", new { id = taskItem.Id }, taskItem);
        }

        // PUT: api/TaskItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskItem(Guid id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TaskItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(Guid id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/TaskItems/5/dependencies/{prerequisiteId}
        [HttpPost("{taskItemId}/dependencies/{prerequisiteId}")]
        public async Task<IActionResult> AddDependency(Guid taskItemId, Guid prerequisiteId)
        {
            var taskItem = await _context.TaskItems.FindAsync(taskItemId);
            if (taskItem == null)
                return NotFound("TaskItem not found");

            var prerequisite = await _context.TaskItems.FindAsync(prerequisiteId);
            if (prerequisite == null)
                return NotFound("Prerequisite TaskItem not found");

            if (taskItemId == prerequisiteId)
            {
                return BadRequest("A task cannot depend on itself");
            }

            if (!taskItem.DependsOn.Contains(prerequisiteId))
            {
                taskItem.DependsOn.Add(prerequisiteId);
                await _context.SaveChangesAsync();
            }

            return Ok(taskItem);
        }

        // GET: api/TaskItems/5/dependencies
        [HttpGet("{taskItemId}/dependencies")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetDependencies(Guid taskItemId)
        {
            var taskItem = await _context.TaskItems.FindAsync(taskItemId);
            if (taskItem == null)
                return NotFound();

            var dependencies = await _context.TaskItems
                .Where(t => taskItem.DependsOn.Contains(t.Id))
                .ToListAsync();

            return Ok(dependencies);
        }

        // DELETE: api/TaskItems/5/dependencies/{prerequisiteId}
        [HttpDelete("{taskItemId}/dependencies/{prerequisiteId}")]
        public async Task<IActionResult> RemoveDependency(Guid taskItemId, Guid prerequisiteId)
        {
            var taskItem = await _context.TaskItems.FindAsync(taskItemId);
            if (taskItem == null)
                return NotFound();

            if (taskItem.DependsOn.Remove(prerequisiteId))
            {
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool TaskItemExists(Guid id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }
    }
}