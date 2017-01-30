using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Diagnostics;
using System.Web.Http.Results;
using BL.DTOs.Filters;
using BL.DTOs.Playlists;
using BL.Facades;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class PlaylistController : ApiController
    {
        public PlaylistFacade PlaylistFacade { get; set; }

        /// <summary>
        /// GET: api/Playlist
        /// Gets all playlists regardless of user
        /// </summary>
        /// <returns>HTTP 200 on success</returns>
        public IHttpActionResult Get()
        {
            return Content(HttpStatusCode.OK, PlaylistFacade.ListPlaylists(null));
        }

        /// <summary>
        /// GET: api/Playlist/User/0
        /// Gets playlists for corresponding user
        /// </summary>
        /// <param name="id">id of user to get playlists for</param>
        /// <returns>HTTP 200 on success</returns>
        [Route("~/api/Playlist/User/{id}")]
        public IHttpActionResult Get(int id)
        {
            var result = id < 0 ? null : PlaylistFacade.ListPlaylists(new PlaylistFilter {UserId = id});
            return result == null
                ? (IHttpActionResult)NotFound()
                : Content(HttpStatusCode.OK, result);
        }
        
        /// <summary>
        /// POST: api/Playlist
        /// Creates new playlist
        /// </summary>
        /// <param name="value">playlist data</param>
        /// <returns>HTTP 201 on success</returns>
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDTO>(value);
                PlaylistFacade.CreatePlaylist(playlist, playlist.UserId);
                return Content(HttpStatusCode.Created, playlist);
            }
            catch (JsonException)
            {
                Debug.WriteLine($"Playlist API - Post(...) - failed to deserialize value: {value}");
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }

        }

        /// <summary>
        /// PUT: api/Playlist/1
        /// Updates corresponding playlist
        /// </summary>
        /// <param name="id">id of playlist to update</param>
        /// <param name="value">modified playlist data</param>
        /// <returns>HTTP 200 on success</returns>
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            try
            {
                var playlistEditDto = JsonConvert.DeserializeObject<PlaylistDTO>(value);
                if (id < 0)
                {
                    return Content(HttpStatusCode.PreconditionFailed, "Playlist ID must be greater or equal to zero.");
                }
                playlistEditDto.ID = id;
                PlaylistFacade.EditPlaylist(playlistEditDto, playlistEditDto.UserId);
                return Content(HttpStatusCode.OK, playlistEditDto);
            }
            catch (JsonException)
            {
                Debug.WriteLine($"Playlist API - Put(...)-failed to deserialize value: {value}");
                return StatusCode(HttpStatusCode.BadRequest);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }

        }
 

        /// <summary>
        /// DELETE: api/Playlist/1
        /// Deletes corresponding playlist
        /// </summary>
        /// <param name="id">id of playlist to delete</param>
        /// <returns>HTTP 204 on success</returns>
        public IHttpActionResult Delete(int id)
        {
            try
            {
                PlaylistFacade.DeletePlaylist(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}