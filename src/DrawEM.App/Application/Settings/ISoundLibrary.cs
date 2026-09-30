using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>
/// drawEM's managed copies of user-selected WAV and MP3 files. Importing copies the source; the source
/// file is never moved or deleted. Implemented in v3 Step 2.
/// </summary>
public interface ISoundLibrary
{
    /// <param name="sourceFile">The user's selected file, as the file picker reports it.</param>
    /// <exception cref="SoundImportException">The file is not a WAV or MP3, or the copy failed.</exception>
    SoundReference Import(string sourceFile);
}

/// <summary>Plays a draft sound on the global sound channel without changing active shortcuts.</summary>
public interface ISoundSampler
{
    void Sample(SoundReference sound);
}

public sealed class SoundImportException : Exception
{
    public SoundImportException(string message)
        : base(message)
    {
    }

    public SoundImportException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
