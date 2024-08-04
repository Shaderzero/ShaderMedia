using Api.Data.Books;
using AutoMapper;
using Shared.Models.Books;

namespace Api.Utils;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(x => x.Authors.Select(a => a.Author)))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(x => x.Genres.Select(g => g.Genre)))
            .ForMember(dest => dest.Keywords, opt => opt.MapFrom(x => x.Keywords.Select(k => k.Keyword)));
        CreateMap<Author, AuthorDto>();
        CreateMap<Genre, GenreDto>();
        CreateMap<Language, LanguageDto>();
        CreateMap<Series, SeriesDto>();
        CreateMap<Keyword, KeywordDto>();
    }
}