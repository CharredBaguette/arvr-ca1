using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Recipes.Display
{
    public class ProcedureDisplay : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _procedureText;
        [SerializeField] private TMP_Text _recipeNameText;
        [SerializeField] private ModelArranger _modelArranger;
        [SerializeField] private IngredientDatabase _ingredientDatabase;
        [SerializeField] private IReadOnlyList<RecipeStep> _steps;
        [SerializeField] private Recipe _recipe;

        private Dictionary<Ingredient, GameObject> _modelPool = new Dictionary<Ingredient, GameObject>();
        private int _procedureIndex = 0;

        private void Start()
        {
            SetRecipe(_recipe);
        }

        /// <summary>
        /// Update currently displayed recipe
        /// </summary>
        public void SetRecipe(Recipe recipe)
        {
            _recipe = recipe;

            // Clear recipe if recipe is null
            if (!_recipe)
            {
                ClearRecipe();
                return;
            }

            _recipeNameText.text = _recipe.RecipeName;

            // Cache steps in recipe
            _steps = _recipe.Steps;

            // Set current procedure to first step
            SetProcedure(0);

            // Populate model pool with all ingredients in the recipe
            InitializeModelPool(_recipe.Steps, _ingredientDatabase);

            // Update display to be shown
            RefreshModelArranger();
        }

        /// <summary>
        /// Display next procedure
        /// </summary>
        public void NextProcedure()
        {
            if (!_recipe)
            {
                return;
            }
            
            _procedureIndex = Mathf.Clamp(_procedureIndex + 1, 0, _steps.Count - 1);
            SetProcedure(_procedureIndex);
        }

        /// <summary>
        /// Display previous procedure
        /// </summary>
        public void PreviousProcedure()
        {
            if (!_recipe)
            {
                return;
            }
            
            _procedureIndex = Mathf.Clamp(_procedureIndex - 1, 0, _steps.Count - 1);
            SetProcedure(_procedureIndex);
        }

        // Clear all models and redisplay all active models
        private void RefreshModelArranger()
        {
            // Remove all children
            _modelArranger.RemoveAllChildren();

            foreach (var model in _modelPool.Values)
            {
                // Add active model to children
                if (model.activeSelf)
                {
                    _modelArranger.AddToChildren(model.transform);
                }
            }

            _modelArranger.Arrange();
        }

        // Set currently shown procedure
        private void SetProcedure(int procedureIndex)
        {
            var step = _steps[procedureIndex];
            _procedureText.text = step.Procedure;

            HideIngredients();
            ToggleIngredients(step.Ingredients, true);
            RefreshModelArranger();
        }

        // Hide all ingredients from being displayed
        private void HideIngredients()
        {
            foreach (var model in _modelPool.Values)
            {
                model.SetActive(false);
            }
        }

        // Toggle the ingredient to be displayed or hidden
        private void ToggleIngredients(in IReadOnlyCollection<Ingredient> ingredients, bool toggle)
        {
            foreach (var ingredient in ingredients)
            {
                if (_modelPool.TryGetValue(ingredient, out GameObject model))
                {
                    model.SetActive(toggle);
                }
            }
        }

        // Create instances of the model in the pool
        private void InitializeModelPool(in IEnumerable<RecipeStep> steps, IngredientDatabase _ingredientDatabase)
        {
            // Set to store models
            HashSet<string> ingredients = new HashSet<string>();

            // Iterate and find every ingredient in the list of steps
            // then add it to the set of models
            foreach (var step in steps)
            {
                foreach (var ingredient in step.Ingredients)
                {
                    ingredients.Add(ingredient.PluralName);
                }
            }

            // Instantiate and add each model found to the model pool
            foreach (var name in ingredients)
            {
                // Query ingredient from database
                if (_ingredientDatabase.Query(name, out var ingredient))
                {
                    // Create new instance of the ingredient model and set inactive
                    var model = Instantiate(ingredient.Model, transform);
                    model.SetActive(false);

                    _modelPool.Add(ingredient, model);
                }
            }
        }

        public void ClearRecipe()
        {
            _recipe = null;
            _procedureIndex = 0;

            // Destory all models from model pool
            foreach (var model in _modelPool.Values)
            {
                Destroy(model);
            }

            _modelPool.Clear();
        }
    }
}