using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour
{
    public SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        renderer.sortingOrder = (int)Mathf.Abs(transform.position.y * 10);
    }
}
