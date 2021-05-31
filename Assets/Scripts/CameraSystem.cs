using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 followDistance;
    [SerializeField] private HumanPyramid pyramid;


    //Movement is fixed so it must be fixed instead late update.
    public void FixedUpdateCamera()
    {
        Vector3 centerPosition = pyramid.centerPosition;
        Vector3 distanceFactor = new Vector3(0, 0, -(6F + (pyramid.totalBottomElementCount * 0.5F)));
        Vector3 followPosition = centerPosition + followDistance + distanceFactor;
        _transform.position = Vector3.Lerp(_transform.position, followPosition, 15 * Time.deltaTime);
    }
}
