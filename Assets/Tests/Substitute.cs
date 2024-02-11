using Sample;
using UnityEngine;

public static class Substitute
{
    public static ItemConfig CreateItem(string itemName, params object[] components)
    {
        var item = ScriptableObject.CreateInstance<ItemConfig>();
        item.item = new Item(itemName, ItemFlags.EQUPPABLE, components);
        return item;
    }
}