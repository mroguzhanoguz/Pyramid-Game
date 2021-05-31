using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public Transform _transform;
    [SerializeField] private Animator _animator;

    public void Jump()
    {
        _animator.SetTrigger("Jump");
    }
}
