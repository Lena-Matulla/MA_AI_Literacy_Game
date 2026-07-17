using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameObject tutorial;
    private void Awake()
    {
        GameConfigManager.LoadOrCreateConfig();

        tutorial.SetActive(true);

    }
}
