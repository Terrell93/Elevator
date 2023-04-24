// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using elevators.Application.ElevatorServices.MoveElevatorService;
using elevators.Application.ElevatorServices.RequestElevatorService;
using elevators.Application.Infrastructure;
using elevators.Application.Infrastructure.Interfaces;
using elevators.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(services =>
	{
		services.AddTransient<IStateService, StateService>();
		services.AddTransient<IRequestElevatorService, RequestElevatorService>();
		services.AddTransient<IMoveElevatorService, MoveElevatorService>();
		services.AddTransient<ISaveCurrentStateService, SaveCurrentStateService>();
		services.AddTransient<ILoadCurrentStateService, LoadCurrentStateService>();
	})
	.Build();

/*Project Setup*/
var saveCurrentStateService = new SaveCurrentStateService();
var loadCurrentStateService = new LoadCurrentStateService();
var stateService = new StateService(saveCurrentStateService, loadCurrentStateService);
var requestElevatorService = new RequestElevatorService(stateService);
var moveElevatorService = new MoveElevatorService(stateService);
var elevatorService = new ElevatorService(requestElevatorService, moveElevatorService);


/*Run Program*/
RunElevators(elevatorService);

static void RunElevators(ElevatorService elevatorService)
{
	var elevators = GetNumberOfElevators();
	var floor = PickFloor();
	var requestElevator = elevatorService.RequestElevator(floor, elevators);
	Console.WriteLine(requestElevator.Result.Message);

	var (numberOfPeople,totalWeight) = LoadPeople(requestElevator.Result.MaxWeight);

	var rideElevator = elevatorService.RideElevator(requestElevator.Result.ElevatorNumber, floor, totalWeight, numberOfPeople);
	Console.WriteLine(rideElevator.Result.Message);
}

static int GetNumberOfElevators()
{
	int selectedNumber;
	while (true)
	{
		Console.WriteLine("Number of elevators to create (Max 20): ");
		var numberOfElevators = Console.ReadLine();

		if (ValidateNumberOfElevators(numberOfElevators))
		{
			Console.WriteLine("Valid input: " + numberOfElevators);
			selectedNumber = int.Parse(numberOfElevators);
			break;
		}

		Console.WriteLine("Invalid input. Please try again.");
	}
	return selectedNumber;
}

static bool ValidateNumberOfElevators(string numberOfElevators)
{
	var elevatorLimitRegex = new Regex("^[1-9]$|^1[0-9]$|^10$");
	return elevatorLimitRegex.IsMatch(numberOfElevators);
}

static int PickFloor()
{
	int selectedFloor;
	while (true)
	{
		Console.WriteLine("Pick floor number (Max 20): ");
		var floor = Console.ReadLine();

		if (ValidateSelectedFloor(floor))
		{
			Console.WriteLine("Valid input: " + floor);
			selectedFloor = int.Parse(floor);
			break;
		}

		Console.WriteLine("Invalid input. Please try again.");
	}
	return selectedFloor;
}

static bool ValidateSelectedFloor(string selectedFloor)
{
	var floorLimitRegex = new Regex("^[1-9]$|^1[0-9]$|^10$");
	return floorLimitRegex.IsMatch(selectedFloor);
}

static (int numberOfPeople, int totalWeight) LoadPeople(int maxWeight)
{
	var totalWeight = 0;
	int people;
	var numberOfPeople = GetNumberOfPeople();

	for (people = 1; people <= numberOfPeople; people++)
	{
		while (people <= numberOfPeople)
		{
			Console.Write($"Enter the weight of person {people} (or 0 to exit): ");
			var weight = Console.ReadLine();
			var weightValid = int.TryParse(weight,out var peopleWeight);
			if (!weightValid) continue;
			if (peopleWeight == 0)
			{
				Console.WriteLine("Exiting...");
				break;
			}

			if (totalWeight + peopleWeight > maxWeight)
			{
				Console.WriteLine("Maximum limit exceeded. Exiting...");
				break;
			}

			people++;
			totalWeight += peopleWeight;
		}

		Console.WriteLine("Invalid input. Please try again.");
	}

	Console.WriteLine($"Total number of people: {people}");
	Console.WriteLine($"Total weight: {totalWeight}");
	return (people, totalWeight);
}

static int GetNumberOfPeople()
{
	int people;
	var peopleLimitRegex = new Regex("^[1-8]$");
	
	while (true)
	{
		Console.Write("Enter number of people entering elevator (Max 8): ");
		var numberOfPeople = Console.ReadLine();

		if (peopleLimitRegex.IsMatch(numberOfPeople))
		{
			Console.WriteLine("Valid input: " + numberOfPeople);
			if (int.TryParse(numberOfPeople, out people))
			{
				people = int.Parse(numberOfPeople);
			}
			
			break;
		}

		Console.WriteLine("Invalid input. Please try again.");
	}
	return people;
}

Console.ReadKey();