﻿using System.ComponentModel.DataAnnotations;
using Orders2.Shared.Interfaces;
namespace Orders2.Shared.Entities;

public class Category : IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Categoría")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;
}
