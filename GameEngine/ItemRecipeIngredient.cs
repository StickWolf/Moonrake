namespace GameEngine
{
    public class ItemRecipeIngredient
    {
        public string IngredientName { get; private set; }
        public int Amount { get; private set; }

        public ItemRecipeIngredient(string ingredientName, int amount)
        {
            IngredientName = ingredientName;
            Amount = amount;
        }
    }
}
