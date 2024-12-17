
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Context;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Repositories;

public class CategoryRepository(HealthCheckContext context) : ICategoryRepository
{
    private readonly HealthCheckContext _context = context;

    public async Task<Category?> GetCategory(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
}
