namespace Voxon.CatFace
{
    /// <summary>
    /// Enum/definitions for cat expression types
    /// 
    /// Based on research from:
    /// - CatFACS (Cat Facial Action Coding System): Identifies facial actions associated with specific emotions
    ///   (Caeiro et al., 2017; https://pubmed.ncbi.nlm.nih.gov/28341145/)
    /// - Human recognition of cat affective states from facial expressions
    ///   (Humans can identify cats' affective states from subtle facial expressions, Cambridge Core)
    /// - Comprehensive feline ethogram for emotion identification
    ///   (Irish Veterinary Journal, 2021; https://irishvetjournal.biomedcentral.com/articles/10.1186/s13620-021-00189-z)
    /// 
    /// Expression mappings:
    /// - Fear: Blinking, half-blinking, left head/gaze bias (CatFACS)
    /// - Frustration: Hissing, nose-licking, jaw dropping, upper lip raising, nose wrinkling, 
    ///   lower lip depression, lip parting, mouth stretching, vocalization, tongue showing (CatFACS)
    /// - Positive states: Relaxed eyes, forward-facing ears, slow blinking
    /// </summary>
    public enum ExpressionType
    {
        Neutral,      // Relaxed, baseline state
        Happy,        // Positive affect: relaxed eyes, forward ears, slow blinking
        Curious,      // Interest/exploration: forward-facing ears, wide eyes, head orientation
        Surprised,    // Alert/startled: wide eyes, raised ears
        Sleepy,       // Drowsy/relaxed: half-closed eyes, relaxed facial muscles
        Playful,      // Positive engagement: forward ears, alert eyes, open mouth
        Focused,      // Attention/concentration: forward gaze, still head, alert ears
        Sad           // Negative affect: lowered ears, narrowed eyes, withdrawn expression
    }
}

