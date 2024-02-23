using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseRayCast : MonoBehaviour
{
    private bool stopMouse = false;
    private Vector3 mousePos;
    public List<GameObject> areaObjs;
    private int area1CT = 0;
    private int area2CT = 0;
    private int area3CT = 0;
    //private int area4CT = 0;
    private int overallCount = 0;
    public int overallCountMax = 9;
    public GameObject nextSculpture;
    public float audioTime;
    public GameObject audioHolder;
    public AudioSource[] audioCracking;

    private void Start()
    {
        audioCracking = audioHolder.GetComponents<AudioSource>();
    }

    void Update()
    {

        while (stopMouse)
        {
            return;    
        }
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitClick = Physics2D.Raycast
            (mousePos, Vector3.back, Mathf.Infinity, LayerMask.GetMask("Areas"));

        if (Input.GetMouseButtonDown(0))
        {
            if (hitClick.collider != null)
            {
                string objName = hitClick.collider.gameObject.name;
                print(objName);

                switch (objName)
                {
                    case "Area1":
                        if (area1CT <= 2)
                        {
                            LoadCracks(0, area1CT);
                            area1CT++;
                        }
                        break;
                    case "Area2":
                        if (area2CT <= 2)
                        {
                            LoadCracks(1, area2CT);
                            area2CT++;
                        }
                        break;
                    case "Area3":
                        if (area3CT <= 2)
                        {
                            LoadCracks(2, area3CT);
                            area3CT++;
                        }
                        break;
                    // case "Area4":
                    //     if (area4CT <= 2)
                    //     {
                    //         LoadCracks(3, area4CT);
                    //         area4CT++;
                    //     }
                    //     break;
                }
            }
        }
    }

    void LoadCracks(int areaNo, int CT)
    {
        print(areaNo + " " + CT);
        GameObject crack = Instantiate(areaObjs[areaNo].GetComponent<LoadCracks>().cracks[CT]);
        crack.transform.position = areaObjs[areaNo].transform.position;
        crack.transform.parent = gameObject.transform;
        audioCracking[overallCount].Play();
        overallCount++;
        
        if (overallCount == overallCountMax)
        {
            LoadCrackingRoutine();
            DisableClicking();
        }
    }

    void DisableClicking()
    {
        stopMouse = true;
    }

    void LoadCrackingRoutine()
    {
        StartCoroutine(LoadAnim());
    }

    IEnumerator LoadAnim()
    {
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().sortingOrder = 10;
        GetComponent<Animator>().SetTrigger("StartBreaking");
    }

    void LoadAudio()
    {
        GetComponent<AudioSource>().Play();
    }

    public void LoadNext()
    {
        Instantiate(nextSculpture);
    }
    
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
