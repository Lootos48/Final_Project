﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Enums
{
    /// <summary>
    /// Перечисление разных порядков сортировки
    /// </summary>
    public enum SortOptions
    {
        Title = 1,
        Older = 2,
        Newer = 3,
        Popular = 4,
        Unpopular = 5
    }

    /// <summary>
    /// Перечисление разных способов фильтрации
    /// </summary>
    public enum FilterOptions
    {
        Title = 1,
        Text = 2,
        Author = 3
    }
}