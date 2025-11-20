using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Utility class to add sound effects to UI Toolkit buttons.
/// Adds rollover sound on hover and click sound on click.
/// </summary>
public static class UIButtonSoundHandler
{
    /// <summary>
    /// Adds sound effects to a button.
    /// </summary>
    /// <param name="button">The button to add sound effects to</param>
    public static void AddSoundEffects(Button button)
    {
        if (button == null) return;
        
        // Add hover sound
        button.RegisterCallback<MouseEnterEvent>(_ => {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayRolloverSound();
        });
        
        // Add click sound (using mouseDown instead of clicked to make sound play immediately)
        button.RegisterCallback<MouseDownEvent>(_ => {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClickSound();
        });
    }
    
    /// <summary>
    /// Adds sound effects to multiple buttons.
    /// </summary>
    /// <param name="buttons">The buttons to add sound effects to</param>
    public static void AddSoundEffectsToButtons(params Button[] buttons)
    {
        foreach (var button in buttons)
        {
            AddSoundEffects(button);
        }
    }
    
    /// <summary>
    /// Adds sound effects to all buttons in a VisualElement.
    /// </summary>
    /// <param name="root">The root VisualElement containing buttons</param>
    public static void AddSoundEffectsToAllButtons(VisualElement root)
    {
        if (root == null) return;
        
        // Find all buttons in the hierarchy
        root.Query<Button>().ForEach(button => {
            AddSoundEffects(button);
        });
    }
}