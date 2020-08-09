using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recipes.Suggestions
{
public class SuggestionDisplay : MonoBehaviour
    {
        [SerializeField] private WebRequest webRequest;
        [SerializeField] private ListContentManager contentManager;

        public void StartSuggetionForRecipe(Recipe recipe)
        {
            webRequest.StartSuggestionRequest(recipe.Ingredients);
        }

        private void Start()
        {
            webRequest.OnDataLoaded.AddListener(ListSuggestions);
        }

        private void OnDestroy()
        {
            webRequest.OnDataLoaded.RemoveListener(ListSuggestions);
        }

        private void ListSuggestions(IEnumerable<WebRequest.RecipeSuggestion> suggestions)
        {
            List<string> recipes = new List<string>();
            foreach (var suggestion in suggestions)
            {
                recipes.Add(suggestion.Title);
            }

            contentManager.AddElements(recipes);
        }
    }
}