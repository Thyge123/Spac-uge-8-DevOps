using CerealAPI.Model;
using System;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;

namespace CerealAPI.Manager
{
    // Parser that will be used to parse date from CSV file into SQL database
    public class CerealParser
    {
        private readonly string _filePath;
        private readonly CerealManager _cerealManager;
        public CerealParser(string filePath, CerealManager cerealManager)
        {
            _filePath = filePath;
            _cerealManager = cerealManager;
        }

        public async Task ParseAsync()
        {
            int lineNumber = 2; // Starting after the 2 header rows
            int successCount = 0;
            int errorCount = 0;

            using (var reader = new StreamReader(_filePath))
            {
                // Skip header rows
                reader.ReadLine(); // Skip column names
                reader.ReadLine(); // Skip data types

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine($"Line {lineNumber}: Empty line, skipping");
                        continue;
                    }

                    var values = line.Split(';');
                    if (values.Length < 16)
                    {
                        Console.WriteLine($"Line {lineNumber}: Invalid format - expected 16 values but found {values.Length}");
                        errorCount++;
                        continue;
                    }

                    try
                    {
                        // Fix for Rating
                        var ratingString = values[15].Replace(".", "");
                        ratingString = ratingString.Insert(ratingString.Length - 6, ".");

                        // Fix for Weight and Cups to handle decimal points correctly
                        var weightString = values[13].Replace(".", ","); // Replace decimal separator
                        var cupsString = values[14].Replace(".", ",");   // Replace decimal separator

                        var fiberString = values[7].Replace(".", ",");   // Replace decimal separator
                        var carboString = values[8].Replace(".", ",");   // Replace decimal separator

                        // Parse and round to 2 decimal places
                        float weight = float.Parse(weightString);
                        float cups = float.Parse(cupsString);
                        weight = (float)Math.Round(weight, 2);
                        cups = (float)Math.Round(cups, 2);

                        float fiber = float.Parse(fiberString);
                        float carbo = float.Parse(carboString);
                        fiber = (float)Math.Round(fiber, 2);
                        carbo = (float)Math.Round(carbo, 2);

                        var cereal = new Cereal
                        {
                            Name = values[0],
                            Mfr = values[1],
                            Type = values[2],
                            Calories = int.Parse(values[3]),
                            Protein = int.Parse(values[4]),
                            Fat = int.Parse(values[5]),
                            Sodium = int.Parse(values[6]),
                            Fiber = fiber,
                            Carbo = carbo,
                            Sugars = int.Parse(values[9]),
                            Potass = int.Parse(values[10]),
                            Vitamins = int.Parse(values[11]),
                            Shelf = int.Parse(values[12]),
                            Weight = weight,
                            Cups = cups,
                            Rating = float.Parse(ratingString)
                        };

                        Console.WriteLine($"Parsed cereal: {cereal.Name}, Weight: {cereal.Weight:F2}, Cups: {cereal.Cups:F2}, Rating: {cereal.Rating}");
                        await _cerealManager.Add(cereal);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line {lineNumber}: {line}");
                        Console.WriteLine($"Error details: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                        }
                        errorCount++;
                    }
                }
            }

            Console.WriteLine($"Parsing complete. Successfully parsed {successCount} cereals. Encountered {errorCount} errors.");
        }

        // Keep the synchronous version for backward compatibility
        public void Parse()
        {
            ParseAsync().GetAwaiter().GetResult();
        }
    }
}
