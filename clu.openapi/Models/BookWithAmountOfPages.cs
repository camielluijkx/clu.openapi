﻿using System;

namespace clu.openapi.Models
{
    public class BookWithAmountOfPages
    {
        public Guid Id { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorLastName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int AmountOfPages { get; set; }
    }
}