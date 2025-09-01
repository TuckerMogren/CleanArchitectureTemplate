
using Domain;

namespace Application
{
    /// <summary>
    /// Marker type for the Application assembly.
    /// Also creates a concrete IL dependency on the Domain assembly
    /// so architecture tests can detect it.
    /// </summary>
    public sealed class Marker
    {
        private static readonly System.Type? _ = typeof(Domain.Marker);
    }
}
