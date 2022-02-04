using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Monster : MonoBehaviour
{
    public int _life;
    public Image ImageLife;
    [SerializeField] int _lifeMax;
    public GameObject Visual;
    public Canvas Canvas;
    public int _loot;
    public bool bonusLoot;
    private int StartLoot;
    public int _attack;
    public float _timer;
    public GameObject Image_Background;
    public GameObject Image_Platform;
    public TMP_Text Name;
    private string _name;
    public GameObject ImageChronoMonster;
    public TMP_Text TextChrono;
    private bool ChronoTransformWork = true;

    public MainGame mainGame;

    private void Awake()
    {
        UpdateLife();
        StartLoot = _loot;
    }

    private void Update()
    {
        UpdateLife();
        int Chrono = (int)(mainGame._timer);
        TextChrono.text = Chrono.ToString();
        switch (Chrono)
        {
            case 4:
                if (ChronoTransformWork == true)
                {
                    ChronoTransformWork = false;
                    StartCoroutine(ChronoTransform());
                }
                break;
            case 3:
                if (ChronoTransformWork == true)
                {
                    ChronoTransformWork = false;
                    StartCoroutine(ChronoTransform());
                }
                break;
            case 2:
                if (ChronoTransformWork)
                {
                    ChronoTransformWork = false;
                    StartCoroutine(ChronoTransform());
                }
                break;
            case 1:
                if (ChronoTransformWork)
                {
                    ChronoTransformWork = false;
                    StartCoroutine(ChronoTransform());
                }
                break;
        }
    }

    IEnumerator ChronoTransform()
    {
        yield return new WaitForSeconds(1f);
        ImageChronoMonster.transform.DOComplete();
        ImageChronoMonster.transform.DOPunchScale(new Vector2(0.4f, 0.4f), 0.5f);
        ChronoTransformWork = true;
    }

    public bool IsAlive()
    {
        return _life > 0;
    }
    public void SetMonster(MonsterInfos infos)
    {
        _lifeMax = infos.Life;
        _life = _lifeMax;
        _loot = infos.Loot;
        _attack = infos.Attack;
        _timer = infos.Timer;
        _name = infos.Name;
        UpdateLife();
        Visual.GetComponent<SpriteRenderer>().sprite = infos.Sprite;
        Image_Background.GetComponent<SpriteRenderer>().sprite = infos.Background;
        Image_Platform.GetComponent<SpriteRenderer>().sprite = infos.Platform;
    }

    private void UpdateLife()
    {
        //TextLife.text = $"{_life}/{_lifeMax}";
        Name.text = _name;
        float percent = (float)_life / (float)_lifeMax;
        ImageLife.fillAmount = percent;
    }

    public void Hit(int damage)
    {
        _life -= damage;
        UpdateLife();
        Visual.transform.DOComplete();
        Visual.transform.DOPunchScale(new Vector3(0.05f, 0.2f, 0), 0.15f);
    }
}
