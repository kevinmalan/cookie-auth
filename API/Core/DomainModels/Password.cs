﻿namespace Core.DomainModels
{
    public class Password
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}