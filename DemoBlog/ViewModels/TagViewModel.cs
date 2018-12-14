﻿using System;
using System.Collections.Generic;
using DemoBlog.ViewModels.ArticleViewModels;

namespace DemoBlog.ViewModels
{
    public class TagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<ArticleListVewModel> Articles { get; set; }
    }
}