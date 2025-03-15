using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SlotManager
{
    public List<Slot> Slots => _defaultSlots;

    private readonly List<Slot> _defaultSlots;
    private List<Slot> _freeSlots = new List<Slot>();
        
    public SlotManager(Settings settings)
    {
        _defaultSlots = settings.Slots;
    }

    public Slot GetFreeSlot()
    {
        foreach (var slot in Slots)
        {
            if (slot.MergeObject == null)
            {
                return slot;
            }
        }

        return null;
    }

    public Slot GetRandomFreeSlot()
    {
        _freeSlots.Clear();
        foreach (var slot in Slots)
        {
            if (slot.MergeObject == null)
            {
                _freeSlots.Add(slot);
            }
        }

        if (_freeSlots.Count == 0)
        {
            return null;
        }
            
        var randomIndex = Random.Range(0, _freeSlots.Count);
        var randomSlot = _freeSlots[randomIndex > _freeSlots.Count - 1 ? 0 : randomIndex];
        return randomSlot;
    }

    public bool CanFreeSlot()
    {
        foreach (var slot in Slots)
        {
            if (slot.MergeObject == null)
            {
                return true;
            }
        }

        return false;
    }

    [Serializable]
    public class Settings
    {
        public List<Slot> Slots = new List<Slot>();
    }
}