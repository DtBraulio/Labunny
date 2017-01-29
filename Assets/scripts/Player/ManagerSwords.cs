using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSwords : MonoBehaviour
{

    public GameObject[] swordsGO;
    public Swords[] swordScrpt;
    public int swordCrnt;
    public bool isAttacking;

    // Use this for initialization
    void Start()
    {
        swordScrpt = new Swords[swordsGO.Length];

        for (int i = 0; i < swordsGO.Length; i++)
        {
            swordScrpt[i] = swordsGO[i].GetComponent<Swords>();
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {

            Collider[] collider;

            collider = Physics.OverlapBox(transform.position, swordScrpt[swordCrnt].size / 2, transform.rotation, swordScrpt[swordCrnt].mask);

            if (collider.Length != 0)
            {
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].tag == "Enemy")
                    {
                        //TODO: llamar al reciveDaamage del enemigo
                    }
                }
            }
        }

    }
    public void ActiveSword (int swordNum)
    {
        for (int i = 0; i < swordsGO.Length; i++)
        {
            swordsGO[i].SetActive(false);
        }

        swordsGO[swordNum].SetActive(true);

        swordCrnt = swordNum;
    }
}
