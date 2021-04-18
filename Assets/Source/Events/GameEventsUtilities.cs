using Photon.Realtime;

namespace TableSync
{
    public class GameEventsUtilities
    {
        public static readonly RaiseEventOptions RaiseEventOptionsReceiversAll =
            new RaiseEventOptions {Receivers = ReceiverGroup.All};
    }
}