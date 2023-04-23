using elevators.Application.Models;

namespace elevators.Application.ElevatorServices.MoveElevatorService;

public interface IMoveElevatorService
{
	public Task<ElevatorRequestResponse> OperateElevator(MoveElevatorCommand command);
}