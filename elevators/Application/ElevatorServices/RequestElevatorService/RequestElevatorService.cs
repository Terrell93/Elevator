using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Models;

namespace elevators.Application.ElevatorServices.RequestElevatorService;

public class RequestElevatorService : IRequestElevatorService
{
	private readonly IStateService _stateService;

	public RequestElevatorService(IStateService stateService)
	{
		_stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
	}

	public Task<ElevatorRequestResponse> ReturnElevator(RequestElevatorCommand request)
	{
		// Load initial state
		_stateService.CreateInitialState(request.NumberOfElevators);
		var requestedElevator = RequestElevator(request.Floor);
		var elevator = _stateService.ReturnElevator(requestedElevator.number);
		var elevatorResponse = new ElevatorRequestResponse()
		{
			ElevatorNumber = elevator.Result.Number,
			Message = requestedElevator.message,
			MaxWeight = elevator.Result.MaxWeight
		};

		return Task.FromResult(elevatorResponse);
	}

	private (int number, string message) RequestElevator(int floor)
	{
		var elevators = _stateService.LoadElevators().Result;
		var closestElevator = -1;
		var closestDistance = int.MaxValue;
		var message = "";
		var elevatorList = elevators as Elevator[] ?? elevators.ToArray();
		for (var i = 0; i < elevatorList.Length; i++)
		{
			if (elevatorList[i].Busy) continue;
			var distance = Math.Abs(elevatorList[i].CurrentFloor - floor);
			if (distance >= closestDistance) continue;
			closestElevator = i;
			closestDistance = distance;
		}
		if (closestElevator != -1) {
			elevatorList[closestElevator].TargetFloor = floor;
			elevatorList[closestElevator].Busy = true;
			message = $"Elevator {elevatorList[closestElevator].Number} is going to floor {floor}";
			return (elevatorList[closestElevator].Number, message);
		}
		message = "All elevators are currently busy. Please try again later.";
		return (-1, message);
	}
}