﻿using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Document { get; set; }
    }
}
