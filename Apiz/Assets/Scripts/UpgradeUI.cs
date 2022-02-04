using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{
    public Image Image;
    public TMP_Text Text;
    public TMP_Text TextCost;
    public TMP_Text Description;
    private Upgrade _upgrade;

    public void Initialize(Upgrade upgrade)
    {
        _upgrade = upgrade;
        Image.sprite = upgrade.Sprite;
        Text.text = upgrade.Name;
        TextCost.text = upgrade.Cost + "";
        Description.text = upgrade.Description;
    }

    public void OnClick()
    {        
        if (MainGame.Instance.money >= _upgrade.Cost)
        {
            MainGame.Instance.OnclickUpgrade(_upgrade);
            transform.DOComplete();
            transform.DOPunchScale(new Vector3(0.1f, 0.2f, 0), 0.2f);
        }        
    }
}
