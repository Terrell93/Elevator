using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Models;

namespace elevators.Application.Infrastructure;

public class LoadCurrentStateService : ILoadCurrentStateService
{
	public Task<IEnumerable<Elevator>> LoadState()
	{
		var elevators = new List<Elevator>();
		for (var i = 0; i < 6; i++)
		{
			var elevatorCurrent = new Elevator(i);
			var fileName = $"elevator_{i}_state.txt";
			if (File.Exists(fileName))
			{
				using var reader = new StreamReader(fileName);
				ElevatorRead(elevatorCurrent, reader);
			}
			elevators.Add(elevatorCurrent);
		}

		return Task.FromResult<IEnumerable<Elevator>>(elevators);
	}

	public Task<Elevator> FindElevator(string fileName, int number)
	{
		var elevator = new Elevator(number);
		if (!File.Exists(fileName)) return Task.FromResult(elevator);
		using (var reader = new StreamReader(fileName))
		{
			ElevatorRead(elevator, reader);
		}

		return Task.FromResult(elevator);
	}

	private static void ElevatorRead(Elevator elevator, TextReader reader)
	{
		elevator.Number = int.Parse(reader.ReadLine());
		if (reader.ReadLine().Contains("true"))
		{
			elevator.Open = reader.ReadLine().Contains("true");
		}

		elevator.CurrentFloor = int.Parse(reader.ReadLine());
		if (reader.ReadLine().Contains("true"))
		{
			elevator.Busy = reader.ReadLine().Contains("true");
		}

		elevator.TargetFloor = int.Parse(reader.ReadLine());
	}
}