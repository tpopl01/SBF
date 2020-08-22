using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMaintenance : MonoBehaviour
{
    GameObject fixedModel;
    GameObject brokenModel;
    Vector3 startPos;
    Quaternion startRot;
    List<SavePositions> savePositions = new List<SavePositions>();

    public void Init(ModularController c)
    {
        startPos = transform.position;
        if (c)
        {
            fixedModel = c.transform.GetChild(0).gameObject;
            brokenModel = c.transform.GetChild(1).gameObject;
        }
        else
        {
            fixedModel = transform.GetChild(0).gameObject;
            brokenModel = transform.GetChild(1).gameObject;
        }
        if (savePositions.Count == 0)
        {
            int count = brokenModel.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform t = brokenModel.transform.GetChild(i);
                savePositions.Add(new SavePositions(t, t.position, t.rotation));
            }
        }
        brokenModel.SetActive(false);
    }

    public void Kill()
    {
        fixedModel.SetActive(false);
        brokenModel.SetActive(true);
    }

    public void Respawn()
    {
        for (int i = 0; i < savePositions.Count; i++)
        {
            savePositions[i].Trans.rotation = savePositions[i].Trans.rotation;
            savePositions[i].Trans.position = savePositions[i].Trans.position;
        }
        fixedModel.SetActive(true);
        brokenModel.SetActive(false);
        transform.position = startPos;
        transform.rotation = startRot;
    }

}

public class SavePositions
{
    public Transform Trans { get; private set; } = null;
    public Vector3 StartPos { get; private set; }
    public Quaternion StartRot { get; private set; }

    public SavePositions(Transform trans, Vector3 startPos, Quaternion startRot)
    {
        this.StartPos = startPos;
        this.StartRot = startRot;
        this.Trans = trans;
    }
}