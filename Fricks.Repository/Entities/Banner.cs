﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Repository.Entities
{
    public partial class Banner : BaseEntity
    {
        public string? Name { get; set; }

        public string? Image {  get; set; }

        public int Index { get; set; }
    }
}
