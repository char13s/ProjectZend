using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSword : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    private void OnEnable() {
        Instantiate(particles,transform);
    }
}
