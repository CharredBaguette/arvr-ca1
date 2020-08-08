using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Recipes
{
    /// <summary>
    /// A step in a recipe.
    /// </summary>
    [Serializable]
    public class RecipeStep
    {
        // TODO: Remove SerializeField attributes from debugging session.
        [SerializeField] private Ingredient[] _ingredients;

        /// <summary>
        /// Ingredients mentioned in the step.
        /// </summary>
        public IReadOnlyList<Ingredient> Ingredients => _ingredients;

        [SerializeField] private string _procedure;

        /// <summary>
        /// Procedure for the step as text string.
        /// </summary>
        public string Procedure => _procedure;

        // Constructor
        public RecipeStep(string procedure)
        {
            _procedure = procedure;

            // initialize ingredients present in the procedure
            _ingredients = ExtractIngredientsFromText(procedure);
        }

        /// <summary>
        /// Extract ingredient names from a given procedure text by querying with IngredientDatabase
        /// and create an ingredient list.
        /// Note: This function is costly with O(2n^2) complexity (I think LOL)
        /// </summary>
        private static Ingredient[] ExtractIngredientsFromText(string procedure)
        {
            // Use regex to extract words in procedure by splitting 
            // string using a whitespace as delimeter and subsequently
            // trimming it.
            var words = procedure.Split(new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim())
                .ToArray();

            // Cache ingredient database instance
            var ingredientDatabase = IngredientDatabase.Instance;

            var foundIngredients = new HashSet<Ingredient>();

            // Iterate through words and select all words that exists in database
            for (int i = 0; i < words.Length; ++i)
            {
                // Query database and check match with word. Returns null if not founn
                // Add found ingredient to list if not null
                if (ingredientDatabase.Query(words[i], out Ingredient foundIngredient))
                {
                    foundIngredients.Add(foundIngredient);
                }
            }

            return foundIngredients.ToArray();
        }
    }
}