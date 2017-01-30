using AutoMapper;
using BL.DTOs.Albums;
using BL.DTOs.Events;
using BL.DTOs.Interprets;
using BL.DTOs.Playlists;
using BL.DTOs.Reviews;
using BL.DTOs.Songlists;
using BL.DTOs.Songs;
using BL.DTOs.User;
using BL.DTOs.Users;
using DAL.Entities;

namespace BL.Bootstrap
{
    public static class MappingInit
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Account.FirstName))
                    .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Account.LastName))
                    .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Account.Email))
                    .ReverseMap();

                config.CreateMap<UserAccount, UserAccountDTO>().ReverseMap();

                config.CreateMap<UserRegistrationDTO, UserAccount>();

                config.CreateMap<Review, ReviewDTO>()
                    .Include<SongReview, SongReviewDTO>()
                    .Include<AlbumReview, AlbumReviewDTO>();

                config.CreateMap<ReviewDTO, Review>()
                    .Include<SongReviewDTO, SongReview>()
                    .Include<AlbumReviewDTO, AlbumReview>();

                config.CreateMap<SongReview, SongReviewDTO>()
                    .ForMember(dest => dest.SongId, opts => opts.MapFrom(songreview => songreview.Song.ID))
                    .ReverseMap();

                config.CreateMap<AlbumReview, AlbumReviewDTO>()
                    .ForMember(dest => dest.AlbumId, opts => opts.MapFrom(albumreview => albumreview.Album.ID))
                    .ReverseMap();

                config.CreateMap<Song, SongDTO>()
                    .ForMember(dest => dest.AlbumId, opts => opts.MapFrom(song => song.Album.ID))
                    .ReverseMap();

                config.CreateMap<Album, AlbumDTO>()
                    .ForMember(dest => dest.InterpretId, opts => opts.MapFrom(album => album.Interpret.ID))
                    .ReverseMap();

                config.CreateMap<Event, EventDTO>()
                    .ForMember(dest => dest.InterpretId, opts => opts.MapFrom(e => e.Interpret.ID))
                    .ReverseMap();

                config.CreateMap<Interpret, InterpretDTO>()
                    .ReverseMap();

                config.CreateMap<Playlist, PlaylistDTO>()
                    .ForMember(dest => dest.UserId, opts => opts.MapFrom(playlist => playlist.User.ID))
                    .ReverseMap();

                config.CreateMap<SongList, SongListDTO>()
                    .ForMember(dest => dest.PlaylistId, opts => opts.MapFrom(songlist => songlist.Playlist.ID))
                    .ForMember(dest => dest.SongId, opts => opts.MapFrom(songlist => songlist.Song.ID))
                    .ReverseMap();
            });
        }
    }
}