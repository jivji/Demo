﻿namespace DataAccess.Objects
{
    public class Parent
    {
        public int Id { get; set; }
        public int GrandParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}