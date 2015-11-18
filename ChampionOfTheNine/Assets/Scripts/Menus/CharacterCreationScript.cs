using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that handles the character creation screen
/// </summary>
public class CharacterCreationScript : MonoBehaviour
{
    #region Fields

    static CharacterCreationScript instance;

    [SerializeField]GameObject ranger;
    [SerializeField]GameObject mage;
    [SerializeField]GameObject warrior;
    [SerializeField]GameObject selectButton;
    CharacterType selectedType;
    Dictionary<CharacterType, GameObject> previewObjects;

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
    }

    /// <summary>
    /// Handles the confirm selection button being pressed
    /// </summary>
    public void SelectButtonPressed()
    {
        GameManager.Instance.CurrentSaveName = "temp";
        GameManager.Instance.NewSavegame(selectedType);
        Application.LoadLevel(Constants.MAP_SCENE);
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
    }

    #endregion
}
