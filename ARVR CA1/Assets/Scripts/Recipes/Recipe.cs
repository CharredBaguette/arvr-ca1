using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Recipes
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipes/Recipe")]
    public class Recipe : ScriptableObject
    {
        [SerializeField] private string _recipeName;
        // List of ingredients needed in the recipe
        [SerializeField] private List<string> _ingredients;

        // Text instructions for recipe
        [Multiline(20)]
        [SerializeField] private string _instructions;

        [Header("Anything below here is serialized only for ease of debugging. These fields are generated whenever _instructions are changed.")]
        [Header("[Debug]")]
        // TODO: Remove SerializeField attribute from _steps?
        [SerializeField] private List<RecipeStep> _steps;

        private void OnValidate()
        {
            _steps = ToRecipeSteps(_instructions);
        }
        
        public List<RecipeStep> ToRecipeSteps(string instructions)
        //public List<RecipeStep> ToRecipeStep(string instructions)
        {
            // Split instructions into paragraphs using regex
            // Note: we use line breaks as delimeters which requires
            // platform to be considered, therefore we have both \r\n and \n
            // styled line breaks.
            var paragraphs = Regex.Split(instructions, @"(\r\n?|\n){2}")
                      .Where(p => p.Any(char.IsLetterOrDigit));

            List<RecipeStep> steps = new List<RecipeStep>();

            // Consider each paragraph as a procedure and create a
            // new step from each procedure
            foreach (var paragraph in paragraphs)
            {
                steps.Add(new RecipeStep(paragraph));
            }

            return steps;
        }
    }
}