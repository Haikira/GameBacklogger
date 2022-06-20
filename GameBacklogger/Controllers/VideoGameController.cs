using Dapper;
using GameBacklogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GameBacklogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VideoGameController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetAllVideoGames()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var videoGames = await connection.QueryAsync<VideoGame>("Select * from videogames");
            return Ok(videoGames);
        }

        [HttpGet("{videoGameId}")]
        public async Task<ActionResult<VideoGame>> GetVideoGame(int videoGameId)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var videoGame = await connection.QueryAsync<VideoGame>("Select * from videogames where id = @Id",
                new { Id = videoGameId });
            return Ok(videoGame);
        }
    }
}
