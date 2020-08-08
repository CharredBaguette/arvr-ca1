using UnityEngine;

namespace Recipes
{
    /// <summary>
    /// An ingredient used in recipes.
    /// </summary>
    [CreateAssetMenu(fileName = "NewIngredient", menuName = "Recipes/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        [SerializeField] private string _pluralName;
        /// <summary>
        /// Name in plural form
        /// </summary>
        public string PluralName => _pluralName;

        [SerializeField] private GameObject _model;
        /// <summary>
        /// Model of this ingredient
        /// </summary>
        public GameObject Model => _model;
    }
}
