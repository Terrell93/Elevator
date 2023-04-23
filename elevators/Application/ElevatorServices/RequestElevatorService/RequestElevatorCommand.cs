namespace elevators.Application.ElevatorServices.RequestElevatorService;

public class RequestElevatorCommand
{
	public int Floor { get; set; }
	public int NumberOfElevators { get; set; }
	public int MaxWeight { get; set; }
}