namespace DotnetTemplates.Configs;

/// <summary>
/// Configuration model for Seq.
/// </summary>
public class SeqConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether logs will be written to Seq.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the URL of the Seq instance.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating the API key used to access Seq.
    /// </summary>
    public string ApiKey { get; set; } = default!;
}
