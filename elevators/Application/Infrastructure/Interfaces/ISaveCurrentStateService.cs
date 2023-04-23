using elevators.Application.Models;

namespace elevators.Application.Infrastructure.Interfaces;

public interface ISaveCurrentStateService
{
	void SaveState(Elevator elevator);
}