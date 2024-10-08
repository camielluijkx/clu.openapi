﻿using System.ComponentModel.DataAnnotations;

namespace clu.openapi.Models
{
    /// <summary>
    /// An author for update with FirstName and LastName fields
    /// </summary>
    public class AuthorForUpdate // dto
    {
        /// <summary>
        /// The first name of the author
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the author
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }
    }
}