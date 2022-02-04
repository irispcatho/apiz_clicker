using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DestroySeal : MonoBehaviour
{
    public GameObject PrefabLoot;
    private float _timer;
    //private DestroySeal destroySeal;
    public void OnMouseOver()
    {
        FindObjectOfType<AudioManager>().Play("Seal");
        PrefabLoot.transform.DOMove(new Vector2(-7.2f, 3.9f), 0.8f);
        Object.Destroy(PrefabLoot, 0.8f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 5)
        {
            PrefabLoot.transform.DOMove(new Vector2(-7.2f, 3.9f), 0.8f);
            Object.Destroy(PrefabLoot, 0.8f);
        }
    }
}
