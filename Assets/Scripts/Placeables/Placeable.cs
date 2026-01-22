using UnityEngine;

public abstract class Placeable
{
    public Placeable(Player owner)
    {
        this.owner = owner;
    }

    protected Player owner;
}
