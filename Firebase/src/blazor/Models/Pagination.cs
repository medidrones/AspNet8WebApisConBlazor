﻿namespace blazor.Models;

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalNumberOfPages { get; set; }
    public int TotalNumberOfRecords { get; set; }
    public List<Producto> Results { get; set; } = new List<Producto>();
}
