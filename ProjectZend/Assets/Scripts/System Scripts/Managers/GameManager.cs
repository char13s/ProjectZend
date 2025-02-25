using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private bool pause;
    private bool deviceLost;
    [SerializeField]private bool unpause;
    public static event UnityAction<bool> pauseScreen;
    public static event UnityAction<int> switchMap;
    public static event UnityAction gameOver;
    public static event UnityAction<int> sendToLevelManager;
    public static event UnityAction<int> sendOrbAmount;
    public static event UnityAction<float, float, int, int> sendBoost;
    public static event UnityAction<int, int> healBoost;
    public static event UnityAction<Skill, int> setSetters;
    public static event UnityAction<RelicData, int> setRelicSlots;
    public static event UnityAction findButtonToSelect;
    public static event UnityAction<int> setscheme;
    public static event UnityAction<bool> zoom;

    [SerializeField] private Skill[] skillsList;
    private List<RelicData> items = new List<RelicData>();
    [SerializeField]private Skill xslot;
    [SerializeField]private Skill triangleslot;
    [SerializeField]private Skill squareslot;
    [SerializeField]private Skill circleslot;

    private RelicData slot1;
    private RelicData slot2;
    private RelicData slot3;
    private RelicData slot4;

    private SkillButton xbutton;
    private SkillButton squarebutton;
    private SkillButton circlebutton;
    private SkillButton trianglebutton;

    private bool screenShake;
    private bool viberate;
    private RelicData holdRelic;

    private float attackBoost;
    private float defenseBoost;
    private int healthBoost;
    private int mpBoost;

    private int healAmount;
    private int healRate;

    private int orbAmt;
    private int lastLevel;
    private NewZend zend;
    private PlayerInput input;
    private Stats stats;
    private Game game;
    bool isWaiting;
    public enum ControllerState {KeyBoardAndMouse ,  Controller}
    private ControllerState currentController;
    public enum GameState { Paused, PlayMode, Dialogue }
    private GameState currentState;
    public GameState CurrentState { get => currentState; set { currentState = value; StateMappings(); if (gameModeChange != null) { gameModeChange(); } } }
    public int OrbAmt { get => orbAmt; set { orbAmt = value; if (sendOrbAmount != null) { sendOrbAmount(value); } } }
    public NewZend Zend { get => zend; set => zend = value; }
    public int LastLevel { get => lastLevel; set => lastLevel = value; }
    public int MpBoost { get => mpBoost; set => mpBoost = Mathf.Clamp(value, 0, 70); }
    public RelicData HoldRelic { get => holdRelic; set => holdRelic = value; }
    public SkillButton Xbutton { get => xbutton; set => xbutton = value; }
    public SkillButton Squarebutton { get => squarebutton; set => squarebutton = value; }
    public SkillButton Circlebutton { get => circlebutton; set => circlebutton = value; }
    public SkillButton Trianglebutton { get => trianglebutton; set => trianglebutton = value; }
    public Skill Xslot { get => xslot; set => xslot = value; }
    public Skill Triangleslot { get => triangleslot; set => triangleslot = value; }
    public Skill Squareslot { get => squareslot; set => squareslot = value; }
    public Skill Circleslot { get => circleslot; set => circleslot = value; }
    /*public Skill Xslot1 { get => xslot; set => xslot = value; }
    public Skill Triangleslot1 { get => triangleslot; set => triangleslot = value; }
    public Skill Squareslot1 { get => squareslot; set => squareslot = value; }
    public Skill Circleslot1 { get => circleslot; set => circleslot = value; }*/
    public RelicData Slot1 { get => slot1; set => slot1 = value; }
    public RelicData Slot2 { get => slot2; set => slot2 = value; }
    public RelicData Slot3 { get => slot3; set => slot3 = value; }
    public RelicData Slot4 { get => slot4; set => slot4 = value; }
    //public SwordSkills SwordSkillX { get => swordSkillX; set => swordSkillX = value; }
    //public SwordSkills SwordSkillTri { get => swordSkillTri; set => swordSkillTri = value; }
    //public SwordSkills SwordSkillCir { get => swordSkillCir; set => swordSkillCir = value; }
    //public SwordSkills SwordSkillSqu { get => swordSkillSqu; set => swordSkillSqu = value; }
    public Stats Stats { get => stats; set => stats = value; }
    public Game Game { get => game; set => game = value; }

    public static GameManager GetManager() => instance;
    // Start is called before the first frame update
    private void Awake() {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
            CurrentState = GameState.Paused;
        //SaveLoad.DeleteFile();
        //if() Set an if to "If a save file can be found for the Game"
    }
    private void OnEnable() {
        //Subscribe();
    }
    private void OnDisable() {
        //Unsubscribe();
    }
    void Start() {
        //LoadGame();
        currentController = ControllerState.KeyBoardAndMouse;
        InputSystem.onDeviceChange += ControllerChange;
        IntialControllerCheck();
    }
    /*void Subscribe() {
        input = GetComponent<PlayerInput>();
        DialogueTrigger.gameMode += DialogueTriggered;
        PlayerInputs.pause += PauseGameControls;
        ExperimentalInputs.pause += PauseGameControls;

        //Stats.onOrbGain += Collect;
        PlayerInputs.playerEnabled += StateMappings;
        NewZend.onPlayerDeath += HandlePlayerDeath;
        CheckPoint.onCheckPoint += SetCheck;
        LoadingCanvas.loadPlayer += RespawnPlayer;
        ExperimentalInputs.assignSkills += AssignPlayer;
        SkillHolder.sendSkill += SetSkills;
        Enemy.sendOrbs += Collect;
        StatsEnhancementData.sendBoost += StatsBoost;
        EnergyEnhancerData.barsToAdd += RecieveEnergyBoost;
        LevelManager.unpause += PauseGameControls;
        LevelManager.levelFinished += FinishedLoad;
        LevelManager.sendToMain += MainMenuUp;
        EquipmentCanvas.requestAssign += AssignSlots;
        RelicSlot.sendRelicData += SetRelic;
        RewardSlot.bought += SpendObrs;
        RewardCanvas.pause += PauseGameControls;
        RewardSlot.sendRelic += SetTempRelic;
        RelicSlot.requestData += SetRelicslots;
        OrbCanvas.requestOrbs += NewLevelStarted;
        // BuyableSkill.unlockSkill += UnlockSkill;
        AuraTreeNode.unlockSkill += UnlockSkill;
        ExperimentalInputs.controlSense += KeyOrController;
        HealRelicData.heal += HealRelicSet;
        NewZend.playerStart += AssignPlayer;
        HitBox.zawarudo += ZaWarudo;
        AttackStates.vibe += BeginRumbling;
        HurtBox.vibe += BeginRumbling;
        HitBox.vibe += BeginRumbling;
        CounterHitCounter.vibe += BeginRumbling;
        ReactionRange.dodged += DodgeEffect;
        CameraSettingsMenu.setvibe += vibrationControl;
        SkillTreeNode.sendOrbs += Collect;
        SkillTreeNode.save += SaveGame;
        SaveSlot.load += SetGame;
        SkillSlotSetter.askForSkill += AssignSlots;
        SkillSlotSetter.sendSkill += SetSkills;
        NewZend.playerEnabled += PlayerLoaded;
        //input.onDeviceLost += ControllerDisabled;
    }
    void Unsubscribe() {
        DialogueTrigger.gameMode -= DialogueTriggered;
        PlayerInputs.pause -= PauseGameControls;
        ExperimentalInputs.pause -= PauseGameControls;
        LevelManager.unpause -= PauseGameControls;
        NewZend.playerEnabled -= PlayerLoaded;
        //Stats.onOrbGain -= Collect;
        PlayerInputs.playerEnabled -= StateMappings;
        NewZend.onPlayerDeath -= HandlePlayerDeath;
        CheckPoint.onCheckPoint -= SetCheck;
        LoadingCanvas.loadPlayer -= RespawnPlayer;
        ExperimentalInputs.assignSkills -= AssignPlayer;
        SkillHolder.sendSkill -= SetSkills;
        Enemy.sendOrbs -= Collect;
        LevelManager.levelFinished -= FinishedLoad;
        LevelManager.sendToMain -= MainMenuUp;
        StatsEnhancementData.sendBoost -= StatsBoost;
        EnergyEnhancerData.barsToAdd -= RecieveEnergyBoost;
        EquipmentCanvas.requestAssign -= AssignSlots;
        RelicSlot.sendRelicData -= SetRelic;
        RewardSlot.bought -= SpendObrs;
        RewardCanvas.pause -= PauseGameControls;
        RelicSlot.requestData -= SetRelicslots;
        RewardSlot.sendRelic -= SetTempRelic;
        OrbCanvas.requestOrbs -= NewLevelStarted;
        // BuyableSkill.unlockSkill -= UnlockSkill;
        ExperimentalInputs.controlSense -= KeyOrController;
        HealRelicData.heal -= HealRelicSet;
        NewZend.playerStart -= AssignPlayer;
        HitBox.zawarudo -= ZaWarudo;
        AttackStates.vibe -= BeginRumbling;
        HurtBox.vibe -= BeginRumbling;
        HitBox.vibe -= BeginRumbling;
        CounterHitCounter.vibe -= BeginRumbling;
        ReactionRange.dodged -= DodgeEffect;
        CameraSettingsMenu.setvibe -= vibrationControl;
        SkillTreeNode.sendOrbs -= Collect;
        SkillTreeNode.save -= SaveGame;
        SaveSlot.load -= SetGame;
        SkillSlotSetter.askForSkill -= AssignSlots;
        SkillSlotSetter.sendSkill -= SetSkills;
    }*/
    private void vibrationControl(bool val) {
        viberate = val;
    }
    private void ScreenShakeControl(bool val) {
        screenShake = val;
    }
    private void OnControllerUsed() {
        KeyOrController(0);
        if (deviceLost) {
            UnpauseGame();
        }
    }
    private void OnKeyBoardUsed() {
        KeyOrController(1);
        Debug.Log("keys");
        if (deviceLost) {
            UnpauseGame();
            deviceLost = false;
        }
    }
    private void OnMouseUsed() {
        KeyOrController(1);
        if (deviceLost) {
            UnpauseGame();
            deviceLost = false;
        }
    }
    private void ControllerDisabled() {
        //PauseGame();
        deviceLost = true;
        //needs to throw up a canvas telling players their controller is lost and to press to continue the game
        print("COntroller was lost!");
    }
    public static event UnityAction<DeviceType, InputDeviceChange> devicechange;
    private void WhatTypeOfController(InputDevice device, InputDeviceChange change) {

    }
    private void ControllerChange(InputDevice device, InputDeviceChange change) {
        switch (change) {
            case InputDeviceChange.Added:
                // New Device.
                if (device.description.manufacturer == "Sony Interactive Entertainment") {
                    //Sets UI scheme
                    Debug.Log("Playstation Controller Detected");
                    if (setscheme != null)
                        setscheme(0);
                }
                else if (device.description.manufacturer != "Sony Interactive Entertainment") {
                    Debug.Log("Xbox Controller Detected");
                    if (setscheme != null)
                        setscheme(1);
                }
                break;
            case InputDeviceChange.Disconnected:
                // Device got unplugged.
                ControllerDisabled();
                break;
            //case InputDeviceChange.Connected:
            // Plugged back in.
            // break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
        //UnpauseGame();
    }
    private void IntialControllerCheck() {

        //Disables all devices currently read by InputSystem
        /*for (int rep = 0; rep < InputSystem.devices.Count - 1; rep++) {
            //InputSystem.RemoveDevice(InputSystem.devices[rep]);
            InputSystem.DisableDevice(InputSystem.devices[rep]);
        }*/

        if (InputSystem.devices.Count>0)
            if (InputSystem.devices[0] == null) return;

        //Checks the first slot of the InputSystem devices list for controller type
        if (InputSystem.devices[0].description.manufacturer == "Sony Interactive Entertainment") {
            //Sets UI scheme to PS

            if (setscheme != null)
                setscheme(0);
        }
        else {
            //Sets UI scheme to XB

            if (setscheme != null)
                setscheme(1);
        }
        
    }
    private void KeyOrController(int val) {
        if (val == 0 && currentController == ControllerState.KeyBoardAndMouse) {//if controller 
            //has to send an even to all UI control panels saying use controller based ui
            //send an event to event manager for getting what button to sync controls to
            currentController = ControllerState.Controller;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (findButtonToSelect != null) {
                findButtonToSelect();
            }
        }
        else if (val == 1 && currentController == ControllerState.Controller) {//if mouse
            currentController = ControllerState.KeyBoardAndMouse;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void BeginRumbling(float low, float high) {
        if (viberate) {
            Gamepad.current.SetMotorSpeeds(low, high);
            StartCoroutine(waitToStopRumble());
        }
    }
    IEnumerator waitToStopRumble() {
        YieldInstruction wait = new WaitForSeconds(0.2f);
        yield return wait;
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
    private void AssignPlayer() {
        Zend = NewZend.GetPlayer();
        //tempSave = new Game(Zend);
        //SetSkillsToZend();
        AssignBoost();
    }
    private void FinishedLoad(bool val) {
        if (pause) {
            pause = false;
            Time.timeScale = 1;
            //CurrentState = GameState.PlayMode;
        }
    }
    #region Hit Stopping
    private void ZaWarudo(float duration) {
        if (isWaiting)
            return;
        //Time.timeScale = 0;
        StartCoroutine(HitStop(duration));
    }
    private IEnumerator HitStop(float duration) {
        isWaiting = true;
        CustomYieldInstruction wait = new WaitForSecondsRealtime(duration);
        yield return wait;
        Time.timeScale = 1;
        isWaiting = false;
    }
    #endregion
    private void DodgeEffect() {
        Time.timeScale = 0.1f;
        zoom.Invoke(true);
        StartCoroutine(ResetTimeStop());
    }
    private IEnumerator ResetTimeStop() {
        //YieldInstruction wait;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        zoom.Invoke(false);
        print("Reset");
    }
    private void PauseGame() {
        //pause = true;
        Time.timeScale = 0;
        CurrentState = GameState.Paused;
        pause = true;
    }
    private void UnpauseGame() {
        pause = false;
        Time.timeScale = 1;
        CurrentState = GameState.PlayMode;
    }
    public static event UnityAction gameModeChange;
    public void PauseGameControls() {
        if (pause) {
            UnpauseGame();
        }
        else {
            PauseGame();
        }
        if (pauseScreen != null)
            pauseScreen(pause);
        if (gameModeChange != null) {
            gameModeChange();
        }
    }
    private void HandlePlayerDeath() {
        //kill player controls
        //tell level manager to send back to main menu
        if (switchMap != null)
            switchMap(99);
        //gameOver.Invoke();
    }
    private void DialogueTriggered() {
        CurrentState = GameState.Dialogue;
        Time.timeScale = 0;
    }
    private void GameStateControl(bool val) {
        if (val) {
            CurrentState = GameState.PlayMode;
        }
        else {
            CurrentState = GameState.Paused;
        }
    }
    void PlayerLoaded() {
        print("Player was loaded");
        CurrentState = GameState.PlayMode;
    }
    void StateMappings() {
        switch (currentState) {
            case GameState.PlayMode:
                if (switchMap != null)
                    switchMap(0);
                break;
            case GameState.Paused:
                if (switchMap != null)
                    switchMap(1);
                break;
            case GameState.Dialogue:
                if (switchMap != null)
                    switchMap(4);
                break;
        }
    }
    #region Data Assignment To player
    void SetSkillsToZend() {
        if (Triangleslot != null)
            Zend.Triangleslot = Triangleslot;
        if (Squareslot != null)
            Zend.Squareslot = Squareslot;
        if (Circleslot != null)
            Zend.Circleslot= Circleslot;
        if (Xslot != null)
            Zend.Xslot = Xslot;
    }
    private void AssignSlots() {
        print("Sent");
        if (setSetters != null)
            setSetters(Triangleslot, 0);
        if (setSetters != null)
            setSetters(Squareslot, 1);
        if (setSetters != null)
            setSetters(Circleslot, 2);
        if (setSetters != null)
            setSetters(Xslot, 3);
    }
    private void SetRelicslots() {
        if (setRelicSlots != null) {
            setRelicSlots(Slot1, 0);
        }
        if (setRelicSlots != null) {
            setRelicSlots(Slot2, 1);
        }
        if (setRelicSlots != null) {
            setRelicSlots(Slot3, 2);
        }
        if (setRelicSlots != null) {
            setRelicSlots(Slot4, 3);
        }
    }

    #region Stats buffs from passives
    private void AssignBoost() {
        if (sendBoost != null)
            sendBoost(attackBoost, defenseBoost, mpBoost, healthBoost);

        if (healBoost != null) {
            healBoost(healAmount, healRate);
        }
    }
    private void StatsBoost(float att, float def) {
        attackBoost += att;
        defenseBoost += def;
    }
    private void RecieveEnergyBoost(int bars) {
        MpBoost += bars * 10;
        print(mpBoost);
    }
    private void HealRelicSet(int amt, int rate) {
        healAmount = amt;
        healRate = rate;
    }
    #endregion
    #endregion
    #region Data collections
    void Setslots(int id, Skill skill) {
        switch (id) {
            case 0:
                //trianglebutton.SkillAssigned = skill;
                Triangleslot = skill;
                break;
            case 1:
                //squarebutton.SkillAssigned = skill;
                Squareslot = skill;
                break;
            case 2:
                //circlebutton.SkillAssigned = skill;
                Circleslot = skill;
                break;
            case 3:
                //Xbutton.SkillAssigned = skill;
                Xslot = skill;
                break;
        }
    }
    void SetSkills(Skill skill, int id) {
        switch (id) {
            case 0:
                //trianglebutton.SkillAssigned = skill;
                Triangleslot = skill;
                break;
            case 1:
                //squarebutton.SkillAssigned = skill;
                Squareslot = skill;
                break;
            case 2:
                //circlebutton.SkillAssigned = skill;
                Circleslot = skill;
                break;
            case 3:
                //Xbutton.SkillAssigned = skill;
                Xslot = skill;
                break;
        }
    }
   
    void SetRelic(RelicData relic, int id) {
        switch (id) {
            case 0:
                Slot1 = relic;
                if (relic != null)
                    relic.Use();
                break;
            case 1:
                Slot2 = relic;
                if (relic != null)
                    relic.Use();
                break;
            case 2:
                Slot3 = relic;
                if (relic != null)
                    relic.Use();
                break;
            case 3:
                Slot4 = relic;
                if (relic != null)
                    relic.Use();
                break;
        }
    }
    public void SpendObrs(int amt) {
        OrbAmt -= amt;
    }
    private void Collect(int amt, Elements element) {
        switch (element) {
            case Elements.Normal:
                break;
            case Elements.Dark:
                OrbAmt += amt;
                break;
            case Elements.Electric:
                break;
            case Elements.Fire:
                break;
            case Elements.Ice:
                break;
            case Elements.Light:
                break;
        }
    }
    private void SetTempRelic(RelicData relic) {
        HoldRelic = relic;
    }
    #endregion
    private void SetCheck(GameObject val) {
        //checkPoint = val;
        // tempSave.Stats = zend.stats;
    }
    private void RespawnPlayer() {
        //NewZend tempZ = Instantiate(zend, checkPoint.transform.position, Quaternion.identity);
        //tempZ.stats=tempSave.Stats;
    }
    public void SetGame(Game game) {
        Game = game;
        LoadGame();
    }
    private void UnlockSkill(Skill skill) {
        foreach (Skill skillInList in skillsList) {
            if (skill.SkillName == skillInList.SkillName) {
                skillInList.Unlocked = true;
                //OrbAmt -= cost;
            }
        }
    }
    public void BackToMain() {
        if (sendToLevelManager != null)
            sendToLevelManager(0);
    }
    private void NewLevelStarted() {
        if (sendOrbAmount != null)
            sendOrbAmount(orbAmt);
    }
    private void NewItems(List<RelicData> invent) {
        items = invent;
        SaveGame();
    }
    public void SaveGame() {
        //if (SaveLoad.DoesFileExist()) {
        List<SkillSaver> skills = ProcessSkills();
        SaveLoad.Save(skills,stats);
        //}
    }
    private void LoadGame() {
        LoadUpSkills();
        LoadItems();
        LoadUpPrevSlotInfo();
        OrbAmt = Game.OrbCount;
        Stats = Game.Stats;
        Stats.Start();
        //}
    }
    private List<SkillSaver> ProcessSkills() {
        List<SkillSaver> savers = new List<SkillSaver>();
        foreach (Skill skill in skillsList) {
            if (skill.Unlocked) {
                SkillSaver saver = new SkillSaver(skill.SkillName, skill.Unlocked);
                savers.Add(saver);
            }
        }
        return savers;
    }
    private void LoadUpSkills() {
        foreach (Skill skill in skillsList) {
            foreach (SkillSaver saver in Game.Skillsavers)
                if (skill.SkillName == saver.SkillName) {
                    skill.Unlocked = saver.UnlockedOrNot;
                }
        }
    }
    private Skill GetSkill(SkillSaver saver) {
        if (saver != null) {
            foreach (Skill skill in skillsList) {
                if (saver.SkillName == skill.SkillName) {
                    return skill;
                }
            }
        }
        return null;
    }
    public bool CheckSkill(Skill saver) {
        foreach (Skill skill in skillsList) {
            if (saver.SkillName == skill.SkillName) {
                return skill.Unlocked;
            }
        }

        return false;
    }
    private void LoadItems() {
        Inventory.Items = Game.Items;
    }
    private void LoadUpPrevSlotInfo() {
        Triangleslot = GetSkill(Game.SkSlot1);
        Squareslot = GetSkill(Game.SkSlot2);
        Circleslot = GetSkill(Game.SkSlot3);
        Xslot = GetSkill(Game.SkSlot4);

        Slot1 = Game.RelSlot1;
        Slot2 = Game.RelSlot2;
        Slot3 = Game.RelSlot3;
        Slot4 = Game.RelSlot4;
    }
    private void MainMenuUp() {

        //set skillslots, relic slots and store data

    }
    private void OnApplicationPause(bool pause) {
        //PauseGame();
    }
    public void QuitGame() {
        Application.Quit();
    }
}
