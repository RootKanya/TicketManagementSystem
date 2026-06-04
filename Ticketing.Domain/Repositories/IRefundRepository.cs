using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Aggregates.RefundAggregate;

namespace Ticketing.Domain.Repositories;

public interface IRefundRepository
{
    Task<Refund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Refund refund, CancellationToken cancellationToken = default);
    Task UpdateAsync(Refund refund, CancellationToken cancellationToken = default);
}
