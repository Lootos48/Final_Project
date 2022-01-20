using CardFile.BLL.DTO;
using CardFile.Web.Enums;
using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Util
{
    /// <summary>
    /// Статический класс для фильтрации и сортировки страницы
    /// </summary>
    public static class PageFiltration
    {
        /// <summary>
        /// Метод для получения преобразованных сущностей карточек
        /// </summary>
        /// <param name="cards">Коллекция карточек которую нужно преобразовать</param>
        /// <returns>Преобразованная колекция карточек</returns>
        public static IEnumerable<CardDTO> Transform(IEnumerable<CardDTO> cards)
        {
            foreach (CardDTO card in cards)
            {
                if (card.Text.Length > 250)
                {
                    card.Text = card.Text.Substring(0, 250) + "...";
                }
            }
            return cards;
        }

        /// <summary>
        /// Метод для получения преобразованной и отсортированной колекции карточек
        /// </summary>
        /// <param name="cards">Карточки которые нужно преобразовать</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        /// <returns>Преобразованный и отсортированный массив</returns>
        public static IEnumerable<CardDTO> Transform(IEnumerable<CardDTO> cards, SortOptions sortOrder)
        {
            switch (sortOrder)
            {
                case SortOptions.Title:
                    cards = cards.OrderBy(c => c.Title);
                    break;
                case SortOptions.Older:
                    cards = cards.OrderBy(c => c.DateOfCreate);
                    break;
                case SortOptions.Newer:
                    cards = cards.OrderByDescending(c => c.DateOfCreate);
                    break;
                case SortOptions.Popular:
                    cards = cards.OrderByDescending(c => c.LikeAmount);
                    break;
                case SortOptions.Unpopular:
                    cards = cards.OrderBy(c => c.LikeAmount);
                    break;
                default:
                    break;
            }
            return Transform(cards);
        }

        /// <summary>
        /// Метод для преобразования, сортировки и фильтрации колекции карточек
        /// </summary>
        /// <param name="cards">Коллекция карточек с которой нужно произвести действия</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        /// <param name="searchFilter">Параметры фильтрации</param>
        /// <returns>Преобразованная, отсортированная и отфильтрованная коллекция карточек</returns>
        public static IEnumerable<CardDTO> Transform(IEnumerable<CardDTO> cards, SortOptions sortOrder, PageFilter searchFilter)
        {
            if (searchFilter != null && searchFilter.SearchString != null)
            {
                switch (searchFilter.SearchBy)
                {
                    case FilterOptions.Title:
                        cards = cards.Where(c => c.Title.Contains(searchFilter.SearchString));
                        break;
                    case FilterOptions.Text:
                        cards = cards.Where(c => c.Text.Contains(searchFilter.SearchString));
                        break;
                    case FilterOptions.Author:
                        cards = cards.Where(c => 
                        c.Author.Username.Contains(searchFilter.SearchString) ||
                        c.Author.FirstName.Contains(searchFilter.SearchString) ||
                         c.Author.SecondName.Contains(searchFilter.SearchString));
                        break;
                    default:
                        break;
                }
            }
            return Transform(cards, sortOrder);
        }
    }
}