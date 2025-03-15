using SimpleEventBus.Events;

public class EventSpawn : EventBase
{
    public Slot Slot { get; }

    public EventSpawn(Slot slot)
    {
        Slot = slot;
    }
}
