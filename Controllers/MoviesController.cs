using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;

        public MoviesController(IGenresService genresService , IMoviesService moviesService)   // dependency injection
        {
            _genresService = genresService;
            _moviesService = moviesService;
        }


        //allowing specific extensions and size 
        private new List<string> _allowedExtensions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;



        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            //map movies to dto
            return Ok(movies);
        }

        //adding get method for returning only one movie
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        //adding get method for returning by genre id
        //[HttpGet("GetByGenreId")]
        //public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        //{
        //    var movie = await _context.Movies.FindAsync(id);

        //    if (movie == null)
        //        return NotFound();

        //    return Ok(movie);
        //}




        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster == null)
                return BadRequest("poster is required");
            //allowing specific extensions and size 
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Allowed Extensions only are jpg and png");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Allowed image size is 1MB");

            //if the data posted to an invalid id
            var isValidGenre = await _genresService.isValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id");
           
            using var DataStream = new MemoryStream();  
            await dto.Poster.CopyToAsync(DataStream); // storing poster as data straem of bytes
            
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = DataStream.ToArray(),
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                Year = dto.Year
            };

            _moviesService.Add(movie);
            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync (int id , [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null)
                return NotFound();

            //if the data posted to an invalid id
            var isValidGenre = await _genresService.isValidGenre ( dto.GenreId);
            if (isValidGenre)
                return BadRequest("Invalid Genre Id");

            if(dto.Poster != null)
            {
                //allowing specific extensions and size 
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Allowed Extensions only are jpg and png");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Allowed image size is 1MB");

                using var DataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(DataStream); // storing poster as data straem of bytes

                movie.Poster = DataStream.ToArray();

            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.StoryLine = dto.StoryLine;
            movie.Rate = dto.Rate;

            _moviesService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesService.GetById(id);

            if (movie == null) 
                return BadRequest() ;
            
            _moviesService.Delete(movie);
            return Ok(movie);
        }
        
    }
}
