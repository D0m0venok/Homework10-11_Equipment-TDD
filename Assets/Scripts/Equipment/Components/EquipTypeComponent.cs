using System;

namespace Equipment
{
    [Serializable]
    public sealed class EquipTypeComponent : IComponent
    {
        public EquipmentType Type;
    }
}