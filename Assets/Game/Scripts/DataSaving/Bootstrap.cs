using UnityEngine;

/// <summary>
/// Called when the game is started. Makes sure the Config is loaded and starts the tutorial.
/// </summary>
public class Bootstrap : MonoBehaviour
{
    public GameObject tutorial;
    private void Awake()
    {
        GameConfigManager.LoadOrCreateConfig();

        tutorial.SetActive(true);

    }
}
