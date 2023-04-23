using elevators.Application.ElevatorServices.MoveElevatorService;
using elevators.Application.ElevatorServices.RequestElevatorService;
using elevators.Application.Models;

namespace elevators.Application.Services;

public class ElevatorService
{
	private readonly IRequestElevatorService _requestElevatorService;
	private readonly IMoveElevatorService _moveElevatorService;
	public ElevatorService(IRequestElevatorService requestElevatorService, IMoveElevatorService moveElevatorService)
	{
		_requestElevatorService = requestElevatorService ?? throw new ArgumentNullException(nameof(requestElevatorService));
		_moveElevatorService = moveElevatorService ?? throw new ArgumentNullException(nameof(moveElevatorService));
	}

	public Task<ElevatorRequestResponse> RequestElevator(int floor, int numberOfElevators)
	{
		var command = new RequestElevatorCommand()
		{
			Floor = floor,
			NumberOfElevators = numberOfElevators,
		};
		return _requestElevatorService.ReturnElevator(command);
	}

	public Task<ElevatorRequestResponse> RideElevator(int elevatorNumber,int targetFloor, int totalWeight, int numberOfPeople)
	{
		var command = new MoveElevatorCommand()
		{
			ElevatorNumber = elevatorNumber,
			TargetFloor = targetFloor,
			TotalWeight = totalWeight,
			NumberOfPeople = numberOfPeople
		};

		return _moveElevatorService.OperateElevator(command);
	}
}