using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Singleton class that stores game data
/// </summary>
public class GameManager
{
    #region Fields

    static GameManager instance;
    Dictionary<string, Savegame> saves;
    Dictionary<CharacterType, GameObject> playerPrefabs;
    Dictionary<string, AudioClip> gameSounds;
    Dictionary<string, GameObject> particles;
    Queue<ParticleSystem> activeParticles;
    
    #endregion

    #region Constructor

    /// <summary>
    /// Private constructor
    /// </summary>
    private GameManager()
    {
        GameObject.DontDestroyOnLoad(new GameObject("gmUpdater", typeof(Updater)));

        // Loads the saved levels, if possible
        saves = Serializer.Deserialize<Dictionary<string, Savegame>>(Constants.SAVES_FILE);
        if (saves == null)
        {
            saves = new Dictionary<string, Savegame>();
            Save();
            CurrentSaveName = "";
        }
        else
        {
            if (saves.Count > 0)
            { CurrentSaveName = saves.First().Key; }
            else
            { CurrentSaveName = ""; }
        }
        Paused = false;

        // Loads player prefabs
        playerPrefabs = new Dictionary<CharacterType, GameObject>();
        playerPrefabs.Add(CharacterType.Ranger, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.RANGER_PLAYER_PREFAB));
        playerPrefabs.Add(CharacterType.Mage, Resources.Load<GameObject>(Constants.PREFAB_FOLDER + Constants.MAGE_PLAYER_PREFAB));

        // Loads the game sounds
        gameSounds = new Dictionary<string, AudioClip>();
        AudioClip[] sounds = Resources.LoadAll<AudioClip>(Constants.SND_FOLDER);
        foreach (AudioClip sound in sounds)
        { gameSounds.Add(sound.name, sound); }

        // Loads the particles
        activeParticles = new Queue<ParticleSystem>();
        particles = new Dictionary<string, GameObject>();
        GameObject[] parts = Resources.LoadAll<GameObject>(Constants.PART_FOLDER);
        foreach (GameObject part in parts)
        { particles.Add(part.name, part); }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the game manager
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameManager(); }

            return instance;
        }
    }

    /// <summary>
    /// Gets the saves dictionary
    /// </summary>
    public Dictionary<string, Savegame> Saves
    { get { return saves; } }

    /// <summary>
    /// Gets the player prefab dictionary
    /// </summary>
    public Dictionary<CharacterType, GameObject> PlayerPrefabs
    { get { return playerPrefabs; } }

    /// <summary>
    /// Gets the pre-loaded game sounds dictionary
    /// </summary>
    public Dictionary<string, AudioClip> GameSounds
    { get { return gameSounds; } }

    /// <summary>
    /// Gets or sets the current save name
    /// </summary>
    public string CurrentSaveName
    { get; set; }

    /// <summary>
    /// Gets or sets whether or not the game is paused
    /// </summary>
    public bool Paused
    { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Spawns a particle emitter at the given location
    /// </summary>
    /// <param name="name">the name of the particle emitter prefab to spawn</param>
    /// <param name="position">the position at which to spawn the particle emitter</param>
    public void SpawnParticle(string name, Vector3 position)
    {
        activeParticles.Enqueue(((GameObject)GameObject.Instantiate(particles[name], position, Quaternion.identity)).GetComponent<ParticleSystem>());
    }

    /// <summary>
    /// Deletes a savegame
    /// </summary>
    public void DeleteSave()
    {
        saves.Remove(CurrentSaveName);
        Save();
    }

    /// <summary>
    /// Creates a new savegame
    /// </summary>
    public void NewSavegame(CharacterType playerType)
    {
        if (!saves.ContainsKey(CurrentSaveName))
        { saves.Add(CurrentSaveName, new Savegame(playerType)); }
        else
        { saves[CurrentSaveName] = new Savegame(playerType); }
        Save();
    }

    /// <summary>
    /// Saves the current state of the savegames to a file
    /// </summary>
    public void Save()
    {
        Serializer.Serialize(Constants.SAVES_FILE, saves);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the game manager
    /// </summary>
    private void Update()
    {
        // Checks if the oldest particle should be removed
        if (activeParticles.Count > 0)
        {
            try
            {
                if (!activeParticles.Peek().IsAlive())
                {
                    GameObject.Destroy(activeParticles.Peek().gameObject);
                    activeParticles.Dequeue();
                }
            }
            catch (System.Exception)
            { activeParticles.Dequeue(); }
        }
    }

    #endregion

    #region Updater class

    /// <summary>
    /// Internal class that updates the game manager
    /// </summary>
    class Updater : MonoBehaviour
    {
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        { 
            GameManager.Instance.Update();
        }
    }

    #endregion
}
