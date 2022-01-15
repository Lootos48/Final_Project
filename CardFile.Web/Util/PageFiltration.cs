using CardFile.BLL.DTO;
using CardFile.Web.Enums;
using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Util
{
    public static class PageFiltration
    {
        public static IEnumerable<CardDTO> Transform(IEnumerable<CardDTO> cards)
        {
            foreach (CardDTO card in cards)
            {
                if (card.Text.Length > 250)
                {
                    card.Text = card.Text.Substring(0, 250) + " ...";
                }
                if (card.Author == null)
                {
                    card.Author = new AuthorDTO
                    {
                        FirstName = "Annonymuos"
                    };
                }
            }
            return cards;
        }

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

        public static IEnumerable<CardDTO> Transform(IEnumerable<CardDTO> cards, SortOptions sortOrder, PageFilter searchFilter)
        {
            if (searchFilter != null)
            {
                switch (searchFilter.SearchBy)
                {
                    case FilterOptions.Title:
                        cards = cards.Where(c => c.Title.Contains(searchFilter.SearchString));
                        break;
                    case FilterOptions.Text:
                        cards = cards.Where(c => c.Text == searchFilter.SearchString);
                        break;
                    default:
                        break;
                }
            }
            return Transform(cards, sortOrder);
        }
    }
}