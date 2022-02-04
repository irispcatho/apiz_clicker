using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NewCompetenceUI : MonoBehaviour
{
    public Image Image;
    public TMP_Text Text;
    public TMP_Text TextCost;
    public TMP_Text Description;
    private NewCompetence _newCompetence;

    public void Initialize(NewCompetence newCompetence)
    {
        _newCompetence = newCompetence;
        Image.sprite = newCompetence.Sprite;
        Text.text = newCompetence.Name;
        TextCost.text = newCompetence.Cost + "";
        Description.text = newCompetence.Description;
    }

    public void OnClick()
    {
        if(MainGame.Instance.money >= _newCompetence.Cost)
        {
            MainGame.Instance.OnclickNewCompetence(_newCompetence);
            transform.DOComplete();
            transform.DOPunchScale(new Vector3(0.1f, 0.2f, 0), 0.2f);
        }        
    }
}