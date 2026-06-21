using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGuildDemo.Data
{
    [Serializable]
    public sealed class RecipeIngredient
    {
        public string itemId = string.Empty;
        public int quantity = 1;
    }

    [CreateAssetMenu(menuName = "Idle Guild/Data/Recipe Definition", fileName = "RecipeDefinition")]
    public sealed class RecipeDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
        [SerializeField] private string outputItemId;
        [SerializeField] private int outputQuantity = 1;

        public string Id => id;
        public string DisplayName => displayName;
        public string Description => description;
        public IReadOnlyList<RecipeIngredient> Ingredients => ingredients;
        public string OutputItemId => outputItemId;
        public int OutputQuantity => outputQuantity;
    }
}
