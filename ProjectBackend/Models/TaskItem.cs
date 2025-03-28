using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectBackend.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        public required string Title { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "A prioridade é obrigatória")]
        [Range(1, 5, ErrorMessage = "A prioridade deve estar entre 1 e 5.")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        public required string Status { get; set; }
    }
}
