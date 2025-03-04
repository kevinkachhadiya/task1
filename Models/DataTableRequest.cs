﻿public class DataTableRequest
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public SearchParam Search { get; set; }
    public OrderParam[] Order { get; set; }
}

public class SearchParam
{
    public string Value { get; set; }
    public bool Regex { get; set; }
}

public class OrderParam
{
    public int Column { get; set; }
    public string Dir { get; set; }
}