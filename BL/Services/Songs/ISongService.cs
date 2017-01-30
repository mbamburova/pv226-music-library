using System.Collections.Generic;
using BL.DTOs.Filters;
using BL.DTOs.Songs;

namespace BL.Services.Songs
{
    public interface ISongService
    {
        int CreateSong(SongDTO songDto, int albumId);

        void EditSong(SongDTO songDto, int albumId, params int[] songReviewIds);

        void DeleteSong(int songId);

        SongDTO GetSong(int songId);

        IEnumerable<SongDTO> ListSongs(SongFilter filter);

        IEnumerable<SongDTO> ListSongsShuffle(SongFilter filter);

        void MakeSongPublic(SongDTO songDto, int albumId);
    }
}