using ProjectBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectBackend.Data;
using System;

namespace ProjectBackend.Services
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync(int pageNumber = 1, int pageSize = 10);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskItem taskItem);
        Task UpdateTaskAsync(TaskItem taskItem);
        Task DeleteTaskAsync(int id);
    }

    public class TaskItemService : ITaskItemService
    {
        private readonly AppDbContext _context;

        public TaskItemService(AppDbContext context)
        {
            _context = context;
        }

        // Método para obter todas as tarefas
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.TaskItems
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        }

        // Método para obter uma tarefa pelo ID
        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        // Método para adicionar uma nova tarefa
        public async Task AddTaskAsync(TaskItem taskItem)
        {
            // Validação: Título não pode ser vazio ou nulo
            if (string.IsNullOrWhiteSpace(taskItem.Title))
            {
                throw new ArgumentException("O título da tarefa não pode estar vazio.");
            }

            // Validação: Data de vencimento não pode ser no passado
            if (taskItem.DueDate < DateTime.Now)
            {
                throw new ArgumentException("A data de vencimento não pode ser anterior à data atual.");
            }

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar uma tarefa existente
        public async Task UpdateTaskAsync(TaskItem taskItem)
        {
            // Validação: Título não pode ser vazio ou nulo
            if (string.IsNullOrWhiteSpace(taskItem.Title))
            {
                throw new ArgumentException("O título da tarefa não pode estar vazio.");
            }

            // Validação: Data de vencimento não pode ser no passado
            if (taskItem.DueDate < DateTime.Now)
            {
                throw new ArgumentException("A data de vencimento não pode ser anterior à data atual.");
            }

            _context.Entry(taskItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Método para excluir uma tarefa
        public async Task DeleteTaskAsync(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
