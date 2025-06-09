using UnityEngine;

public class FirstButtonTracker : MonoBehaviour
{
    private static MenuNavigation menuNav;

    void OnEnable()
    {
        if (menuNav == null)
            menuNav = FindObjectOfType<MenuNavigation>();

        if (menuNav != null)
        {
            menuNav.RegisterFirstButton(gameObject);
        }
    }

    void OnDisable()
    {
        if (menuNav != null)
        {
            menuNav.UnregisterFirstButton(gameObject);
        }
    }
}