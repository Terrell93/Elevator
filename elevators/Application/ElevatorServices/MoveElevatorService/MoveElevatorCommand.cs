namespace elevators.Application.ElevatorServices.MoveElevatorService;

public class MoveElevatorCommand
{
	public int TargetFloor { get; set; }
	public int ElevatorNumber { get; set; }
	public int NumberOfPeople { get; set; }
	public int TotalWeight { get; set; }
}