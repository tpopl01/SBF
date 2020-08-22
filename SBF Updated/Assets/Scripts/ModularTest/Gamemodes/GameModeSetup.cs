using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
 (
     fileName = "scenename_gamemode_setup",
     menuName = "GameMode/Setup"
 )]
public class GameModeSetup : ScriptableObject
{
    [SerializeField] SetupObject[] setupObjects = null;

    public virtual void SetUp(Transform folder)
    {
        for (int i = 0; i < setupObjects.Length; i++)
        {
            for (int n = 0; n < setupObjects[i].spawn_pos.Length; n++)
            {
                GameObject g = Instantiate<GameObject>(Resources.Load<GameObject>(setupObjects[i].object_slug), setupObjects[i].spawn_pos[n], Quaternion.identity);
                g.transform.SetParent (folder);
            }
        }
    }

}

[System.Serializable]
public class SetupObject
{
    public string object_slug;
    public Vector3[] spawn_pos;
}
