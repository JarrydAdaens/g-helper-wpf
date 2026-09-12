namespace GHelper.WPF
{
    /// <summary>
    /// Single-instance guard for the WPF head. The first process to start owns a named
    /// event; later ones signal it and quit, which asks the owner to show itself.
    /// </summary>
    /// <remarks>
    /// The WinForms head uses a global event to make a new instance *replace* the old one
    /// (see <c>app/Helpers/ProcessHelper.cs</c>). This head does the opposite — the running
    /// instance wins — and stays session-local, so it never collides with a G-Helper
    /// instance running as SYSTEM or in another user's session.
    /// </remarks>
    internal sealed class SingleInstance : IDisposable
    {
        private const string ShowEventName = @"Local\GHelperWpfApp-Show";

        private readonly EventWaitHandle _showEvent;
        private readonly RegisteredWaitHandle _registration;

        private SingleInstance(EventWaitHandle showEvent, Action onShowRequested)
        {
            _showEvent = showEvent;
            _registration = ThreadPool.RegisterWaitForSingleObject(
                showEvent,
                (_, _) => onShowRequested(),
                null,
                Timeout.Infinite,
                executeOnlyOnce: false);
        }

        /// <summary>
        /// Claims the single-instance slot. Returns <c>null</c> when another instance already
        /// holds it, after asking that instance to show its window.
        /// </summary>
        /// <param name="onShowRequested">
        /// Called on a thread-pool thread whenever a later instance asks for the window.
        /// </param>
        public static SingleInstance? Claim(Action onShowRequested)
        {
            EventWaitHandle showEvent = new EventWaitHandle(false, EventResetMode.AutoReset, ShowEventName, out bool createdNew);

            if (createdNew)
            {
                return new SingleInstance(showEvent, onShowRequested);
            }

            showEvent.Set();
            showEvent.Dispose();
            return null;
        }

        public void Dispose()
        {
            _registration.Unregister(null);
            _showEvent.Dispose();
        }
    }
}
