using AutoMapper;
using clu.openapi.Attributes;
using clu.openapi.Models;
using clu.openapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.openapi.Controllers
{
    [Produces("application/json", "application/xml")]
    //[Route("api/authors/{authorId}/books")]
    [Route("api/v{version:apiVersion}/authors/{authorId}/books")]
    [ApiController]
    public class BooksController : ControllerBase
    { 
        private readonly IBookRepository _bookRepository;

        private readonly IAuthorRepository _authorRepository;

        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the books for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <returns>An ActionResult of type IEnumerable of Book</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(Guid authorId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            IEnumerable<Entities.Book> booksFromRepo = await _bookRepository.GetBooksAsync(authorId);

            return Ok(_mapper.Map<IEnumerable<Book>>(booksFromRepo));
        }

        /// <summary>
        /// Get a book by id for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <param name="bookId">The id of the book</param>
        /// <returns>An ActionResult of type Book</returns>
        /// <response code="200">Returns the requested book</response>
        [HttpGet("{bookId}", Name = "GetBook")] // bugfix: ASP.NET Core doesn't automatically infer route names based on method names
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/clu.openapi.book+json")]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept, "application/json", "application/clu.openapi.book+json")] // action constraint to differentiate both http get request routes using {bookId}
        public async Task<ActionResult<Book>> GetBook(Guid authorId, Guid bookId)
        {
            if (! await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            Entities.Book bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Book>(bookFromRepo));
        }

        /// <summary>
        /// Get a book by id for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <param name="bookId">The id of the book</param>
        /// <returns>An ActionResult of type BookWithConcatenatedAuthorName</returns>
        [HttpGet("{bookId}", Name = "GetBookWithConcatenatedAuthorName")] // bugfix: ASP.NET Core doesn't automatically infer route names based on method names
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/clu.openapi.bookwithconcatenatedauthorname+json")]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept, "application/clu.openapi.bookwithconcatenatedauthorname+json")] // action constraint to differentiate both http get request routes using {bookId}
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<BookWithConcatenatedAuthorName>> GetBookWithConcatenatedAuthorName(Guid authorId, Guid bookId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            Entities.Book bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookWithConcatenatedAuthorName>(bookFromRepo));
        }

        /// <summary>
        /// Get a book by id for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <param name="bookId">The id of the book</param>
        /// <returns>An ActionResult of type BookWithAmountOfPages</returns>
        [HttpGet("{bookId}", Name = "GetBookWithAmountOfPages")] // bugfix: ASP.NET Core doesn't automatically infer route names based on method names
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/clu.openapi.bookwithamountofpages+json")]
        [RequestHeaderMatchesMediaType(HeaderNames.Accept, "application/clu.openapi.bookwithamountofpages+json")] // action constraint to differentiate both http get request routes using {bookId}
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<BookWithAmountOfPages>> GetBookWithAmountOfPages(Guid authorId, Guid bookId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            Entities.Book bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);

            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookWithAmountOfPages>(bookFromRepo));
        }

        /// <summary>
        /// Create a book for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <param name="bookForCreation">The book to create</param>
        /// <returns>An ActionResult of type Book</returns>
        /// <response code="422">Validation error</response>
        [HttpPost()]
        [Consumes("application/json", "application/clu.openapi.bookforcreation+json")]
        [RequestHeaderMatchesMediaType(HeaderNames.ContentType, "application/json",  "application/clu.openapi.bookforcreation+json")] // action constraint to differentiate both http post request routes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
            Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        [Produces("application/clu.openapi.book+json")]
        public async Task<ActionResult<Book>> CreateBook(Guid authorId, [FromBody] BookForCreation bookForCreation)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            Entities.Book bookToAdd = _mapper.Map<Entities.Book>(bookForCreation);

            bookToAdd.AuthorId = authorId; // Bugfix: author must be specified for new book

            _bookRepository.AddBook(bookToAdd);

            await _bookRepository.SaveChangesAsync();

            bookToAdd.Author = await _authorRepository.GetAuthorAsync(authorId); // Bugfix: book with author should be returned

            return CreatedAtRoute("GetBook", new { version = "1", authorId, bookId = bookToAdd.Id }, _mapper.Map<Book>(bookToAdd));
        }

        /// <summary>
        /// Create a book for a specific author
        /// </summary>
        /// <param name="authorId">The id of the book author</param>
        /// <param name="bookForCreationWithAmountOfPages">The book to create</param>
        /// <returns>An ActionResult of type Book</returns>
        /// <response code="422">Validation error</response>
        [HttpPost()]
        [Consumes("application/json", "application/clu.openapi.bookforcreationwithamountofpages+json")] // Bugfix: accept should define standard media type
        [RequestHeaderMatchesMediaType(HeaderNames.ContentType, "application/clu.openapi.bookforcreationwithamountofpages+json")] // action constraint to differentiate both http post request routes
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity,
            Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        [Produces("application/clu.openapi.bookwithamountofpages+json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Book>> CreateBookWithAmountOfPages(Guid authorId, [FromBody] BookForCreationWithAmountOfPages bookForCreationWithAmountOfPages)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            Entities.Book bookToAdd = _mapper.Map<Entities.Book>(bookForCreationWithAmountOfPages);

            bookToAdd.AuthorId = authorId; // Bugfix: author must be specified for new book

            _bookRepository.AddBook(bookToAdd);

            await _bookRepository.SaveChangesAsync();

            bookToAdd.Author = await _authorRepository.GetAuthorAsync(authorId); // Bugfix: book with author should be returned

            return CreatedAtRoute("GetBookWithAmountOfPages", new { version = "1", authorId, bookId = bookToAdd.Id }, _mapper.Map<Book>(bookToAdd)); // Fixme: amount of pages is missing in response
        }
    }
}