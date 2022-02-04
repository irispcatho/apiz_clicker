using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public static MainGame Instance; //

    //// Joueur
    //Vie du joueur
    public int pdvPlayer;           //Vie actuelle du joueur
    private int pdvPlayerMax = 1000; //Vie de départ (et maximum) du joueur
    public Image ImageFill;//Affichage des points de vie
    //Dégâts du joueur
    private int PlayerDamage;         //Dégât du joueur au clic
    public int StartPlayerDamage;

    //// Ennemis
    private int killedEnnemies;         //Nombres d'ennemis tués
    public TMP_Text numberEnnemies;         //Nombres d'ennemis total
    public List<MonsterInfos> Monsters; //Liste de monstres
    private int _currentMonster;        //
    public Monster Monster;
    public Canvas CanvasSeal; //Appel du script monster

    //// Shop
    //Boutons du shop
    public GameObject PrefabUpgradeUI;  //Bouton Upgrade
    public GameObject ParentUpgrades;   //Contenant du bouton upgrade
    public GameObject PrefabNewCompetenceUI;    //Bouton NewCompetence
    public GameObject ParentNewCompetences;     //Contenant du bouton NewCompetence
    public List<Upgrade> Upgrades;              //Liste des boutons Upgrade
    public List<NewCompetence> NewCompetences;  //Liste des boutons NewCompetence
    //public List<Upgrade> _unlockedUpgrades = new List<Upgrade>();   //
    public Button ButtonShop1;
    public Button ButtonShop2;
    /*private bool shopStart;
    private bool button1AlreadyUp;
    private bool button2AlreadyUp;*/
    //Contenant des éléments des shop
    public GameObject Content;  //Window 1
    public GameObject Content2; //Window 2
    //Argents du jeu
    public int money = 0;              //Nombre de sceaux
    public TMP_Text currentMoney;           //Affichage du nombre de sceaux
    private int concentration;          //Nombre de points de concentration
    public TMP_Text currentConcentration;   //Affichage du nombre de point de concentration
    //Scroll View
    public GameObject Sv1;
    public GameObject Sv2;
    //Var shop
    private bool Buy;
    //Var éléments Upgrade
    private bool parcheminDeConcentration;
    //Var éléments New Competence
    public bool bonusLoot;
    public bool StopBonusLoot = false;
    private int Invulnerable = 0;

    //// FeedBack
    //public GameObject PrefabHitPoint;   //FeedBack de l'attaque du joueur
    public GameObject PrefabLoot;       //FeedBack des sceaux que récolte le joueur

    public float _timer;
    private bool skipToNext;

    private void Awake()
    {
        Instance = this;
        UpdateLifePlayer();
    }

    void Start()
    {
        Buy = false; //On empêche le joueur de pouvoir utiliser les boutons du shop
        parcheminDeConcentration = false;
        PlayerDamage = StartPlayerDamage;
        Monster.SetMonster(Monsters[_currentMonster]);

        FindObjectOfType<AudioManager>().Play("MusicLv1");
        skipToNext = false;

        _timer = Monster._timer;
        //pdvPlayer = pdvPlayerMax;

        foreach (var newCompetence in NewCompetences)
        {
            GameObject go = GameObject.Instantiate(PrefabNewCompetenceUI, ParentNewCompetences.transform, false);
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<NewCompetenceUI>().Initialize(newCompetence);
        }

        //Création des éléments du shop
        foreach (var upgrade in Upgrades)
        {
            GameObject go = GameObject.Instantiate(PrefabUpgradeUI, ParentUpgrades.transform, false);
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<UpgradeUI>().Initialize(upgrade);
        }

        OnClickShop1(); //On affiche la window 1 du shop
    }

    void Update()
    {

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = Monster._timer;
            AttackMonsters(Monster);
        }

        OnClickMonster();
        UpdateLifePlayer();
        currentMoney.text = "" + money;
        numberEnnemies.text = killedEnnemies + " / 13";
        currentConcentration.text = concentration + "";

        if (_currentMonster >= 4 && _currentMonster < 8)
        {
            if (!skipToNext)
            {
                skipToNext = true;
                //Debug.Log("Niveau 2");
                FindObjectOfType<AudioManager>().Play("MusicLv2");
                FindObjectOfType<AudioManager>().Stop("MusicLv1");

            }
            else
                return;
        }

        if (_currentMonster >= 8)
        {
            if (skipToNext)
            {
                skipToNext = false;
                //Debug.Log("Niveau 3");
                FindObjectOfType<AudioManager>().Play("MusicLv3");
                FindObjectOfType<AudioManager>().Stop("MusicLv2");
            }
            else
                return;
        }

        

        if (pdvPlayer <= 0)
            SceneManager.LoadScene("Scene_RIP");

    }

    private void UpdateLifePlayer()
    {
        float percent = (float)pdvPlayer / (float)pdvPlayerMax;
        ImageFill.fillAmount = percent;
        if (pdvPlayer > pdvPlayerMax)
            pdvPlayer = 1000;
    }

    public void OnClickMonster()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);      

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.name == "Monster")
            {
                Hit(PlayerDamage, Monster);
                FindObjectOfType<AudioManager>().Play("EnnemyDamage");
                if (!parcheminDeConcentration)
                    concentration++;
                else
                    concentration += 5;
            }
        }
    }

    //Boutons choix window du shop
    public void OnClickShop1()
    {
        Content.SetActive(true);
        Sv1.SetActive(true);
        Content2.SetActive(false);
        Sv2.SetActive(false);
    }

    public void OnClickShop2()
    {
        Content2.SetActive(true);
        Sv2.SetActive(true);
        Content.SetActive(false);
        Sv1.SetActive(false);
    }


    //Achat d'un element du shop Upgrade
    private void BuyUpgrade(Upgrade upgrade)
    {
        if (money >= upgrade.Cost)
        {
            Buy = true;
            //Debug.Log("ok");
        }
        else
        {
            return;
        }
    }

    //Fonctions Boutons Upgrade
    public void OnclickUpgrade(Upgrade upgrade) //Ajouter la fonction sur les boutons
    {
        switch (upgrade.WhatFonction)
        {
            case 1: // Rend 175pv au joueur
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    if (pdvPlayer < pdvPlayerMax/* - 175*/)
                    {
                        pdvPlayer += 175;
                        money -= upgrade.Cost;
                        Buy = false;
                    }
                }
                break;
            case 2: //rend 450 PV au joueur
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    if (pdvPlayer < pdvPlayerMax/* - 450*/)
                    {
                        pdvPlayer += 450;
                        money -= upgrade.Cost;
                        Buy = false;
                    }
                }
                break;
            case 3:
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    StartCoroutine(ParcheminDeRegen());
                    money -= upgrade.Cost;
                    Buy = false;

                }
                break;
            case 4:
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    StartCoroutine(ParcheminDeConcentration());
                    parcheminDeConcentration = true;
                    money -= upgrade.Cost;
                    Buy = false;
                }
                break;
            case 5:
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    Monster._life -= 500;
                    money -= upgrade.Cost;
                    Buy = false;
                    if (!Monster.IsAlive())
                    {
                        NextMonster(Monster);
                    }
                }
                break;
            case 6:
                BuyUpgrade(upgrade);
                if (Buy == true)
                {
                    StartCoroutine(FioleDePoison());
                    money -= upgrade.Cost;
                    Buy = false;
                }
                break;
        }
    }

    //Achat d'un element du shop NewCompetence
    private void BuyNewCompetence(NewCompetence newCompetence)
    {
        if (concentration >= newCompetence.Cost)
        {
            concentration -= newCompetence.Cost;
            Buy = true;
            //Debug.Log("ok");
        }
        else
        {
            return;
        }
    }

    //Fonctions Boutons New Competence
    public void OnclickNewCompetence(NewCompetence newCompetence) //Ajouter la fonction sur les boutons
    {
        switch (newCompetence.WhatFonction)
        {
            case 1: //Vole 35 sceaux à l’ennemi
                BuyNewCompetence(newCompetence);
                if (Buy == true)
                {
                    money += 35;
                    Buy = false;
                }
                break;
            case 2: //Augmente le chrono d'un ennemi de 10 secondes
                BuyNewCompetence(newCompetence);
                if (Buy == true)
                {
                    Monster._timer += 10;
                    Buy = false;
                }
                break;
            case 3: //Invulnerabilise le joueur pour le prochain coup de l'ennemi
                if (Invulnerable < 3)
                {
                    BuyNewCompetence(newCompetence);
                    if (Buy == true)
                    {
                        Invulnerable++;
                        Buy = false;
                    }
                }
                break;
            case 4: //Augmente de 50% le nombre de sceaux obtenus à la mort d’un ennemi
                BuyNewCompetence(newCompetence);
                if (Buy == true)
                {
                    //Multiplier par 1.5 la valeur du loot de chaque ennemi pendant 30 sec
                    StartCoroutine(BoostDeSceaux());
                    Monster._loot = (int)(Monster._loot * 1.5f);
                    Buy = false;
                    //Debug.Log("Debut : " + Monster._loot);
                }
                break;
            case 5://Augmente de 50% les dégâts du joueur pendant 30sec
                BuyNewCompetence(newCompetence);
                if (Buy == true)
                {
                    StartCoroutine(BoostDeDegat());
                    PlayerDamage = (int)(PlayerDamage * 1.5f);
                    Buy = false;
                    //Debug.Log("Debut : " + Dammage);
                }
                break;
        }
    }

    //Coroutines


    public void AttackMonsters(Monster monster) //attacque des monstres au bout d'un certain temps sans les tuer
    {
        if (Invulnerable <= 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerDamage");
            if (_currentMonster < 4)
            {
                CameraShaker.Instance.ShakeOnce(1f, 4f, .1f, 1f);
                //Debug.Log("Shake faible");
            }

            if ((_currentMonster >= 4) && (_currentMonster < 9))
            {
                CameraShaker.Instance.ShakeOnce(3f, 4f, .1f, 1f);
                //Debug.Log("Shake moyen");
            }

            if (_currentMonster >= 9)
            {
                CameraShaker.Instance.ShakeOnce(5f, 4f, .1f, 1f);
                //Debug.Log("Shake fort");
            }


            if (_currentMonster == 13)
            {
                CameraShaker.Instance.ShakeOnce(6f, 4f, .1f, 1f);
                //  Debug.Log("Shake ultime");
            }

            pdvPlayer -= monster._attack;
            UpdateLifePlayer();
        }
        Invulnerable--;
        if (Invulnerable < 0)
            Invulnerable = 0;
    }

    IEnumerator ParcheminDeRegen() //rend 8pv/sec au joueur pendant une minute 
    {
        int timer = 0;
        while (timer <= 59)
        {
            yield return new WaitForSeconds(1);
            timer++;
            pdvPlayer += 8;
        }
    }

    IEnumerator ParcheminDeConcentration() //annule le bonus de concentration au bout de 30 sec
    {
        yield return new WaitForSeconds(30);
        parcheminDeConcentration = false;
    }

    IEnumerator FioleDePoison()
    {

        int timer = 0;
        while (timer <= 9)
        {
            yield return new WaitForSeconds(1);
            timer++;
            Monster._life -= 10;
        }
    }

    IEnumerator BoostDeSceaux() //annule le bonus de sceau au bout de 30 sec
    {
        yield return new WaitForSeconds(30);
        Monster._loot = (int)(Monster._loot / 1.5f);
        //Debug.Log("Fin : " + Monster._loot);
    }

    IEnumerator BoostDeDegat() //annule le bonus de degats au bout de 30 sec
    {
        yield return new WaitForSeconds(30);
        PlayerDamage = (int)(PlayerDamage / 1.5f);
        if (PlayerDamage < StartPlayerDamage)
            PlayerDamage = StartPlayerDamage;
        //Debug.Log("Fin : " + Dammage);
    }
    public void Hit(int damage, Monster monster)
    {
        monster.Hit(damage);
        if (monster.IsAlive() == false)
        {
            money += monster._loot;
            NextMonster(monster);
        }

        //Debug.Log(money);
    }

    public void NextMonster(Monster monster)
    {
        FindObjectOfType<AudioManager>().Play("MonsterDeath");
        if (_currentMonster < 13)
        {            
            _currentMonster++;
            killedEnnemies++;
            Monster.SetMonster(Monsters[_currentMonster]);
            _timer = monster._timer;
            for (int i = 0; i < Random.Range(5f, 10f); i++)
            {
                GameObject loot = GameObject.Instantiate(PrefabLoot, CanvasSeal.transform, false);
                loot.transform.DOJump(new Vector2(monster.transform.position.x + Random.Range(-2f, 3f), monster.transform.position.y - 4), 3, 1, 0.8f);
            }
        }
        else
            SceneManager.LoadScene("Scene_Menu");

    }
}