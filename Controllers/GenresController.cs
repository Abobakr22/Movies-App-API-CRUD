

using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;  //implement service of interface

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }


        //private readonly ApplicationDbContext _context;  //creating instance from database

        //public GenresController(ApplicationDbContext context)  //injecting db inside a constructor of a controller
        //{
        //    _context = context;
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);
        }


        [HttpPost]

        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await _genresService.Add(genre);
            return Ok(genre);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
            {
                return NotFound($"no genre was found for id {id}");
            }
            genre.Name = dto.Name;

            _genresService.Update(genre);

            return Ok(genre);
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
            {
                return NotFound($"no genre was found for id {id}");
            }
            _genresService.Delete(genre);
            return Ok(genre);

        }
    }
}