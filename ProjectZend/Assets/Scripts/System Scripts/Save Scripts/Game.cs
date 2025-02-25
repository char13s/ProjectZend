using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Game {
    private List<RelicData> items;
    private Stats stats;
    private int orbCount;
    private float[] position=new float[3];
    private List<SkillSaver> skillsavers;
    private SkillSaver skSlot1;
    private SkillSaver skSlot2;
    private SkillSaver skSlot3;
    private SkillSaver skSlot4;
    private RelicData relSlot1;
    private RelicData relSlot2;
    private RelicData relSlot3;
    private RelicData relSlot4;
    //private Vector3 spawn;
    private int lastLevel;
    public List<RelicData> Items { get => items; set => items = value; }
    public Stats Stats { get => stats; set => stats = value; }
    public float[] Position { get => position; set => position = value; }
    public int OrbCount { get => orbCount; set => orbCount = value; }
    public List<SkillSaver> Skillsavers { get => skillsavers; set => skillsavers = value; }
    public SkillSaver SkSlot1 { get => skSlot1; set => skSlot1 = value; }
    public SkillSaver SkSlot2 { get => skSlot2; set => skSlot2 = value; }
    public SkillSaver SkSlot3 { get => skSlot3; set => skSlot3 = value; }
    public SkillSaver SkSlot4 { get => skSlot4; set => skSlot4 = value; }
    public RelicData RelSlot1 { get => relSlot1; set => relSlot1 = value; }
    public RelicData RelSlot2 { get => relSlot2; set => relSlot2 = value; }
    public RelicData RelSlot3 { get => relSlot3; set => relSlot3 = value; }
    public RelicData RelSlot4 { get => relSlot4; set => relSlot4 = value; }

    //public Vector3 Spawn { get => spawn; set => spawn = value; }

    public Game(List<SkillSaver>skillsList,Stats stats) {
        //Spawn = GameController.GetGameController().Spawn.transform.position;
        // Debug.Log(Position[0].ToString());
        orbCount = GameManager.GetManager().OrbAmt;
        Skillsavers = skillsList;
        items = Inventory.Items;
        if(GameManager.GetManager().Triangleslot !=null)
            SkSlot1 = new SkillSaver(GameManager.GetManager().Triangleslot.SkillName, GameManager.GetManager().Triangleslot.Unlocked);
        if (GameManager.GetManager().Squareslot != null)
            SkSlot2 = new SkillSaver(GameManager.GetManager().Squareslot.SkillName, GameManager.GetManager().Squareslot.Unlocked);
        if (GameManager.GetManager().Circleslot != null)
            SkSlot3 = new SkillSaver(GameManager.GetManager().Circleslot.SkillName, GameManager.GetManager().Circleslot.Unlocked);
        if (GameManager.GetManager().Xslot != null)
            SkSlot4 = new SkillSaver(GameManager.GetManager().Xslot.SkillName, GameManager.GetManager().Xslot.Unlocked);
        RelSlot1 = GameManager.GetManager().Slot1;
        RelSlot2 = GameManager.GetManager().Slot2;
        RelSlot3 = GameManager.GetManager().Slot3;
        RelSlot4 = GameManager.GetManager().Slot4;
        Stats = stats;
        Debug.Log("Game was saved");
    }
}
