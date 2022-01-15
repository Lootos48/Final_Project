using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    /// <summary>
    /// Класс для реализации вызова методов CRUD-операций репозитория Author
    /// </summary>
    public class AuthorService : IAuthorsService
    {
        /// <summary>
        /// Объект класса UnitOfWork через который происходит работа с репозиториями
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Поле класса авто-маппера, в который переданы конфигурация карт проекций используемый, в этом классе - классов сущностей
        /// </summary>
        readonly IMapper mapper;

        /// <summary>
        /// Конструктор в котором инициализируется поле взаимодействия с БД, а также задается конфигурация проекций авто-маппера
        /// </summary>
        /// <param name="uow">Класс который хранит в себе репозитории для взаимодействия с контекстом БД</param>
        public AuthorService(IUnitOfWork uow)
        {
            Database = uow;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Card, CardDTO>()
                    .ForMember(s => s.Author, o => o.UseDestinationValue());

                cfg.CreateMap<CardDTO, Card>()
                    .ForMember(s => s.Author, o => o.UseDestinationValue());

                cfg.CreateMap<Author, AuthorDTO>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());

                cfg.CreateMap<AuthorDTO, Author>()
                    .ForMember(s => s.Cards, o => o.UseDestinationValue());
            });

            mapper = config.CreateMapper();
        }

        #region [ CRUD-operations ]

        public async Task<AuthorDTO> CreateAuthor(AuthorDTO authorDto)
        {
            Author createdEntity = await Database.Authors.CreateAsync(mapper.Map<Author>(authorDto));

            return mapper.Map<AuthorDTO>(createdEntity);
        }

        public async Task<AuthorDTO> GetAuthor(int? id)
        {
            var author = await Database.Authors.FindByIdAsync(id.Value);

            return mapper.Map<AuthorDTO>(author);
        }

        public async Task<AuthorDTO> GetAuthor(Func<AuthorDTO, bool> predicate)
        {
            var authors = await Database.Authors.GetAllAsync();
            var authorsDTO = mapper.Map<IEnumerable<AuthorDTO>>(authors);
            return authorsDTO.FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<AuthorDTO>> GetAll()
        {
            return mapper.Map<IEnumerable<AuthorDTO>>(await Database.Authors.GetAllAsync());
        }

        public async Task<bool> UpdateAuthor(AuthorDTO authorDTO)
        {
            Author author = mapper.Map<Author>(authorDTO);
            return await Database.Authors.UpdateAsync(author);
        }

        public async Task<bool> DeleteAuthor(int id)
        { 
            return await Database.Authors.RemoveAsync(id);
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
