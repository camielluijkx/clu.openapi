using AutoMapper;
using clu.openapi.Models;
using clu.openapi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clu.openapi.Controllers
{
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Route("api/v{version:apiVersion}/authors")]
    [ApiVersion("2.0")]
    public class AuthorsControllerV2 : ControllerBase
    {
        private readonly IAuthorRepository _authorsRepository;

        private readonly IMapper _mapper;

        public AuthorsControllerV2(IAuthorRepository authorsRepository, IMapper mapper)
        {
            _authorsRepository = authorsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the authors (V2)
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of Author</returns>
        /// <response code="200">Returns the list of authors</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            IEnumerable<Entities.Author> authorsFromRepo = await _authorsRepository.GetAuthorsAsync();

            return Ok(_mapper.Map<IEnumerable<Author>>(authorsFromRepo));
        }
    }
}