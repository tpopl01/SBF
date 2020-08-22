using System.Collections;
using System.Collections.Generic;
using tpopl001.Events;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerModular : MonoBehaviour
{
    public ModularTeams[] Teams { get; private set; }
    const string TEAM_PATH = "Teams/Modular_Team_";

    [Header("Audio")]
    [SerializeField] AudioSource backgroundMusicAS = null;
    [SerializeField] AudioSource commentatorAS = null;
    [SerializeField] AudioProfileCommentator commentry = null;
    [SerializeField] AudioProfileMusic music = null;

    [Space]
    [Header("Teams")]
    [SerializeField] public int[] teams_id = new int[0];
    GameMode gameMode;

    [Space]
    [Header("Folders")]
    public Transform aiFolder;
    public Transform weaponsFolder;
    public Transform levelSpawnsFolder;

    [Space]
    [Header("UI")]
    [SerializeField] Text team1Score = null;
    [SerializeField] Text team2Score = null;
    [SerializeField] Text endGameTimer = null;

    public int PlayerTeam { get; private set; } = -1;

    [SerializeField]GameModeType debugGameMode;
    [SerializeField] bool debug;

    [SerializeField] int spawnUnitCount;
    bool complete = false;

    public void SetPlayerTeam(int team)
    {
        PlayerTeam = team;
        string load = "Audio/Commentary/Audio_Commentator";
        if (PlayerTeam == 0)
            load += "_REP";
        else load += "_CIS";
        commentry = Resources.Load<AudioProfileCommentator>(load);
        commentry.PlayStartAudio(commentatorAS);
    }

    public static GameManagerModular instance;
    private void Awake()
    {
        instance = this;
        if (teams_id.Length == 0) {
            teams_id =new int[] { 0, 1 };
        }

        Teams = new ModularTeams[teams_id.Length];
        for (int i = 0; i < teams_id.Length; i++)
        {
            Teams[i] = Resources.Load<ModularTeams>(TEAM_PATH + teams_id[i]);
            Teams[i].Init();
        }
        GameModeType g = MainMenu.gameMode;
        if(debug)
        {
            g = debugGameMode;
        }
        gameMode = Resources.Load<GameMode>("GameMode/" + g);
        gameMode.Setup(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "_" + g.ToString(), 1000, levelSpawnsFolder);

        
    }

    private void Start()
    {
        ISpawner team1 = ResourceManagerModular.instance.GetSpawner(teams_id[0]);
        ISpawner team2 = ResourceManagerModular.instance.GetSpawner(teams_id[1]);
        UnitProfile[] team1Units = Resources.LoadAll<UnitProfile>("Modular/Profiles/" + teams_id[0] + "/");
        UnitProfile[] team2Units = Resources.LoadAll<UnitProfile>("Modular/Profiles/" + teams_id[1] + "/");
        for (int i = 0; i < spawnUnitCount; i++)
        {
            UnitProfile t0 = team1Units[Random.Range(0, team1Units.Length)];
            ModularController unit0 = Instantiate(Resources.Load<ModularController>("Modular/Units/"+ teams_id[0] + "/" + t0.model_slug), team1.GetSpawnPosition() + Vector3.up, Quaternion.identity);
            unit0.SetTeam(teams_id[0]);
            unit0.SetStats(t0.stats);
            WeaponSystem ws = unit0.GetComponent<WeaponSystem>();
            ws.defaultWeaponSlug = t0.starting_guns;
            DeactivateSpecials(unit0.transform, t0.additional);

            UnitProfile t1 = team2Units[Random.Range(0, team2Units.Length)];
            ModularController unit1 = Instantiate(Resources.Load<ModularController>("Modular/Units/" + teams_id[1] + "/" + t1.model_slug), team2.GetSpawnPosition() + Vector3.up, Quaternion.identity);
            unit1.SetTeam(teams_id[1]);
            unit1.SetStats(t1.stats);
            ws = unit1.GetComponent<WeaponSystem>();
            ws.defaultWeaponSlug = t1.starting_guns;
            DeactivateSpecials(unit1.transform, t1.additional);
        }

        endGameTimer.gameObject.SetActive(false);

        EventHandling.OnCapture += OnCapture;
        music.PlayGeneralAudio(backgroundMusicAS);
    }

    private void DeactivateSpecials(Transform root, string[] specials)
    {
        SpecialBase1[] children = root.GetComponentsInChildren<SpecialBase1>();
        for (int i = 0; i < children.Length; i++)
        {
            bool contains = false;
            for (int n = 0; n < specials.Length; n++)
            {
                if (children[i].GetName().Contains(specials[n]))
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
            {
                //deactivate
                children[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        int player_index = 0;
        int hostile_index = 1;

        if(PlayerTeam >-1)
        {
            for (int i = 0; i < teams_id.Length; i++)
            {
                if (teams_id[i] == PlayerTeam)
                    player_index = i;
                else// if(GetTeam(teams_id[i]).Score > GetTeam(teams_id[hostile_index]).Score)
                {
                    hostile_index = i;
                }
            }
        }

        team1Score.text = Teams[player_index].Score.ToString();
        team2Score.text = Teams[hostile_index].Score.ToString();
        if(gameMode)
        if (gameMode.IsComplete && !complete)
        {
            complete = true;
            Time.timeScale = 0;
            for (int i = 0; i < Teams.Length; i++)
            {
                for (int n = 0; n < Teams[i].ActiveUnits.Count; n++)
                {
                    StatSheet.instance.AddStats(Teams[i].ActiveUnits[n].Stats);
                }
                if (Teams[i].Score == gameMode.MaxScore)
                {
                    if (Teams[i].GetTeam() == PlayerTeam)
                    {
                        Debug.Log("Victory");
                        music.PlayVictoryAudio(backgroundMusicAS);
                        commentry.PlayVictoryAudio(commentatorAS);
                    }
                    else
                    {
                        Debug.Log("Defeat");
                        music.PlayDefeatAudio(backgroundMusicAS);
                        commentry.PlayDefeatAudio(commentatorAS);
                    }
                    EventHandling.Complete();
                }
            }

        }
    }

    public void AddUnitToTeam(ModularController aI)
    {
        for (int i = 0; i < Teams.Length; i++)
        {
            if (Teams[i].GetTeam() == aI.Team)
            {
                Teams[i].AddUnit(aI);
                break;
            }
        }
    }

    public void RemoveUnitFromTeam(ModularController aI)
    {
        for (int i = 0; i < Teams.Length; i++)
        {
            if (Teams[i].GetTeam() == aI.Team)
            {
                Teams[i].RemoveUnit(aI);
                break;
            }
        }
    }

    public ModularTeams GetTeam(int team)
    {
        for (int i = 0; i < Teams.Length; i++)
        {
            if (Teams[i].GetTeam() == team)
            {
                return Teams[i];
            }
        }
        return null;
    }

    private void OnCapture(int team)
    {
        DetermineAudio();
        if (team == PlayerTeam)
        {
            commentry.PlayCapturedCPAudio(commentatorAS);
        }
        else
        {
            commentry.PlayLostCPAudio(commentatorAS);
        }
    }

    public void OnDeath(int team)
    {
        
    }

    void DetermineAudio()
    {
        if (PlayerTeam == GetTeam(0).GetTeam())
        {
            if (gameMode.VictoryNear(GetTeam(0).Score, GetTeam(1).Score))
            {
                music.PlayWinningAudio(backgroundMusicAS);
                commentry.PlayVictoryNearAudio(commentatorAS);
            }
            else if (gameMode.VictoryNear(GetTeam(1).Score, GetTeam(0).Score))
            {
                music.PlayLosingAudio(backgroundMusicAS);
                commentry.PlayDefeatNearAudio(commentatorAS);
            }
            else
            {
                music.PlayGeneralAudio(backgroundMusicAS);
            }
        }
        else if (PlayerTeam == GetTeam(1).GetTeam())
        {
            if (gameMode.VictoryNear(GetTeam(1).Score, GetTeam(0).Score))
            {
                music.PlayWinningAudio(backgroundMusicAS);
                commentry.PlayVictoryNearAudio(commentatorAS);
            }
            else if (gameMode.VictoryNear(GetTeam(0).Score, GetTeam(1).Score))
            {
                music.PlayLosingAudio(backgroundMusicAS);
                commentry.PlayDefeatNearAudio(commentatorAS);
            }
            else
            {
                music.PlayGeneralAudio(backgroundMusicAS);
            }
        }
    }

    public int GetNearbyUnitCountInTeam(int team, float range, Vector3 pos)
    {
        int count = 0;
        for (int i = 0; i < teams_id.Length; i++)
        {
            if (teams_id[i] != team) continue;
            ModularTeams t = Teams[i];
            for (int n = 0; n < t.ActiveUnits.Count; n++)
            {
                if(Vector3.Distance(t.ActiveUnits[n].transform.position, pos) < range)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void OnDestroy()
    {
        gameMode.DestroyReferences();
        EventHandling.OnCapture -= OnCapture;
    }
}

public enum GameModeType
{
    Conquest,
    CTF
}