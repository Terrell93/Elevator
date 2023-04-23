using Microsoft.Extensions.DependencyInjection;

namespace elevators.Application.Infrastructure.Interfaces;

public interface IReportServiceLifetime
{
	Guid Id { get; }

	ServiceLifetime Lifetime { get; }
}