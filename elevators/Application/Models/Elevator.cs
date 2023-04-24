namespace elevators.Application.Models;

public class Elevator
{
	public int Number { get; set; }
	public bool Open { get; set; }
	public int CurrentFloor { get; set; }
	public int TargetFloor { get; set; }
	public int MaxWeight { get; set; }
	public int NumberOfPeople { get; set; }
	public bool Busy { get; set; }
	public string StatusMessage { get; set; }

	public Elevator(int number)
	{
		Number = number + 1;
		CurrentFloor = 1;
		Open = false;
		NumberOfPeople = 0;
		MaxWeight = 600;
		Busy = false;
		StatusMessage = "";
	}
}