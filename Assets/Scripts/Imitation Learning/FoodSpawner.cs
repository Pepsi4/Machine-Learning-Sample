using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private Transform lastTransformFood;
    private bool hasFoodSpawned = false;
    public bool HasFoodSpawned
    {
        get { return hasFoodSpawned; }
        set { hasFoodSpawned = value; }
    }

    public Transform GetLastTransformFood() => lastTransformFood;
    [SerializeField] Food food;
    public void SpawnFood()
    {
        HasFoodSpawned = true;
        food.gameObject.SetActive(true);
        food.SetRandomPos();
        lastTransformFood = food.transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
