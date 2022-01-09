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
    public class CardsService : ICardsService
    {
        IUnitOfWork Database { get; set; }
        readonly IMapper mapper;

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

        public async Task<CardDTO> CreateCard(CardDTO cardDto)
        {
            Card createdEntity = await Database.Cards.CreateAsync(mapper.Map<Card>(cardDto));

            return mapper.Map<CardDTO>(createdEntity);
        }

        public async Task<CardDTO> GetCard(int? id)
        {
            var card = await Database.Cards.FindByIdAsync(id.Value);

            return mapper.Map<CardDTO>(card);
        }

        public async Task<IEnumerable<CardDTO>> GetAll()
        {
            return mapper.Map<IEnumerable<CardDTO>>(await Database.Cards.GetAllAsync());
        }

        public async Task<bool> UpdateCard(CardDTO cardDTO)
        {
            Card card = mapper.Map<Card>(cardDTO);
            return await Database.Cards.UpdateAsync(card);
        }

        public async Task<bool> DeleteCard(int id)
        {
            /*Card card = mapper.Map<Card>(cardDTO);*/
            return await Database.Cards.RemoveAsync(id);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
