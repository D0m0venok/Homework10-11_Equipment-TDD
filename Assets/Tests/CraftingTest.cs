using System;
using System.Collections.Generic;
using System.Linq;
using Equipment;
using NUnit.Framework;
using Sample;

[TestFixture]
public sealed class CraftingTest
{
    private ItemConfig _boots;
    private ItemConfig _superBoots;
    private ItemConfig _sword;
    private ItemConfig _shield;
    private ItemConfig _chest;
    
    private Inventory _inventory;
    private Equipment.Equipment _equipment;
    private Character _character;

    [SetUp]
    public void Setup()
    {
        _inventory = new Inventory();
        _equipment = new Equipment.Equipment();
        var stats = Enum.GetNames(typeof(StatType)).ToDictionary(key => key, key => 0).ToArray();
        _character = new Character(stats);

        _equipment.OnItemAdded += item =>
        {
            foreach (var (type, value) in item.GetComponent<StatComponent>().Stats)
            {
                var strType = type.ToString();
                var statValue = _character.GetStat(strType);
                _character.SetStat(strType, statValue + value);
            }
            
            _inventory.RemoveItem(item);
        };
        _equipment.OnItemRemoved += item =>
        {
            foreach (var (type, value) in item.GetComponent<StatComponent>().Stats)
            {
                var strType = type.ToString();
                var statValue = _character.GetStat(strType);
                _character.SetStat(strType, statValue - value);
            }
            
            _inventory.AddItem(item);
        };

        _boots = Substitute.CreateItem("Boots", new EquipTypeComponent(){Type = EquipmentType.LEGS}, 
            new StatComponent(new Dictionary<StatType, int>
            {
                { StatType.Speed , 2}
            }));
        
        _superBoots = Substitute.CreateItem("SuperBoots", new EquipTypeComponent(){Type = EquipmentType.LEGS}, 
            new StatComponent(new Dictionary<StatType, int>
            {
                { StatType.Speed , 5},
                { StatType.Defence , 200},
                { StatType.Health , 1000}
            }));
        
        _sword = Substitute.CreateItem("Sword", new EquipTypeComponent(){Type = EquipmentType.RIGHT_HAND}, 
            new StatComponent(new Dictionary<StatType, int>
            {
                { StatType.Strength , 10}
            }));
        
        _shield = Substitute.CreateItem("Shield", new EquipTypeComponent(){Type = EquipmentType.LEFT_HAND}, 
            new StatComponent(new Dictionary<StatType, int>
            {
                { StatType.Defence , 5}
            }));
        
        _chest = Substitute.CreateItem("Chest", new EquipTypeComponent(){Type = EquipmentType.BODY}, 
            new StatComponent(new Dictionary<StatType, int>
            {
                { StatType.Strength , 15}
            }));
    }

    [Test]
    public void EquipBoots()
    {
        //Arrange: (Установка)
        var boots = _boots.item.Clone();
        _inventory.Setup(boots);

        //Act: (Действие) 
        
        _equipment.SetItem(boots);

        //Assert: (Проверка на результат)
        Assert.Zero(_inventory.GetCount(boots.Name));
        Assert.NotNull(_equipment.GetItem(EquipmentType.LEGS));
        Assert.True(_character.GetStat(StatType.Speed.ToString()) == 2);
    }

    [Test]
    public void SwapBoots()
    {
        var boots = _boots.item.Clone();
        var superBoots = _superBoots.item.Clone();
        //Arrange: (Установка)
        _inventory.Setup(superBoots);
        _equipment.SetItem(boots);

        //Act: (Действие)
        
        _equipment.SetItem(superBoots);
        
        //Assert: (Проверка на результат)
        Assert.Zero(_inventory.GetCount(superBoots.Name));
        Assert.True(_inventory.GetCount(boots.Name) == 1);
        Assert.True(_equipment.GetItem(EquipmentType.LEGS).Name == superBoots.Name);
        Assert.True(_character.GetStat(StatType.Speed.ToString()) == 5);
        Assert.True(_character.GetStat(StatType.Defence.ToString()) == 200);
        Assert.True(_character.GetStat(StatType.Health.ToString()) == 1000);
    }

    [Test]
    public void Equip_Sword_Shield_Chest()
    {
        var sword = _sword.item.Clone();
        var shield = _shield.item.Clone();
        var chest = _chest.item.Clone();
        
        //Arrange: (Установка)
        _inventory.AddItem(sword);
        _inventory.AddItem(shield);
        _inventory.AddItem(chest);

        //Act: (Действие)
        
        _equipment.SetItem(sword);
        _equipment.SetItem(shield);
        _equipment.SetItem(chest);

        //Assert: (Проверка на результат)
        Assert.Zero(_inventory.GetItems().Count);
        Assert.True(_equipment.GetItem(EquipmentType.RIGHT_HAND).Name == sword.Name);
        Assert.True(_equipment.GetItem(EquipmentType.LEFT_HAND).Name == shield.Name);
        Assert.True(_equipment.GetItem(EquipmentType.BODY).Name == chest.Name);
        Assert.True(_character.GetStat(StatType.Strength.ToString()) == 25);
        Assert.True(_character.GetStat(StatType.Defence.ToString()) == 5);
    }
}