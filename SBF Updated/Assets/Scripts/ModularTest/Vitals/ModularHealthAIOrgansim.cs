using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularHealthAIOrgansim : ModularHealthOrganism
{
    AudioSource aS;
    AudioProfile audioProfile;

    protected override void Init(ModularController c)
    {
        base.Init(c);
        ModularControllerAI ai = (ModularControllerAI)c;
        aS = ai.VoiceAS;
        audioProfile = ai.Voice;
    }

    public override bool DamageHealth(float amount)
    {
        audioProfile.PlayHitSelf(aS);
        return base.DamageHealth(amount);
    }
}
