using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacePoint : MonoBehaviour
{
    [SerializeField]
    public PlaceableType typeAvailable;
    public bool IsPlaceableAt { get; private set; }
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
