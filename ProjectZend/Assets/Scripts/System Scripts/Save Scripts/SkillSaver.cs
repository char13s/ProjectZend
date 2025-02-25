using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SkillSaver 
{
    string skillName;
    bool unlockedOrNot;
    public SkillSaver(string name,bool unlockable) {
        SkillName = name;
        UnlockedOrNot = unlockable;
    }
    public string SkillName { get => skillName; set => skillName = value; }
    public bool UnlockedOrNot { get => unlockedOrNot; set => unlockedOrNot = value; }
}
