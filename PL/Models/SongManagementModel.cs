using System.Collections.Generic;
using BL.DTOs.Songs;

namespace PL.Models
{
    public class SongManagementModel
    {
        public IList<SongManagementDTO> SongsForPublish { get; set; }

        public IList<SongManagementDTO> SongsEditedForConfirmation { get; set; }

        public IList<SongManagementDTO> SongsBeforeEdit { get; set; }
    }
}