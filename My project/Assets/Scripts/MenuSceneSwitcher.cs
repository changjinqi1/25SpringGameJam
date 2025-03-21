using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneSwitcher : MonoBehaviour
{
    private Button switchButton; // Reference to the button component

    private void Start()
    {
        // Get the Button component automatically (ensure the script is attached to the button)
        switchButton = GetComponent<Button>();

        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchToMenu);
        }
        else
        {
            Debug.LogWarning("MenuSceneSwitcher: No Button component found on this GameObject!");
        }
    }

    public void SwitchToMenu()
    {
        SceneManager.LoadScene("Menu"); // Load the target scene
    }
}
