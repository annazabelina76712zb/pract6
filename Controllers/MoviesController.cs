using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pract6.Data;
using pract6.Models;

namespace pract6.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly WebApiDatabaseContext _context;

    public MoviesController(WebApiDatabaseContext context)
    {
        _context = context;
    }

    // GET 
    [HttpGet]
    public  IActionResult GetAll([FromQuery] string? genre, [FromQuery] int? minYear)
    {
        var query = _context.Movies.AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(m => m.Genre == genre);
        }

        if (minYear.HasValue)
        {
            query = query.Where(m => m.ReleaseYear >= minYear.Value);
        }

        var movies = query.ToListAsync();

        return Ok(movies);
    }

    // GET 
    [HttpGet("{id:int}")]
    public IActionResult GetMovie(int id)
    {
        var movie =  _context.Movies.Find(id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    // POST 
    [HttpPost]
    public  IActionResult CreateMovie(Movie movie)
    {
        if (string.IsNullOrEmpty(movie.Title) ||
            string.IsNullOrEmpty(movie.Genre) ||
            movie.DurationMinutes <= 0 ||
            movie.ReleaseYear <= 0)
 

        _context.Movies.Add(movie);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT 
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest("Id в адресе и Id в теле не совпадают.");
        }

        if (string.IsNullOrEmpty(movie.Title) ||
            string.IsNullOrEmpty(movie.Genre) ||
            movie.DurationMinutes <= 0 ||
            movie.ReleaseYear <= 0)
        {
            return BadRequest("Некорректные данные фильма.");
        }

        var existingMovie = _context.Movies.Find(id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        existingMovie.Title = movie.Title;
        existingMovie.Genre = movie.Genre;
        existingMovie.DurationMinutes = movie.DurationMinutes;
        existingMovie.ReleaseYear = movie.ReleaseYear;
        existingMovie.IsAvailable = movie.IsAvailable;

         _context.SaveChanges();

        return NoContent();
    }

    // PATCH 
    [HttpPatch("{id}")]
    public IActionResult PatchMovie(int id, Movie movie)
    {
        var existingMovie =  _context.Movies.Find(id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(movie.Title))
        {
            existingMovie.Title = movie.Title;
        }

        if (!string.IsNullOrEmpty(movie.Genre))
        {
            existingMovie.Genre = movie.Genre;
        }

        if (movie.DurationMinutes > 0)
        {
            existingMovie.DurationMinutes = movie.DurationMinutes;
        }

        if (movie.ReleaseYear > 0)
        {
            existingMovie.ReleaseYear = movie.ReleaseYear;
        }

        existingMovie.IsAvailable = movie.IsAvailable;

        _context.SaveChanges();

        return NoContent();
    }

    // DELETE
    [HttpDelete("{id}")]
    public IActionResult  DeleteMovie(int id)
    {
        var movie =  _context.Movies.Find(id);

        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}