using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreation : MonoBehaviour
{
    int team = -1;
    [SerializeField]GameObject teamGO = null;
    [SerializeField] GameObject characterSelectionGO = null;
    GameObject model;
    UnitSelection[] unitSelections = null;
    int index = 0;
    [SerializeField] Text weaponPrimary = null;
    [SerializeField] Text weaponSecondary = null;
    [SerializeField] Text nameText = null;
    GameObject podium;
    [SerializeField]GameObject confirmButton = null;
    [SerializeField] Text additional1 = null;
    [SerializeField] Text additional2 = null;
    [SerializeField] ModularControllerPlayer player;
    bool selection;
    private void Awake()
    {
        podium = Instantiate(Resources.Load<GameObject>("Podium"), Vector3.zero + Vector3.up * 100, Quaternion.identity);
        if(GameObject.FindObjectOfType<WeaponSystem_Player>())
        {
            characterSelectionGO.SetActive(false);
            teamGO.SetActive(false);
            podium.SetActive(false);
        }
        else
        {
            characterSelectionGO.SetActive(false);
            teamGO.SetActive(true);
            podium.SetActive(true);
            CameraManager.instance.gameObject.SetActive(false);
            selection = true;
            player = Instantiate<ModularControllerPlayer>(player, podium.transform.position, podium.transform.rotation);
            player.transform.position = podium.transform.position;
            player.transform.rotation = podium.transform.rotation;
        }
        confirmButton.SetActive(false);

        unitSelections = new UnitSelection[]
        {
            new UnitSelection("CIS_engineer", "ACR", "SE-14_sidearm", 1, "Fusion Cutter", "HealthAmmo"),
            new UnitSelection("CIS_heavy", "E-60R", "SE-14_sidearm", 1, "Turret", "Grenade"),
            new UnitSelection("CIS_sniper", "E5", "SE-14_sidearm", 1, "Turret", "Grenade"),
            new UnitSelection("CIS_standard", "droid_blaster", "SE-14_sidearm", 1, "Turret", "Grenade"),
            new UnitSelection("REP_engineer", "ACR", "DC-15_sidearm", 0, "Fusion Cutter", "HealthAmmo"),
            new UnitSelection("REP_heavy", "PLX-1", "DC-15_sidearm", 0, "Turret Holder", "Grenade"),
            new UnitSelection("REP_standard", "DC-15", "DC-15_sidearm", 0, "Turret", "Grenade"),
            new UnitSelection("REP_sniper", "DC-15x", "DC-15_sidearm", 0, "Turret", "Grenade")
        };
    }

    void InitPlayerForSelection()
    {
        player.GetComponent<CapsuleCollider>().isTrigger = true;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<PlayerInput>().enabled = false;
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(selection)
            InitPlayerForSelection();
    }

    public void SelectCharacter(string character_slug)
    {
        if (team == 0)
            character_slug = "REP_" + character_slug;
        else
        {
            character_slug = "CIS_" + character_slug;
        }
        if (model != null)
            Destroy(model);

        for (int i = 0; i < unitSelections.Length; i++)
        {
            if (unitSelections[i].character_slug.Equals(character_slug))
            {
                index = i;
                nameText.text = unitSelections[i].character_slug;
                model = Instantiate(Resources.Load<GameObject>("Modular/Model/" + unitSelections[i].character_slug), podium.transform.position, podium.transform.rotation);
                model.transform.parent = player.transform;
                model.transform.localPosition = Vector3.zero;
                model.transform.rotation = player.transform.rotation;
                model.gameObject.AddComponent<IK>();
                player.GetComponent<WeaponSystem_Player>().defaultWeaponSlug = new string[] { unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary };
                player.GetComponent<WeaponSystem_Player>().GetComponent<ISetup>().SetUp(player.transform);
                SetWeapons(unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary, unitSelections[index].additional1, unitSelections[index].additional2);
                confirmButton.SetActive(true);
                break;
            }
        }
    }

    public void SelectTeam(int team)
    {
        this.team = team;
        teamGO.SetActive(false);
        characterSelectionGO.SetActive(true);
        teamGO.SetActive(false);
        GameManagerModular.instance.SetPlayerTeam(team);
        
    }

    public void NextCharacter()
    {
        if (model != null)
            Destroy(model);
        index++;
        if (index >= unitSelections.Length)
            index = 0;

        while (unitSelections[index].team != team)
        {
            index++;
            if (index >= unitSelections.Length)
                index = 0;
        }

        model = Instantiate(Resources.Load<GameObject>("Modular/Model/" + unitSelections[index].character_slug), podium.transform.position, podium.transform.rotation);
        model.transform.parent = player.transform;
        model.transform.localPosition = Vector3.zero;
        model.transform.rotation = player.transform.rotation;
        model.gameObject.AddComponent<IK>();
        player.GetComponent<WeaponSystem_Player>().defaultWeaponSlug = new string[] { unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary };
        player.GetComponent<WeaponSystem_Player>().GetComponent<ISetup>().SetUp(player.transform);
        SetWeapons(unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary, unitSelections[index].additional1, unitSelections[index].additional2);
        nameText.text = unitSelections[index].character_slug;
        confirmButton.SetActive(true);
    }

    public void PrevCharacter()
    {
        if (model != null)
            Destroy(model);
        index--;
        if (index < 0)
            index = unitSelections.Length-1;

        while (unitSelections[index].team != team)
        {
            index--;
            if (index < 0)
                index = unitSelections.Length - 1;
        }

        model = Instantiate(Resources.Load<GameObject>("Modular/Model/" + unitSelections[index].character_slug), podium.transform.position, podium.transform.rotation);
        model.transform.parent = player.transform;
        model.transform.localPosition = Vector3.zero;
        model.transform.rotation = player.transform.rotation;
        model.gameObject.AddComponent<IK>();
        player.GetComponent<WeaponSystem_Player>().defaultWeaponSlug = new string[] { unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary };
        player.GetComponent<WeaponSystem_Player>().GetComponent<ISetup>().SetUp(player.transform);
        SetWeapons(unitSelections[index].weapon_slug_primary, unitSelections[index].weapon_slug_secondary, unitSelections[index].additional1, unitSelections[index].additional2);
        nameText.text = unitSelections[index].character_slug;
        confirmButton.SetActive(true);
    }

    void SetWeapons(string primary, string secondary, string additional1, string additional2)
    {
        weaponPrimary.text = primary;
        weaponSecondary.text = secondary;
        this.additional1.text = additional1;
        this.additional2.text = additional2;
    }

    public void ConfirmSelection()
    {
        //AISensors_Player character = Instantiate(Resources.Load<AISensors_Player>("Model/player"));
        //character.team = team;
        //character.debugTeam = team;
        //GameManager.instance.SetPlayer(character);
        //WeaponManager_Player wpPlayer = character.GetComponent<WeaponManager_Player>();
        //wpPlayer.defaultWeaponSlug = new string[]
        //{
        //    weaponPrimary.text, weaponSecondary.text
        //};
        //model.transform.parent = character.transform;
        //model.transform.localPosition = Vector3.zero;
        //model.transform.rotation = character.transform.rotation;
        //SpecialBase[] items = model.GetComponentsInChildren<SpecialBase>();
        //for (int i = 0; i < items.Length; i++)
        //{
        //    if(!items[i].name.Contains(additional2.text) && !items[i].name.Contains(additional1.text))
        //    {
        //        items[i].gameObject.SetActive(false);
        //    }
        //}
        //character.Initialise(team);
        //SpawnManager.instance.AddToSpawn(character);
        //gameObject.SetActive(false);
        //podium.SetActive(false);
        podium.SetActive(false);
        gameObject.SetActive(false);
        player.SetTeam(team);

        SpecialBase1[] s = player.GetComponentsInChildren<SpecialBase1>();
        for (int i = 0; i < s.Length; i++)
        {
            if(!s[i].GetName().ToLower().Contains(additional1.text.ToLower()) && !s[i].GetName().ToLower().Contains(additional2.text.ToLower()))
            {
                s[i].gameObject.SetActive(false);
            }
        }

        Jetpack j = player.GetComponentInChildren<Jetpack>();
        Jumppack jp = player.GetComponentInChildren<Jumppack>();
        if(Random.Range(0,10) < 5)
        {
            j.gameObject.SetActive(true);
            jp.gameObject.SetActive(false);
        }
        else
        {
            j.gameObject.SetActive(false);
            jp.gameObject.SetActive(true);
        }

        player.InitExtra();
        player.GetComponent<CapsuleCollider>().isTrigger = false;
      //  player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<Rigidbody>().MovePosition(ResourceManagerModular.instance.GetSpawner(team).GetSpawnPosition());
        
        CameraManager.instance.gameObject.SetActive(true);
        CameraManager.instance.SetPlayer(player.transform);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<Rigidbody>().useGravity = true;
        //Rigidbody p = player.GetComponent<Rigidbody>();
        //p.isKinematic = false;
        //p.velocity = Vector3.zero;
    }

}

public class UnitSelection
{
    public string character_slug;
    public string weapon_slug_primary;
    public string weapon_slug_secondary;
    public string additional1;
    public string additional2;
    public int team;
    public UnitSelection(string character_slug, string weapon_slug_primary, string weapon_slug_secondary, int team, string additional1, string additional2)
    {
        this.character_slug = character_slug;
        this.weapon_slug_primary = weapon_slug_primary;
        this.weapon_slug_secondary = weapon_slug_secondary;
        this.team = team;
        this.additional1 = additional1;
        this.additional2 = additional2;
    }
}
