using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularControllerPlayer : ModularControllerUnit
{
    bool firstPerson = false;
    IFirstPersonSwitch[] firstPersonSwitch;
    GameObject model;
    public void InitExtra()
    {
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name.Equals("Head"))
            {
                Senses s = allChildren[i].gameObject.AddComponent<Senses>();
                Debug.Log("senses added");
                s.Init(transform, "Modular/Determine/Determine_targets_raycast");
                break;
            }
        }
        Initialise();
        ISetup[] setups = GetComponentsInChildren<ISetup>();
        for (int i = 0; i < setups.Length; i++)
        {
            setups[i].SetUp(transform);
        }
        ticks = GetComponentsInChildren<ITick>();
        fixedTicks = GetComponentsInChildren<IFixedTick>();
        iAims = GetComponentsInChildren<IAim>();
        iAimings = GetComponentsInChildren<IAiming>();
        GameManagerModular.instance.RemoveUnitFromTeam(this);
        GameManagerModular.instance.AddUnitToTeam(this);
        firstPersonSwitch = GetComponentsInChildren<IFirstPersonSwitch>();
        model = transform.GetChild(0).gameObject;
        Stats.Team = Team;
        Debug.Log(Senses);
    }
   
    protected override void Tick()
    {
        if (anim == null)
            return;
        base.Tick();

        if(Input.GetKeyDown(KeyCode.F))
        {
            firstPerson = !firstPerson;
            for (int i = 0; i < firstPersonSwitch.Length; i++)
            {
                firstPersonSwitch[i].SetFirstPerson(firstPerson);
                model.SetActive(!firstPerson);
            }
        }

    }
}
