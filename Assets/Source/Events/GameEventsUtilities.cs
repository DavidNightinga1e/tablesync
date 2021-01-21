using Photon.Realtime;

namespace TableSync
{
    public class GameEventsUtilities
    {
        public static RaiseEventOptions RaiseEventOptionsReceiversAll =
            new RaiseEventOptions {Receivers = ReceiverGroup.All};
    }
}