using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class MagazineFollower : MonoBehaviour
{
    private float heightOffset = 0.00005f;
    [SerializeField] private Magazine parentMagazine;
    [SerializeField] private Transform magazineStack;
    [SerializeField] List<GameObject> bulletList = new List<GameObject>();
    private int currentBullets;
    
    private void Start()
    {
        currentBullets = parentMagazine.CurrentAmmo;
        
        foreach (GameObject bullet in bulletList)
        {
            foreach(var renderer in bullet.GetComponentsInChildren<Renderer>())
                renderer.enabled = false;
        }

        for (int i = 0; i < currentBullets; i++)
        {
            foreach (var renderer in bulletList[i].GetComponentsInChildren<Renderer>())
                renderer.enabled = true;
        }

        int emptyBullets = bulletList.Count - currentBullets;
        magazineStack.localPosition += new Vector3(0f, 0f, heightOffset * emptyBullets);
    }

    public void Use()
    {
        if (currentBullets <= 0)
            return;

        currentBullets--;
        bulletList[currentBullets].gameObject.SetActive(false);

        if (currentBullets > 0)
            magazineStack.localPosition += new Vector3(0f, 0f, heightOffset);
    }
}
