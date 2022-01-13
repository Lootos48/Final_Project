using CardFile.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации вызова CRUD-операций репозитория
    /// </summary>
    public interface ICardsService : IDisposable
    {
        Task<CardDTO> CreateCard(CardDTO cardDto);
        Task<CardDTO> GetCard(int? id);
        Task<IEnumerable<CardDTO>> GetAll();
        Task<bool> UpdateCard(CardDTO cardDTO);
        Task<bool> DeleteCard(int id);
    }
}
