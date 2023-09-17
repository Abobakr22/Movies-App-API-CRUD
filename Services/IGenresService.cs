namespace MoviesApp.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Add (Genre genre);
        Genre Update (Genre genre);   //not async so not task
        Genre Delete (Genre genre);   //not async so not task

        Task<bool> isValidGenre (byte id);


    }
}
