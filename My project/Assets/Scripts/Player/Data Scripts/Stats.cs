using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
[System.Serializable]
public class Element {
    public Element() {
        parameters.Level = 1;
        parameters.Exp = 0;
        parameters.RequiredExp = 0;
    }
    public ElementParameters parameters;
    public int GetRequiredExp() { 
    return (int)(8 * Mathf.Pow(parameters.Level, 3));
    }
    public int GetCurrentExp() {
        return parameters.Exp-(int)(8 * Mathf.Pow(parameters.Level-1, 3));
    }
}
[System.Serializable]
public struct ElementParameters {
       int level;
       int exp;
       int requiredExp;
        
        public int Level { get => level; set => level = value; }
        public int Exp { get => exp; set => exp = value; }
        public int RequiredExp { get => requiredExp; set => requiredExp = value; }       
    }
[System.Serializable]
public class Stats
{
    //Variables
    private int health;
    private int attack;
    private int defense;
    private int stamina;
    private int staminaLeft;

    private int ultMeter;
    private int mp;
    private float speed;
    private int healthLeft;

    private int mpLeft;

    private int baseAttack;
    private int baseDefense;
    private int baseMp;
    private int baseHealth;

    private float attackBoost;
    private float defenseBoost;
    private int mpBoost;
    private int healthBoost;

    private byte kryllLevel;

    private int abilitypoints;
    private int transformationMod;
    
