using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
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
    /// Класс для реализации вызова методов CRUD-операций репозитория Card
    /// <inheritdoc cref="ICardsService"/>
    /// </summary>
    public class CardsService : ICardsService
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
        public CardsService(IUnitOfWork uow)
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

        #region [ CRUD-methods ]

        public async Task<CardDTO> CreateCard(CardDTO cardDto)
        {
            if ((await Database.Cards.GetAllAsync()).Any(c => c.Title == cardDto.Title))
            {
                throw new ValidationException("Card with the same title is already exist", "Title");
            }

            Card createdEntity = await Database.Cards.CreateAsync(mapper.Map<Card>(cardDto));

            return mapper.Map<CardDTO>(createdEntity);
        }

        public async Task<CardDTO> GetCard(int? id)
        {
            Card card = await Database.Cards.FindByIdAsync(id.Value);

            if (card == null)
            {
                throw new ObjectNotFoundException(typeof(Card), id.Value.ToString(), "Object wan`t found by ID");
            }

            return mapper.Map<CardDTO>(card);
        }

        public async Task<IEnumerable<CardDTO>> GetAll()
        {
            return mapper.Map<IEnumerable<CardDTO>>(await Database.Cards.GetAllAsync());
        }

        public async Task<bool> UpdateCard(CardDTO cardDTO)
        {
            if ((await Database.Cards.GetAllAsync()).Any(c => c.Id != cardDTO.Id && c.Title == cardDTO.Title))
            {
                throw new ValidationException("Card with the same title is already exist", "Title");
            }

            Card card = mapper.Map<Card>(cardDTO);
            return await Database.Cards.UpdateAsync(card);
        }

        public async Task<bool> DeleteCard(int id)
        {
            return await Database.Cards.RemoveAsync(id);
        }

        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
