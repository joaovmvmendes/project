using Microsoft.AspNetCore.Mvc;
using ProjectBackend.Models;
using ProjectBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProjectBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly ITaskItemService _taskItemService;
        
        public TaskItemsController(AppDbContext context, ITaskItemService taskItemService)
        {
            _context = context;
            _taskItemService = taskItemService;
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }

        // GET: api/TaskItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            var tasks = await _taskItemService.GetAllTasksAsync();
            return Ok(tasks);
        }

        // GET: api/TaskItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskById(int id)
        {
            var taskItem = await _taskItemService.GetTaskByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return Ok(taskItem);
        }

        // POST: api/TaskItems
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            try
            {
                await _taskItemService.AddTaskAsync(taskItem);
                return CreatedAtAction(nameof(GetTaskById), new { id = taskItem.Id }, taskItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Retorna a mensagem de erro para o cliente
            }
        }

        // PUT: api/TaskItems/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateTask(int id, TaskItem updatedTask)
        {
            var taskItem = await _taskItemService.GetTaskByIdAsync(id); // Usando o serviço para pegar o TaskItem
            
            if (taskItem == null)
            {
                return NotFound();
            }

            // Atualiza os dados da tarefa
            taskItem.Title = updatedTask.Title;
            taskItem.Description = updatedTask.Description;
            taskItem.DueDate = updatedTask.DueDate;
            taskItem.Priority = updatedTask.Priority;
            taskItem.Status = updatedTask.Status;

            try
            {
                await _taskItemService.UpdateTaskAsync(taskItem); // Chama o serviço para atualizar no banco
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

            return NoContent(); // Retorna 204 No Content quando a atualização for bem-sucedida
        }

        // DELETE: api/TaskItems/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskItem = await _taskItemService.GetTaskByIdAsync(id); // Usando o serviço para pegar o TaskItem
            
            if (taskItem == null)
            {
                return NotFound();
            }

            await _taskItemService.DeleteTaskAsync(id); // Chama o serviço para excluir a tarefa

            return NoContent(); // Retorna 204 No Content quando a exclusão for bem-sucedida
        }

    }
}