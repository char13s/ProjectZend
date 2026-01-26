using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RoomDoors : Interactable
{
    [SerializeField] private int level;
    public static event UnityAction<int> sendLevel;
    public override void Interact() {
        print("Interaction acted.");
        //Que Level Manager to change scenes
        if(sendLevel!=null)
           sendLevel(level);
    }
}
