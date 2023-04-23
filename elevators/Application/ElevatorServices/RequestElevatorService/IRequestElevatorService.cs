using elevators.Application.Models;

namespace elevators.Application.ElevatorServices.RequestElevatorService;

public interface IRequestElevatorService
{
	public Task<ElevatorRequestResponse> ReturnElevator(RequestElevatorCommand request);
}