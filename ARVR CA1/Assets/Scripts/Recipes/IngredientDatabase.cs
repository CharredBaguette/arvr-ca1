using System.Collections.Generic;
using UnityEngine;

namespace Recipes
{
    /// <summary>
    /// Database of all ingredients in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "Ingredient Database", menuName = "Recipes/Ingredient Database")]
    public class IngredientDatabase : ScriptableObject
    {
        /// <summary>
        /// Singleton instance of IngredientDatabase.
        /// </summary>
        /// <value></value>

        [Tooltip("List of all ingredients in the game")]
        [SerializeField] private Ingredient[] _ingredients;

        private Dictionary<string, Ingredient> _ingredientsDictionary = new Dictionary<string, Ingredient>();
        private Dictionary<Ingredient, string> _ingredientsReverseDictionary = new Dictionary<Ingredient, string>();

        public int Count { get => _ingredientsDictionary.Count; }

        private void OnValidate()
        {
            _ingredientsDictionary = new Dictionary<string, Ingredient>(_ingredients.Length);
            _ingredientsReverseDictionary = new Dictionary<Ingredient, string>(_ingredients.Length);

            foreach (var ingredient in _ingredients)
            {
                // Skip null ingredients
                if (ingredient)
                {
                    Add(ingredient);
                }
            }
        }

        /// <summary>
        /// Get ingredient from name. Returns null if not found.
        /// </summary>
        /// <param name="ingredientName"></param>
        /// <returns></returns>
        public bool Query(string ingredientName, out Ingredient result)
        {
            ingredientName = SanitizeIngredientName(ingredientName);
            return _ingredientsDictionary.TryGetValue(ingredientName, out result);
        }

        /// <summary>
        /// Add ingredient to database. Returns false if ingredient exists.
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        public bool Add(Ingredient ingredient)
        {
            var name = SanitizeIngredientName(ingredient.PluralName);

            // Return false if ingredient already exists
            if (_ingredientsReverseDictionary.TryGetValue(ingredient, out _))
            {
                return false;
            }

            // Add ingredients to dictionaries
            _ingredientsDictionary.Add(name, ingredient);
            _ingredientsReverseDictionary.Add(ingredient, name);
            return true;
        }

        /// <summary>
        /// Sanitize name for input into database
        /// </summary>
        private static string SanitizeIngredientName(string name)
        {
            // We only convert names to upper case for now
            return name.ToUpper();
        }
    }
}
