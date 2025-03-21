using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    private Button switchButton; // Reference to the button component

    private void Start()
    {
        // Get the Button component automatically (ensure the script is attached to the button)
        switchButton = GetComponent<Button>();

        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchScene);
        }
        else
        {
            Debug.LogWarning("SceneSwitcher: No Button component found on this GameObject!");
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("SampleScene"); // Load the target scene
    }
}
