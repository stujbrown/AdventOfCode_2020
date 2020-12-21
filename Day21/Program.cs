using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        struct Food
        {
            public string[] Ingredients;
            public string[] Allergens;
        };

        static void Main(string[] args)
        {
            var foods = Array.ConvertAll(File.ReadAllLines("input.txt"), food => new Food() { Ingredients = food.Split(" (")[0].Split(' '), Allergens = food.Split(new char[] { '(', ')' })[1].Split(", ").Select(allergen => allergen.Replace("contains", "").Trim()).ToArray() });

            var ingredientAllergens = new Dictionary<string, string>();
            var allergenSet = new HashSet<string>();
            var remainingIngredients = new HashSet<string>();

            Array.ForEach(foods, food => Array.ForEach(food.Allergens, allergen => allergenSet.Add(allergen)));
            Array.ForEach(foods, food => Array.ForEach(food.Ingredients, ingredient => remainingIngredients.Add(ingredient)));

            bool didReduction = true;
            while (didReduction)
            {
                didReduction = false;
                allergenSet.ToList().ForEach(allergen =>
                {
                    var foodsWithAllergen = foods.Where(food => food.Allergens.Contains(allergen));

                    var foodIngredientSets = foodsWithAllergen.ToList().Select(food => food.Ingredients.Where(ingredient => ingredientAllergens.ContainsKey(ingredient) == false));
                    var ingredientHash = new HashSet<string>(foodIngredientSets.First());
                    foodIngredientSets.Skip(1).ToList().ForEach(set => ingredientHash.IntersectWith(set));

                    if (ingredientHash.Count() == 1)
                    {
                        remainingIngredients.Remove(ingredientHash.First());
                        ingredientAllergens[ingredientHash.First()] = allergen;
                        didReduction = true;
                    }
                });
            }

            var ingredientAllergensSorted = ingredientAllergens.ToList();
            ingredientAllergensSorted.Sort((lhsPair, rhsPair) => String.Compare(lhsPair.Value, rhsPair.Value));

            int ingredientCount = 0;
            remainingIngredients.ToList().ForEach(ingredient => Array.ForEach(foods, food => Array.ForEach(food.Ingredients, ingredientInFood => ingredientCount += ingredientInFood.Equals(ingredient) ? 1 : 0)));

            Console.WriteLine("Remaining ingredient occurances: {0}", ingredientCount);
            Console.WriteLine("Sorted allergen ingredients: {0}", ingredientAllergensSorted.Select(pair => pair.Key).Aggregate((lhs,rhs) => lhs + "," + rhs));
        }
    }
}
