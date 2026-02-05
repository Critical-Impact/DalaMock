namespace DalaMock.Shared.Interfaces;

using System.Numerics;

using Dalamud.Interface;

public interface IImGuiComponents
{
    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="icon">The icon for the button.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(FontAwesomeIcon icon);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(FontAwesomeIcon icon, Vector2 size);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="defaultColor">The default color of the button.</param>
    /// <param name="activeColor">The color of the button when active.</param>
    /// <param name="hoveredColor">The color of the button when hovered.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(FontAwesomeIcon icon, Vector4? defaultColor, Vector4? activeColor = null, Vector4? hoveredColor = null, Vector2? size = null);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(int id, FontAwesomeIcon icon);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(int id, FontAwesomeIcon icon, Vector2 size);

    /// <summary>
    /// IconButton component to use an icon as a button with color options.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="defaultColor">The default color of the button.</param>
    /// <param name="activeColor">The color of the button when active.</param>
    /// <param name="hoveredColor">The color of the button when hovered.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(int id, FontAwesomeIcon icon, Vector4? defaultColor, Vector4? activeColor = null, Vector4? hoveredColor = null, Vector2? size = null);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string id, FontAwesomeIcon icon);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string id, FontAwesomeIcon icon, Vector2 size);

    /// <summary>
    /// IconButton component to use an icon as a button with color options.
    /// </summary>
    /// <param name="id">The ID of the button.</param>
    /// <param name="icon">The icon for the button.</param>
    /// <param name="defaultColor">The default color of the button.</param>
    /// <param name="activeColor">The color of the button when active.</param>
    /// <param name="hoveredColor">The color of the button when hovered.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string id, FontAwesomeIcon icon, Vector4? defaultColor, Vector4? activeColor = null, Vector4? hoveredColor = null, Vector2? size = null);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="iconText">Text already containing the icon string.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string iconText);

    /// <summary>
    /// IconButton component to use an icon as a button.
    /// </summary>
    /// <param name="iconText">Text already containing the icon string.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string iconText, Vector2 size);

    /// <summary>
    /// IconButton component to use an icon as a button with color options.
    /// </summary>
    /// <param name="iconText">Text already containing the icon string.</param>
    /// <param name="defaultColor">The default color of the button.</param>
    /// <param name="activeColor">The color of the button when active.</param>
    /// <param name="hoveredColor">The color of the button when hovered.</param>
    /// <param name="size">Sets the size of the button. If either dimension is set to 0, that dimension will conform to the size of the icon.</param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButton(string iconText, Vector4? defaultColor, Vector4? activeColor = null, Vector4? hoveredColor = null, Vector2? size = null);

    /// <summary>
    /// IconButton component to use an icon as a button with color options.
    /// </summary>
    /// <param name="icon">Icon to show.</param>
    /// <param name="text">Text to show.</param>
    /// <param name="size">
    /// Sets the size of the button. If either dimension is set to 0,
    /// that dimension will conform to the size of the icon and text.
    /// </param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButtonWithText(FontAwesomeIcon icon, string text, Vector2 size);

    /// <summary>
    /// IconButton component to use an icon as a button with color options.
    /// </summary>
    /// <param name="icon">Icon to show.</param>
    /// <param name="text">Text to show.</param>
    /// <param name="defaultColor">The default color of the button.</param>
    /// <param name="activeColor">The color of the button when active.</param>
    /// <param name="hoveredColor">The color of the button when hovered.</param>
    /// <param name="size">
    /// Sets the size of the button. If either dimension is set to 0,
    /// that dimension will conform to the size of the icon and text.
    /// </param>
    /// <returns>Indicator if button is clicked.</returns>
    bool IconButtonWithText(FontAwesomeIcon icon, string text, Vector4? defaultColor = null, Vector4? activeColor = null, Vector4? hoveredColor = null, Vector2? size = null);

    /// <summary>
    /// Get width of IconButtonWithText component.
    /// </summary>
    /// <param name="icon">Icon to use.</param>
    /// <param name="text">Text to use.</param>
    /// <returns>Width.</returns>
    float GetIconButtonWithTextWidth(FontAwesomeIcon icon, string text);
}
