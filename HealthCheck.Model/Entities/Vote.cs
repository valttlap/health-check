using System;
using System.Collections.Generic;

namespace HealthCheck.Model.Entities;

public partial class Vote
{
    public Guid Id { get; set; }

    public int CategoryId { get; set; }

    public int VoteValue { get; set; }

    public Guid UserId { get; set; }

    public Guid SessionId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;

    public virtual SessionUser User { get; set; } = null!;
}
