using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Models;

namespace elevators.Application.ElevatorServices.MoveElevatorService;

public class MoveElevatorService : IMoveElevatorService
{
	private readonly IStateService _stateService;

	public MoveElevatorService(IStateService stateService)
	{
		_stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
	}

	public Task<ElevatorRequestResponse> OperateElevator(MoveElevatorCommand request)
	{
		var elevator = _stateService.ReturnElevator(request.ElevatorNumber);
		var elevatorStatus = MoveElevator(request.TargetFloor,request.NumberOfPeople,request.TotalWeight, elevator.Result);
		
		var elevatorResponse = new ElevatorRequestResponse()
		{
			ElevatorNumber = elevator.Result.Number,
			Message = elevatorStatus
		};
		return Task.FromResult(elevatorResponse);
	}

	private string MoveElevator(int targetFloor,int numberOfPeople, int totalWeight, Elevator elevator)
	{
		var message = "";
		elevator.NumberOfPeople = numberOfPeople;
		elevator.Busy = true;
		elevator.CurrentFloor = targetFloor;
		elevator.StatusMessage = $"Elevator took {numberOfPeople} people to floor number: {targetFloor}";
		var save = _stateService.SaveState(elevator);

		if (save.Result)
		{
			message =
				$"Elevator number {elevator.Number} moved {numberOfPeople} people weighing {totalWeight}kg to floor {targetFloor} successfully";
		}

		return message;
	}
}