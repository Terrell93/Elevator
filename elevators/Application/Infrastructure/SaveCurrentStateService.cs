using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Models;

namespace elevators.Application.Infrastructure;

public class SaveCurrentStateService : ISaveCurrentStateService
{
	public async void SaveState(Elevator elevator)
	{
		await using var writer = new StreamWriter($"elevator_{elevator.Number}_state.txt");
		await writer.WriteLineAsync($"{elevator.Number}");
		await writer.WriteLineAsync($"{elevator.Open}");
		await writer.WriteLineAsync($"{elevator.CurrentFloor}");
		await writer.WriteLineAsync($"{elevator.Busy}");
		await writer.WriteLineAsync($"{elevator.TargetFloor}");
		await writer.WriteLineAsync($"{elevator.StatusMessage}");
	}
}