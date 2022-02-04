using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Description : MonoBehaviour
{
    public GameObject description;

    private void Start()
    {
        description.SetActive(false);
    }

    private void OnMouseOver()
    {
        description.SetActive(true);
    }

    private void OnMouseExit()
    {
        description.SetActive(false);
    }
}
