using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recipes.Display
{
    /// <summary>
    /// Class containing functions to configure a group of transforms into different
    /// arrangements
    /// </summary>
    public class ModelArranger : MonoBehaviour
    {
        // TODO: Remove SerializeField attribute
        [SerializeField] private List<Transform> _models = new List<Transform>();

        [SerializeField] private float _spacing  = 0.5f;
        /// <summary>
        /// The spacing between models in the arrangement
        /// </summary>
        public float Spacing { get => _spacing; set => _spacing = value; }

        [SerializeField] private ArrangementAxis _axis = ArrangementAxis.X;
        /// <summary>
        /// The axis to arrange models on
        /// </summary>
        public ArrangementAxis Axis { get => _axis; set => _axis = value; }

        /// <summary>
        /// Add model to list of models to arrange
        /// </summary>
        /// <param name="model"></param>
        public void AddToChildren(Transform model)
        {
            model.parent = transform;
            _models.Add(model);
        }

        public void RemoveAllChildren()
        {
            _models.Clear();
        }

        public void Arrange()
        {
            Arrange(_models, _spacing, _axis);
        }

        /// <summary>
        /// Arrange models in an a row
        /// </summary>
        //
        // Set initial model at origin and
        // spread out subsequent models from origin
        public void Arrange(in List<Transform> models, float spacing, ArrangementAxis axis)
        {
            Vector3 position = Vector3.zero;

            // Strategy to increment position of subsequent model targets
            Action incrementPosition = null;

            // Set increment direction function to increment in the direction of the arrangement axis argument
            switch (axis)
            {
                case ArrangementAxis.X:
                    {
                        incrementPosition = () =>
                        {
                            position.x += spacing;
                        };
                    }
                    break;
                case ArrangementAxis.Y :
                    {
                        incrementPosition = () =>
                        {
                            position.y += spacing;
                        };
                    }
                    break;
                case ArrangementAxis.Z :
                    {
                        incrementPosition = () =>
                        {
                            position.z += spacing;
                        };
                    }
                    break;
            }

            for (int i = 0; i < models.Count; ++i)
            {
                models[i].localPosition = position;

                // Increment position on every even model
                if (i % 2 == 0)
                {
                    incrementPosition();
                }

                position = -position;
            }
        }

        /// <summary>
        /// The axis to spread arrangement out on
        /// </summary>
        public enum ArrangementAxis
        {
            X,
            Y,
            Z,
        }
    }
}
