using System.ComponentModel.DataAnnotations;

namespace CerealAPI.Model
{
    public class Cereal
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mfr { get; set; }
        public string Type { get; set; }

        public int Calories { get; set; }

        public int Protein { get; set; }

        public int Fat { get; set; }

        public int Sodium { get; set; }

        public float Fiber { get; set; }

        public float Carbo { get; set; }

        public int Sugars { get; set; }

        public int Potass { get; set; }

        public int Vitamins { get; set; }

        public int Shelf { get; set; }

        public float Weight { get; set; }

        public float Cups { get; set; }

        public float Rating { get; set; }

        public Cereal()
        {
            
        }

        public Cereal(string name, string mfr, string type, int calories, int protein, int fat, int sodium, float fiber, float carbo, int sugars, int potass, int vitamins, int shelf, float weight, float cups, float rating)
        {
            Name = name;
            Mfr = mfr;
            Type = type;
            Calories = calories;
            Protein = protein;
            Fat = fat;
            Sodium = sodium;
            Fiber = fiber;
            Carbo = carbo;
            Sugars = sugars;
            Potass = potass;
            Vitamins = vitamins;
            Shelf = shelf;
            Weight = weight;
            Cups = cups;
            Rating = rating;
        }
    }
}
