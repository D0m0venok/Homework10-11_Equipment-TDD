using System;
using System.Collections.Generic;
using System.Linq;
using Sample;

namespace Equipment
{
    //TODO: Реализовать экипировку
    public sealed class Equipment
    {
        private Dictionary<EquipmentType, Item> _items = new ();
        
        public event Action<Item> OnItemAdded;
        public event Action<Item> OnItemRemoved;

        public void Setup(KeyValuePair<EquipmentType, Item>[] items)
        {
            foreach (var item in items)
            {
                SetItem(item.Value);
            }
        }
        public Item GetItem(EquipmentType type)
        {
            if (!HasItem(type))
                return null;
            
            return _items[type];
        }
        public bool TryGetItem(EquipmentType type, out Item result)
        {
            return _items.TryGetValue(type, out result);
        }
        public void RemoveItem(Item item)
        {
            _items.Remove(item.GetComponent<EquipTypeComponent>().Type);
            OnItemRemoved?.Invoke(item);
        }
        public void SetItem(Item item)
        {
            if(!item.Flags.HasFlag(ItemFlags.EQUPPABLE))
                return;
            
            var type = item.GetComponent<EquipTypeComponent>().Type;
            if(TryGetItem(type, out var result))
                OnItemRemoved?.Invoke(result);

            _items[type] = item;
            OnItemAdded?.Invoke(item);
        }
        public bool HasItem(EquipmentType type)
        {
            return _items.ContainsKey(type);
        }
        public KeyValuePair<EquipmentType, Item>[] GetItems()
        {
            return _items.ToArray();
        }
    }
}