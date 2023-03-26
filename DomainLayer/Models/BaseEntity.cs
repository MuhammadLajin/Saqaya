﻿using System;

namespace DomainLayer.Models
{
    public class BaseEntity
    {
        public string Id { get; set; }        
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
