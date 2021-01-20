using Photon.Realtime;

namespace TableSync
{
    public class EventsUtilities
    {
        public static RaiseEventOptions RaiseEventOptionsReceiversAll =
            new RaiseEventOptions {Receivers = ReceiverGroup.All};
    }
}