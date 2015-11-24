using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that handles the character creation screen
/// </summary>
public class CharacterCreationScript : MenuUIScript
{
    #region Fields

    static CharacterCreationScript instance;

    [SerializeField]Text title;
    [SerializeField]GameObject ranger;
    [SerializeField]GameObject mage;
    [SerializeField]GameObject warrior;
    [SerializeField]GameObject selectButton;
    [SerializeField]GameObject remapPanel;
    [SerializeField]Text[] mainTexts;
    [SerializeField]Text[] secondTexts;
    [SerializeField]Text[] powerTexts;
    [SerializeField]Text[] specialTexts;
    CharacterType selectedType;
    Dictionary<CharacterType, GameObject> previewObjects;
    Dictionary<InputType, Text[]> abilityTexts;
    Dictionary<CharacterType, string> classNames;
    InputType currentRemap;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the static instance of the script
    /// </summary>
    public static CharacterCreationScript Instance
    { get { return instance; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the chosen character type
    /// </summary>
    /// <param name="type">the type</param>
    public void SetCharacterType(CharacterType type)
    {
        previewObjects[selectedType].SetActive(false);
        previewObjects[type].SetActive(true);
        selectedType = type;
        selectButton.SetActive(true);
        title.text = classNames[type];
    }

    /// <summary>
    /// Handles the confirm selection button being pressed
    /// </summary>
    public void SelectButtonPressed()
    {
        //GameManager.Instance.CurrentSaveName = "temp";
        //GameManager.Instance.NewSavegame(selectedType);
        GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].PlayerType = selectedType;
        GameManager.Instance.Save();
        Application.LoadLevel(Constants.MAP_SCENE);
    }

    /// <summary>
    /// Handles a remap
    /// </summary>
    /// <param name="axis">the axis name</param>
    public void RemapAxisPressed(InputType inputType)
    {
        remapPanel.SetActive(true);
        currentRemap = inputType;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        instance = this;
        previewObjects = new Dictionary<CharacterType, GameObject>();
        previewObjects.Add(CharacterType.Ranger, ranger);
        previewObjects.Add(CharacterType.Mage, mage);
        previewObjects.Add(CharacterType.Warrior, warrior);
        abilityTexts = new Dictionary<InputType, Text[]>();
        abilityTexts.Add(InputType.Main, mainTexts);
        abilityTexts.Add(InputType.Secondary, secondTexts);
        abilityTexts.Add(InputType.Power, powerTexts);
        abilityTexts.Add(InputType.Special, specialTexts);
        classNames = new Dictionary<CharacterType, string>();
        classNames.Add(CharacterType.Ranger, "RANGER");
        classNames.Add(CharacterType.Mage, "MAGE");
        classNames.Add(CharacterType.Warrior, "WARRIOR");
        UpdateMapping();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (remapPanel.activeSelf)
        {
            if (Input.inputString != "")
            { 
                string key = Input.inputString[0].ToString();
                foreach (KeyValuePair<InputType, InputButton> input in GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].Inputs)
                {
                    if (input.Value.Key == key)
                    { input.Value.SetBlank(); }
                }
                GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].Inputs[currentRemap].Key = key;
                UpdateMapping();
            }
            else
            {
                // Check mouse buttons
                for (int i = 0; i < 7; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        foreach (KeyValuePair<InputType, InputButton> input in GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].Inputs)
                        {
                            if (input.Value.MouseButton == i)
                            { input.Value.SetBlank(); }
                        }
                        GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].Inputs[currentRemap].MouseButton = i;
                        UpdateMapping();
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Updates button mapping text on remap
    /// </summary>
    private void UpdateMapping()
    {
        foreach (KeyValuePair<InputType, Text[]> texts in abilityTexts)
        {
            foreach (Text txt in texts.Value)
            { txt.text = GameManager.Instance.Saves[GameManager.Instance.CurrentSaveName].Inputs[texts.Key].Name; }
        }
        remapPanel.SetActive(false);
    }

    #endregion
}
