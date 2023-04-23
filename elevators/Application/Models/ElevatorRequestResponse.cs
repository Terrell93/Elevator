namespace elevators.Application.Models;

public class ElevatorRequestResponse
{
	public int ElevatorNumber { get; set; }
	public int MaxWeight { get; set; }
	public string Message { get; set; }
}