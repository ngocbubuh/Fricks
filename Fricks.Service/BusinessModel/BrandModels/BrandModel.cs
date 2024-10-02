﻿using Fricks.Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fricks.Service.BusinessModel.BrandModels
{
    public class BrandModel : BaseEntity
    {
        public string? Name { get; set; }
    }
}
