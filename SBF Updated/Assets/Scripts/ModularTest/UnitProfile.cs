using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu
  (
      fileName = "unit_profile",
      menuName = "Modular/Components/Profiles/Unit Profile"
  )]
public class UnitProfile : ScriptableObject
{
    public string model_slug;
    public AIStats stats;
    public string[] starting_guns;
    public string[] additional;
}
