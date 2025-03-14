using CerealAPI.Model;
using CerealAPIDriver;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


Console.WriteLine("Waiting for API to start...");
Thread.Sleep(3000);

Console.WriteLine("CerealAPI Demo Starting...");
var driver = new ApiDriver("https://localhost:7226");

try
{
    // Anonymous user operations
    Console.WriteLine("\n--- Anonymous User Operations ---");
    await driver.GetAllCerealsAsync();
    await driver.GetCerealByIdAsync(1);
    await driver.GetCerealByNameAsync("Cheerios");
    await driver.GetFilteredCerealsAsync(minCalories: 100, manufacturer: "K");
    await driver.GetSortedCerealsAsync(sortBy: "Calories", sortOrder: "asc");
    await driver.GetCerealImagesAsync();

    // Admin operations (requires login)
    Console.WriteLine("\n--- Admin Operations (after login) ---");
    await driver.LoginAsync("admin", "admin");
    
    // Add a new cereal
    var newCereal = new Cereal
    {
        Name = "Test Cereal",
        Mfr = "K",
        Type = "C",
        Calories = 110,
        Protein = 2,
        Fat = 0,
        Sodium = 200,
        Fiber = 1.0f,
        Carbo = 22.0f,
        Sugars = 3,
        Potass = 95,
        Vitamins = 25,
        Shelf = 3,
        Weight = 1.0f,
        Cups = 0.75f,
        Rating = 42.5f
    };
    var addedCereal = await driver.AddCerealAsync(newCereal);

    // Update a cereal
    var cerealToUpdate = new Cereal
    {
        Id = addedCereal.Id,
        Name = "Updated Cereal",
        Mfr = "G",
        Type = "C",
        Calories = 120,
        Protein = 3,
        Fat = 1,
        Sodium = 210,
        Fiber = 2.0f,
        Carbo = 21.0f,
        Sugars = 5,
        Potass = 100,
        Vitamins = 25,
        Shelf = 3,
        Weight = 1.0f,
        Cups = 0.75f,
        Rating = 44.5f
    };
    await driver.UpdateCerealAsync(cerealToUpdate);

    await driver.DeleteCerealAsync(addedCereal.Id);

    await driver.GetCerealByIdAsync(addedCereal.Id);

}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

Console.WriteLine("\nCerealAPI Demo Completed");
Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();