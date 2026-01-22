using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    public bool IsPlaceableAt { get; private set; }
    public PlaceableType TypeAvailable { get; private set; }
    public Placeable? PlacedObject { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
