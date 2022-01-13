﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Enums
{
    public enum SortOptions
    {
        None = 0,
        Title = 1,
        Older = 2,
        Newer = 3,
        Popular = 4,
        Unpopular = 5
    }

    public enum FilterOptions
    {
        None = 0,
        Title = 1,
        Text = 2
    }
}