using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableTypes
{
    Dumbell,
    Food,
    Drink,
}
public class CollectableObjects : MonoBehaviour
{
    public CollectableTypes collectableTypes;

    public string GetCollectableType()
    {
        return collectableTypes.ToString();
    }

    public int GetCollectableEnergy()
    {
        if (collectableTypes == CollectableTypes.Drink)
        {
            return 10;
        }
        else if (collectableTypes == CollectableTypes.Dumbell)
        {
            return 20;
        }
        else if (collectableTypes == CollectableTypes.Food)
        {
            return 30;
        }
        else { return 0; }
    }
}
