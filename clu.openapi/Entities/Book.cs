﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clu.openapi.Entities
{
    [Table("Books")]
    public class Book // domain model
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }

        public int? AmountOfPages { get; set; }

        public Guid AuthorId { get; set; }

        public Author Author { get; set; }
    }
}