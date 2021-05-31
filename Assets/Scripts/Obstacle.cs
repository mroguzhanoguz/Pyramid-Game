using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private HumanPyramid humanPyramid;

    public void Initialize(HumanPyramid _humanPyramid)
    {
        humanPyramid = _humanPyramid;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HumanPyramid"))
        {
            humanPyramid.OnObstacleHitHuman(other.GetComponent<Human>());
        }
    }
}
