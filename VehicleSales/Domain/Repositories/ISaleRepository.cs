using Domain.Models;

namespace Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale> Create(Sale sale);
}