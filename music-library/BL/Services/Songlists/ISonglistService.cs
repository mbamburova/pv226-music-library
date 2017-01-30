using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Songlists;

namespace BL.Services.Songlists
{
    public interface ISonglistService
    {
        void CreateSonglist(SongListDTO songListDto, int songId, int playlistId);

        void EditSonglist(SongListDTO songListDto, int songId, int playlistId);

        void DeleteSonglist(int songlistId);

        SongListDTO GetSonglist(int songlistId);

        IEnumerable<SongListDTO> ListSonglists(SongListFilter filter);
    }
}