namespace Voxon.VolumetricShapes
{
    /// <summary>
    /// Interface for objects that can be highlighted
    /// </summary>
    public interface IHighlightable
    {
        void SetFocused(bool focused);
        void SetHighlighted(bool highlighted);
        bool IsFocused { get; }
        bool IsHighlighted { get; }
    }
}

