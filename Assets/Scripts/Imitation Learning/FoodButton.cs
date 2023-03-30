using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    private bool _canUseButton = true;
    public bool CanUseButton
    {
        get { return _canUseButton; }
        set { _canUseButton = value; }
    }

    [SerializeField] FoodSpawner spawner;

    public void UseButton()
    {
        spawner.SpawnFood();
        _canUseButton = false;
    }

    private void OnEnable()
    {
        _canUseButton = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<FoodAgent>(out FoodAgent agent))
        //{
        //    if (spawner.HasFoodSpawned() == false)
        //    {

        //    }
        //}
    }
}
