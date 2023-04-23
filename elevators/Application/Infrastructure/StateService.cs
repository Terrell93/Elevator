using System.Text;
using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Models;

namespace elevators.Application.Infrastructure;

public class StateService : IStateService
{
	private readonly ISaveCurrentStateService _saveState;
	private readonly ILoadCurrentStateService _loadState;

	public StateService(ISaveCurrentStateService saveState, ILoadCurrentStateService loadState)
	{
		_saveState = saveState ?? throw new ArgumentNullException(nameof(saveState));
		_loadState = loadState ?? throw new ArgumentNullException(nameof(loadState));
	}
	
	public void CreateInitialState(int numberOfElevators)
	{
		var elevators = InitCreateElevators(numberOfElevators);
		foreach (var elevator in elevators.Result)
		{
			SaveState(elevator);
		}
	}

	public Task<IEnumerable<Elevator>> LoadElevators()
	{
		return _loadState.LoadState();
	}

	public Task<Elevator> ReturnElevator(int number)
	{
		var fileName = $"elevator_{number}_state.txt";
		var elevator = _loadState.FindElevator(fileName, number);
		return Task.FromResult(elevator.Result);
	}

	public Task<bool> SaveState(Elevator elevator)
	{
		bool successfulSave;
		try
		{
			_saveState.SaveState(elevator);
			successfulSave = true;
		}
		catch (Exception e)
		{
			throw new Exception(e.Message);
		}

		return Task.FromResult(successfulSave);
	}

	private static Task<List<Elevator>> InitCreateElevators(int numberOfElevators)
	{
		var elevators = new List<Elevator>();
		for (var i = 1; i < numberOfElevators + 1; i++) {
			elevators.Add(new Elevator(i));
		}

		return Task.FromResult(elevators);
	}
}