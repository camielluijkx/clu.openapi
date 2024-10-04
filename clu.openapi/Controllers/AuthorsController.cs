using AutoMapper;
using clu.openapi.Models;
using clu.openapi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.openapi.Controllers
{
    [Produces("application/json", "application/xml")]
    // [Route("api/authors")]
    [Route("api/v{version:apiVersion}/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorsRepository;

        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorsRepository, IMapper mapper)
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of authors
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Author</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            IEnumerable<Entities.Author> authorsFromRepo = await _authorsRepository.GetAuthorsAsync();

            return Ok(_mapper.Map<IEnumerable<Author>>(authorsFromRepo));
        }

        /// <summary>
        /// Get an author by his/her id
        /// </summary>
        /// <param name="authorId">The id of the author you want to get</param>
        /// <returns>An ActionResult of type Author</returns>
        [HttpGet("{authorId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Author>> GetAuthor(Guid authorId)
        {
            Entities.Author authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Author>(authorFromRepo));
        }

        /// <summary>
        /// Update an author 
        /// </summary>
        /// <param name="authorId">The id of the author to update</param>
        /// <param name="authorForUpdate">The author with updated values</param>
        /// <returns>An ActionResult of type Author</returns>
        /// <response code="422">Validation error</response>
        [HttpPut("{authorId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, 
            Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        public async Task<ActionResult<Author>> UpdateAuthor(Guid authorId, AuthorForUpdate authorForUpdate)
        {
            Entities.Author authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(authorForUpdate, authorFromRepo);

            // update and save
            _authorsRepository.UpdateAuthor(authorFromRepo);

            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<Author>(authorFromRepo)); 
        }

        /// <summary>
        /// Partially update an author
        /// </summary>
        /// <param name="authorId">The id of the author you want to get</param>
        /// <param name="patchDocument">The set of operations to apply to the author</param>
        /// <returns>An ActionResult of type Author</returns>
        /// <remarks>Sample request (this request updates the author's **first name**)  
        /// 
        /// PATCH /authors/authorId
        /// [ 
        ///     {
        ///         "op": "replace", 
        ///         "path": "/firstname", 
        ///         "value": "new first name" 
        ///     } 
        /// ] 
        /// </remarks>
        /// <response code="200">Returns the updated author</response>
        [HttpPatch("{authorId}")]
        [Consumes("application/json-patch+json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, 
            Type = typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary))]
        public async Task<ActionResult<Author>> UpdateAuthor(Guid authorId, JsonPatchDocument<AuthorForUpdate> patchDocument)
        {
            Entities.Author authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            // map to DTO to apply the patch to
            AuthorForUpdate author = _mapper.Map<AuthorForUpdate>(authorFromRepo);

            patchDocument.ApplyTo(author, ModelState);

            // any errors when applying the patch because of badly formed document aren't caught by api controller validation
            // therefore we must manually check the model state and potentially return these errors
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            // map the applied changes on the DTO back into the entity
            _mapper.Map(author, authorFromRepo);

            // update and save
            _authorsRepository.UpdateAuthor(authorFromRepo);

            await _authorsRepository.SaveChangesAsync();

            // return the author
            return Ok(_mapper.Map<Author>(authorFromRepo));
        }
    }
}