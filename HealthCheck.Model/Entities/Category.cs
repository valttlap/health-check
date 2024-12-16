using System;
using System.Collections.Generic;

namespace HealthCheck.Model.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string TitleFi { get; set; } = null!;

    public string TitleEn { get; set; } = null!;

    public string ExampleGoodFi { get; set; } = null!;

    public string ExampleGoodEn { get; set; } = null!;

    public string ExampleBadFi { get; set; } = null!;

    public string ExampleBadEn { get; set; } = null!;

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
