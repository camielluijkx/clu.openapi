using System;

namespace clu.openapi.Models
{
    public class BookWithConcatenatedAuthorName // dto
    {
        public Guid Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}