    private Element baseAura;
    private Element dark;
    private Element light;
    private Element fire;
    private Element ice;
    private Element electric;
    //Events
    public static event UnityAction onHealthChange;
    public static event UnityAction onStaminaLeft;
    public static event UnityAction onLevelUp;
    public static event UnityAction onShowingStats;
    public static event UnityAction onBaseStatsUpdate;
    public static event UnityAction onObjectiveComplete;
    public static event UnityAction<int> onPowerlv;
    public static event UnityAction sendSpeed;
    public static event UnityAction<int> onOrbGain;
    public static event UnityAction<int> sendMp;
    public static event UnityAction<int> updateMP;
    public static event UnityAction increase;
    public static event UnityAction decrease;
    public static event UnityAction updateUlt;
    //Properties
    #region Getters and Setters
    public int Health { get { return health; } set { health = Mathf.Max(0, value); } }
    public int HealthLeft { get { return healthLeft; } set { healthLeft = Mathf.Clamp(value, 0, health); CalculateStatsOutput(); if (onHealthChange != null) { onHealthChange(); } } }
    //public int MPLeft { get { return MpLeft; } set { MpLeft = Mathf.Clamp(value, 0, mp); CalculateStatsOutput(); if (onMPLeft != null) { onMPLeft(); } } }

    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int MP { get { return mp; } set { mp = value; if(sendMp!=null) sendMp(mp); } }//
    public float Speed { get { return speed; } set { speed = value; sendSpeed.Invoke(); } }
    public int BaseAttack { get => baseAttack; set { baseAttack = Mathf.Clamp(value, 0, 300); CalculateStatsOutput(); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseDefense { get => baseDefense; set { baseDefense = Mathf.Clamp(value, 0, 300); CalculateStatsOutput(); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseMp { get => baseMp; set { baseMp = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public int BaseHealth { get => baseHealth; set { baseHealth = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); } }
    public float AttackBoost { get => attackBoost; set { attackBoost = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); CalculateStatsOutput(); } }
    public float DefenseBoost { get => defenseBoost; set { defenseBoost = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); CalculateStatsOutput(); } }
    public int MpBoost { get => mpBoost; set { mpBoost = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); CalculateStatsOutput(); } }
    public int HealthBoost { get => healthBoost; set { healthBoost = Mathf.Clamp(value, 0, 300); if (onBaseStatsUpdate != null) onBaseStatsUpdate(); CalculateStatsOutput(); } }
    public int Abilitypoints { get => abilitypoints; set { abilitypoints = value; if (onBaseStatsUpdate != null) onBaseStatsUpdate(); if(onOrbGain!=null) onOrbGain(abilitypoints); } }
    public byte KryllLevel { get => kryllLevel; set => kryllLevel = value; }
    public int MPLeft { get => mpLeft; set { mpLeft = Mathf.Clamp(value, 0, mp); if(updateMP!=null) updateMP(MPLeft);
            } }
    public int Stamina { get => stamina; set => stamina = value; }
    //public int StaminaLeft { get => staminaLeft; set { staminaLeft = Mathf.Clamp(value, 0, 100); if(onStaminaLeft!=null) onStaminaLeft(); } }
    public int TransformationMod { get => transformationMod; set { transformationMod = Mathf.Clamp(value, 1, 100); CalculateStatsOutput(); } }
    public Element BaseAura { get => baseAura; set => baseAura = value; }
    public Element Dark { get => dark; set => dark = value; }
    public Element Light { get => light; set => light = value; }
    public Element Fire { get => fire; set => fire = value; }
    public Element Ice { get => ice; set => ice = value; }
    public Element Electric { get => electric; set => electric = value; }
    public int UltMeter { get => ultMeter; set { ultMeter = Mathf.Clamp(value, 0, 100); if (updateUlt != null) updateUlt(); } }
    #endregion
    public void DisplayAbilities() {
        if (onShowingStats != null) {
            onShowingStats();
        }
    }
    
    public void Start() {
        SetStats();
        BaseAura = new Element();
        Dark = new Element();
        Light = new Element();
        Fire = new Element();
        Ice = new Element();
        Electric = new Element();
        // Player.weaponSwitch += SetStats;
        //PerfectGuardBox.sendAmt += ChangeMpLeft;
        //PlayerInputs.transformed += OnTransformation;
        //SkillTreeNode.sendOrbs += AdjustOrbs;
        //Enemy.sendOrbs += AdjustOrbs;
        //PerfectGuardBox.sendAmt += AdjustMp;
        //GuardBox.sendAmt += AdjustMp;
        ////HealRelic.heal += Heal;
        //EnemyFireball.dmg += OutsideDamage;
        //EnemyHitBox.sendDmg += CalculateDamage;
        //PlayerAnimationEvents.transform += OnTransformation;
        if (onHealthChange != null) {
            onHealthChange();
        }
        if (onLevelUp != null) { onLevelUp(); }
    }
    private void UpdateUi() {
        if (onHealthChange != null) {
            onHealthChange();
        }

        if (onLevelUp != null) { onLevelUp(); }
    }
    private void SetStats() {
        // + mpBoost
        TransformationMod = 1;
        //Stamina = 99;
        //StaminaLeft = Stamina;
        baseHealth = 120;
        healthLeft = baseHealth;
        baseMp = 90;
        Health = baseHealth;// + healthBoost
        MP = baseMp+MpBoost;
        MPLeft = 0;
        BaseAttack = 10;
        BaseDefense = 5;
        UltMeter = 0;
        if (onHealthChange != null) {
            onHealthChange();
        }
        
        //onMPLeft.Invoke();
        //CalculateStatsOutput();

    }
    //private void ChangeMpLeft(int amt) => MPLeft += amt;
    private void CalculateStatsOutput() {
        //calculated everytime health or Mp is changed.
        /*Attack=(HealthLeft/Health+mpLeft)+baseAttack;
        Defense=(HealthLeft/Health+mpLeft)+baseDefense;
        onPowerlv.Invoke((HealthLeft / Health + mpLeft) * (baseDefense+baseAttack));*/
       // Debug.Log("Attack" + Attack);
        Attack = (int)(BaseAttack * (1 + AttackBoost) * transformationMod);
        Defense =(int)(BaseDefense * (1 + DefenseBoost * transformationMod));
        MP = baseMp + MpBoost;
        Health =baseHealth+ HealthBoost;
    }
    private void AddToAttackBoost() {
        //Upgrading Attacks on Attack boost affect here
        //
        CalculateStatsOutput();
    }
    private void AddToDefenseBoost() {
        //Upgrading Defense boost affect here
        //
        CalculateStatsOutput();
    }
    private void OnTransformation(bool val) {
        if (val) {
            AttackBoost = BaseAttack;
        }
        else {
            AttackBoost = 0;
        }
        //An Mp boost should be given here which would contribute to an attack otput boost
        //but also drains Mp and stamina the longer its held.
        //CalculateStatsOutput();
    }
    private void AdjustOrbs(int val) {
        Abilitypoints += val;
    }
    public void IncreaseHealth() {
        Health += 10;
        //Debug.Log(Health);
    }
    public void IncreaseMPSlowly() {
        MPLeft += (mp / 100);
        increase.Invoke();
    }
    public void AdjustMp(int amt) {
        MPLeft += amt;
    }
    public void DecreaseMPSlowly() {
        MPLeft -= (mp / 100);
        decrease.Invoke();
    }
    private void AddToStats() {
        if (mpLeft / (mp / 2) > 1)
            AttackBoost = mpLeft / (mp / 2);
    }
    private void Heal() {
        HealthLeft += (int)(Health * 0.2);
    }
    private void OutsideDamage(int val) {
        HealthLeft -= val;
    }
    private void CalculateDamage(int val) {
        HealthLeft -= Mathf.Clamp(val-Defense,1,999);
    }
}
