namespace MoviesApp.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;

        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var movies = await _context.Movies
                .OrderByDescending(x => x.Rate)
                .Include(g => g.Genre)
                .ToListAsync();

            return movies;

        }

        public async Task<Movie> GetById(int id)
        {
           
           // var movie = await _context.Movies.FindAsync(id);
            var movie = await _context.Movies
                .Include(g => g.Genre)
                .SingleOrDefaultAsync(m => m.Id == id);

                 return movie;
        }
        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Update(Movie movie)
        {
           _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
