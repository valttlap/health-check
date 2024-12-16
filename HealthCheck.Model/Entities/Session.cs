using System;
using System.Collections.Generic;

namespace HealthCheck.Model.Entities;

public partial class Session
{
    public Guid Id { get; set; }

    public int CurrentCategoryId { get; set; }

    public virtual Category CurrentCategory { get; set; } = null!;

    public virtual ICollection<SessionUser> SessionUsers { get; set; } = new List<SessionUser>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
