using System.Collections;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(CharacterController))]
public class NewZend : MonoBehaviour
{
    private static NewZend instance;
    //private CinemachineStateDrivenCamera normalCam;
    //PlayerLockOn plockOn;
    KidZendEffects effects;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource soundEffects;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hurtbox;
    [SerializeField] private GameObject teleEffect;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject wallJumpBox;
    [SerializeField] private GameObject aimminPoint;
    [SerializeField] private GameObject summonBlastPoint;
    [SerializeField] private GameObject sword;
    private GameObject aimCam;
    [SerializeField] private GameObject target;
    public static event UnityAction<bool> lockon;
    public static event UnityAction playerEnabled;
    public static event UnityAction playerStart;
    public static event UnityAction<GameObject> sendAimCam;
    public static event UnityAction onPlayerDeath;
    public static event UnityAction endSense;
    public static event UnityAction findEnemy;
    CharacterController charCon;
    Vector3 displacement;
    Vector3 direction;
    Vector3 move;
    private GameObject mainCam;
    private GameObject eventCam;
    private GameObject gameplayCam;
    bool moving;
    bool attacking;
    bool playingSkill;
    bool teleport;
    bool lockedon;
    bool aimming;
    float rotationSpeed = 15;
    bool skillButton;
    int combatAnimations;
    bool hasTarget;
    bool dead;
    bool draining;
    bool blocking;
    bool grounded;
    bool zendlocked;
    bool falling;
    bool surfing;
    bool timelining;
    bool swordSkill;
    bool ultimate;
    bool hurt;
    int healAmt;
    int healTimer;
    private float sightMpDrain;
    Quaternion qTo;
    Vector3 speed;
    float moveSpeed = 10;
    [SerializeField] private float gravity;
    private Coroutine staminaRecovery;
    //private ExperimentalInputs inputs;
    internal Stats stats = new Stats();
    [SerializeField] Skill xslot;
    [SerializeField] Skill squareslot;
    [SerializeField] Skill triangleslot;
    [SerializeField] Skill circleslot;
    public Vector3 Displacement { get => displacement; set => displacement = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public bool Moving { get => moving; set { moving = value; Anim.SetBool("Moving", moving); } }
    public Animator Anim { get => anim; set => anim = value; }
    public CharacterController CharCon { get => charCon; set => charCon = value; }
    public bool Attacking { get => attacking; set => attacking = value; }
    public float MoveSpeed { get => moveSpeed; set { moveSpeed = value; } }
    public bool Lockedon { get => lockedon; set { lockedon = value; Anim.SetBool("LockedOn", value); if (lockon != null) { lockon.Invoke(value); } AimCamControl(value); } }
    public bool Aimming { get => aimming; set { aimming = value; lockon.Invoke(value); } }
    public GameObject Bow { get => bow; set { bow = value; } }
    public bool SkillPower { get => skillButton; set { skillButton = value; Anim.SetBool("AuraPower", value); } }
    public int CombatAnimations { get => combatAnimations; set => combatAnimations = value; }
    public bool HasTarget { get => hasTarget; set => hasTarget = value; }
    public bool Draining { get => draining; set { draining = value; StartCoroutine(DrainMp()); } }
    public bool Blocking { get => blocking; set => blocking = value; }
    public GameObject Target { get => target; set => target = value; }
    public bool Grounded { get => grounded; set => grounded = value; }
    //public PlayerLockOn PlockOn { get => plockOn; set => plockOn = value; }
    public bool Zendlocked { get => zendlocked; set => zendlocked = value; }
    public bool Falling { get => falling; set => falling = value; }
    public bool Surfing { get => surfing; set => surfing = value; }
    public Vector3 Move { get => move; set => move = value; }
    public bool Timelining { get => timelining; set => timelining = value; }
    public KidZendEffects Effects { get => effects; set => effects = value; }
    public GameObject SummonBlastPoint { get => summonBlastPoint; set => summonBlastPoint = value; }
    public bool SwordSkill { get => swordSkill; set => swordSkill = value; }
    public GameObject AimminPoint { get => aimminPoint; set => aimminPoint = value; }
    public Skill Xslot { get => xslot; set => xslot = value; }
    public Skill Squareslot { get => squareslot; set => squareslot = value; }
    public Skill Triangleslot { get => triangleslot; set => triangleslot = value; }
    public Skill Circleslot { get => circleslot; set => circleslot = value; }
    public bool Ultimate { get => ultimate; set => ultimate = value; }
    public bool PlayingSkill { get => playingSkill; set => playingSkill = value; }
    public bool Hurt { get => hurt; set => hurt = value; }
    public GameObject MainCam { get => mainCam; set => mainCam = value; }
    public GameObject Body { get => body; set => body = value; }
    //public ExperimentalInputs Inputs { get => inputs; set => inputs = value; }
    public AudioSource SoundEffects { get => soundEffects; set => soundEffects = value; }
    public GameObject EventCam { get => eventCam; set => eventCam = value; }
    public GameObject GameplayCam { get => gameplayCam; set => gameplayCam = value; }

    //ExperimentalAnimEvents animsevents;
    public static NewZend GetPlayer() => instance;
    // Start is called before the first frame update
    private void OnEnable() {
        //sendAimCam.Invoke(aimCam);
        Stats.updateMP += MpToAnim;
        //AimTarget.sendTarget += SetAimminPoint;
        //WallJumpTrigger.wallJump += WallJump;
        //DodgeBox.dodge += Dodge;
        //TeleportBehavior.teleport += Vanish;
        //animsevents = GetComponentInChildren<ExperimentalAnimEvents>();
        //ExperimentalInputs.skillUp += Skills;
        //Stats.onHealthChange += CheckDead;
        //LockStateMachine.lockon += Lockon;
        //SkillButton.sendMpCost += AlterMp;
        //HitBox.onEnemyHit += GainMp;
        //DamageBuffNode.buffDmg += MoreAttack;
        //MpBuffNode.buffMp += MoreMp;
        //HealthBuffNode.buffHealth += MoreHealth;
        //DefenseBuffNode.buffDef += MoreDefense;
        //MpSaverSight.mpUsageReduction += UpgradeDrainage;
        //TimelineManager.timelining += TimeliningControl;
        //GameManager.sendBoost += AssignBoost;
        //Guard.guardOff += GuardControl;
        //GameManager.healBoost += SetHealAmounts;
        //SwordSkillState.staminaSap += TakeStamina;
        //ReactionRange.zoom += Dodge;
        //HurtBoxControl.dodge += Dodge;
        //HealCastSkillTimeline.sendHeal += AlterHealth;
        //PowerUpSwordTimelineSkill.sendBuff += TempBuff;
        //PowerUpSwordTimelineSkill.sendDeBuff += TempDeBuff;
        //DemonSwordSummon.updateSword += DemonSwordControl;
    }
    private void OnDisable() {
        Stats.updateMP -= MpToAnim;
        //AimTarget.sendTarget -= SetAimminPoint;
        //WallJumpTrigger.wallJump -= WallJump;
        //DodgeBox.dodge -= Dodge;
        //TeleportBehavior.teleport -= Vanish;
        //ExperimentalInputs.skillUp -= Skills;
        //Stats.onHealthChange -= CheckDead;
        //LockStateMachine.lockon -= Lockon;
        //SkillButton.sendMpCost -= AlterMp;
        //HitBox.onEnemyHit -= GainMp;
        //DamageBuffNode.buffDmg -= MoreAttack;
        //MpBuffNode.buffMp -= MoreMp;
        //HealthBuffNode.buffHealth -= MoreHealth;
        //DefenseBuffNode.buffDef -= MoreDefense;
        //MpSaverSight.mpUsageReduction -= UpgradeDrainage;
        //TimelineManager.timelining -= TimeliningControl;
        //Guard.guardOff -= GuardControl;
        //GameManager.sendBoost -= AssignBoost;
        //GameManager.healBoost -= SetHealAmounts;
        //SwordSkillState.staminaSap -= TakeStamina;
        //ReactionRange.zoom -= Dodge;
        //HurtBoxControl.dodge -= Dodge;
        //HealCastSkillTimeline.sendHeal -= AlterHealth;
        //PowerUpSwordTimelineSkill.sendBuff -= TempBuff;
        //PowerUpSwordTimelineSkill.sendDeBuff -= TempDeBuff;
        //DemonSwordSummon.updateSword -= DemonSwordControl;
    }
    private void Awake() {

        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        if (playerEnabled != null)
            playerEnabled();
        sightMpDrain = 0.2f;
        MainCam = GameObject.FindGameObjectWithTag("Camera");
        EventCam= GameObject.FindGameObjectWithTag("MainCamera");
        gameplayCam = GameObject.FindGameObjectWithTag("GameplayCam");
        Anim = GetComponent<Animator>();
        //aimCam = GameObject.Find("AimCam 2");
        CharCon = GetComponent<CharacterController>();
        //plockOn = GetComponent<PlayerLockOn>();
        effects = GetComponent<KidZendEffects>();
        //inputs = GetComponent<ExperimentalInputs>();
    }
    // Start is called before the first frame update
    void Start() {

        //MainCam = GameObject.FindWithTag("Camera");
        //Camera.main.gameObject;
        dead = false;
        if (playerStart != null)
            playerStart();
        stats.Start();
        moveSpeed = 10;
        StartCoroutine(HealthRegen());
        staminaRecovery = StartCoroutine(GainStamina());
        //effects.SwordAuraControl(true);
    }
    public void SwitchLayers(bool val) {
        if (val) {
            gameObject.layer = 13;
        }
        else {
            gameObject.layer = 19;
        }
    }
    private void FixedUpdate() {
        //if (!CharCon.isGrounded) {
        //    wallJumpBox.SetActive(true);
        //}
        //else {
        //    wallJumpBox.SetActive(false);
        //}
    }
    private void DemonSwordControl(bool val) {
        print("sword");
        //sword.SetActive(val);
    }
    private void SetHealAmounts(int amt, int timer) {
        healAmt = amt;
        healTimer = timer;
    }
    private void MpToAnim(int val) {
        Anim.SetInteger("Mp", val);
    }
    private void WallJump() {
        //trigger animation
        //give a extra jump
    }
    private void AimCamControl(bool val) {
        //aimCam.SetActive(val);
    }
    private void Dodge(bool val) {
        Anim.SetTrigger("Teleport");
        hurtbox.SetActive(val);
    }
    private void AssignBoost(float att, float def, int mp, int health) {
        stats.AttackBoost = att;
        stats.DefenseBoost = def;
        stats.MpBoost = mp;
        // stats.HealthBoost += health;

    }

    private void Vanish(bool val) {

        Body.SetActive(val);
        Instantiate(teleEffect, transform.position, transform.rotation);
        if (val) {
            teleport = false;
        }
        else {
            //animsevents.BodyEffect();
            teleport = true;
        }
    }
    private void Skills(bool val) {
        SkillPower = val;
        
        Effects.SwordAura.SetActive(val);
    }
    private void SetAimminPoint(GameObject target) {
        AimminPoint = target;
    }
    private void CheckDead() {
        if (stats.HealthLeft <= 0 && !dead) {
            Anim.SetTrigger("Dead");
            if (onPlayerDeath != null)
                onPlayerDeath();
            dead = true;
        }
    }
    private void Lockon(bool val) {
        Zendlocked = val;
    }
    #region Stats altering
    private void GainMp() {
        AlterMp(1);
    }
    private void AlterMp(int mpCost) {
        stats.MPLeft += mpCost;
    }
    private void AlterHealth() {
        stats.HealthLeft += 20;
    }
    private void TempBuff() {
        stats.AttackBoost += 2;
        effects.SwordAuraControl(true);
        effects.AuraFistL.SetActive(false);
        StartCoroutine(waitToUnSetPowerUp());
    }
    private void TempDeBuff() {
        stats.AttackBoost -= 2;
        print("Debuff");
        effects.SwordAuraControl(false);
    }

    private void MoreHealth(int buff) {
        stats.Health += buff;
    }
    private void MoreMp(int buff) {
        stats.MP += buff;
    }
    private void MoreDefense(int buff) {
        stats.BaseAttack += buff;
    }
    private void MoreAttack(int buff) {
        stats.BaseDefense += buff;
    }
    private void UpgradeDrainage(int reduction) {
        sightMpDrain -= reduction;//this is wrong
    }
    private void AttackOn() {
        Attacking = true;
    }
    private void AttackOff() {
        Attacking = false;
    }
    private void TimeliningControl(bool val) {
        timelining = val;
        Anim.applyRootMotion = true;
    }
    private void GuardControl() {
        hurtbox.SetActive(true);
    }
    public void StartStaminaRegain() {
        if (staminaRecovery != null) {
            //StopCoroutine(staminaRecovery);
        }
        staminaRecovery = StartCoroutine(GainStamina());
    }
    private void TakeStamina() {

    }
    public static event UnityAction endCharging;
    private IEnumerator DrainMp() {
        //WaitForSecondsRealtime
        YieldInstruction wait = new WaitForSeconds(sightMpDrain);//Change 1 to a variable
        yield return new WaitForSeconds(1);
        while (isActiveAndEnabled & Draining) {
            yield return wait;
            if (stats.UltMeter < 100) {
                if (stats.MPLeft > 0) {
                    stats.MPLeft--;
                    stats.UltMeter++;
                    // if(endSense!=null)
                    //     endSense();
                }
            }
            else {
                Draining = false;
                if (endCharging != null) {
                    endCharging();
                }
            }

        }
    }
    private IEnumerator GainStamina() {
        //WaitForSecondsRealtime
        YieldInstruction wait = new WaitForSeconds(0.2f);//Change 1 to a variable
        while (isActiveAndEnabled) {
            yield return wait;

        }
    }
    private IEnumerator HealthRegen() {
        //WaitForSecondsRealtime
        YieldInstruction wait = new WaitForSeconds(healTimer);//Change 1 to a variable
        while (isActiveAndEnabled) {
            yield return wait;
            stats.HealthLeft += healAmt;
        }
    }
    IEnumerator waitToUnSetPowerUp() {
        YieldInstruction wait = new WaitForSeconds(5);
        yield return wait;
        TempDeBuff();
    }
    #endregion

}
