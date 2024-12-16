using System;
using System.Collections.Generic;

namespace HealthCheck.Model.Entities;

public partial class SessionUser
{
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
