using Dapper;
using GameBackloggerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GameBackloggerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGameController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VideoGameController> _logger;

        public VideoGameController(IConfiguration configuration, 
            ILogger<VideoGameController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoGame>>> GetAllVideoGames()
        {
            _logger.LogInformation("GetAllVideoGames Endpoint Called");

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var videoGames = await connection.QueryAsync<VideoGame>("Select * from videogames");
            return Ok(videoGames);
        }

        [HttpGet("{videoGameId}")]
        public async Task<ActionResult<VideoGame>> GetVideoGame(int videoGameId)
        {
            _logger.LogInformation("GetVideoGame For Id: {videoGameId}", 
                videoGameId);

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var videoGame = await connection.QueryAsync<VideoGame>("Select * from videogames where id = @Id",
                new { Id = videoGameId });
            return Ok(videoGame);
        }

        [HttpPost]
        public async Task<ActionResult<List<VideoGame>>> CreateVideoGame(VideoGame videoGame)
        {            
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO [dbo].[VideoGames]([Title],[HowLongToBeat])VALUES(@Title, @HowLongToBeat)", 
                videoGame);
            return Ok(await SelectAllVideoGames(connection));
        }

        private static async Task<IEnumerable<VideoGame>> SelectAllVideoGames(SqlConnection sqlConnection)
        {
            return await sqlConnection.QueryAsync<VideoGame>("Select * From VideoGames");
        }
    }
}
