using Photon.Realtime;

namespace TableSync.Demo
{
    public class NetEventsUtilities
    {
        public static RaiseEventOptions RaiseEventOptionsReceiversAll = new RaiseEventOptions{Receivers = ReceiverGroup.All};
    }
}