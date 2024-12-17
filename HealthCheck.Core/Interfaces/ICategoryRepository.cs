
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetCategory(int id);
}
