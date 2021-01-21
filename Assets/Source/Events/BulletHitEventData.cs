using System;
using ExitGames.Client.Photon;
using UnityEngine;

namespace TableSync
{
    [Serializable]
    public class BulletHitEventData
    {
        public Vector2 direction;
        public int viewId;

        private const int SizeOfBulletHitData = 3 * 4; // 2 floats and int

        private static readonly byte[] MemBulletHitData = new byte[SizeOfBulletHitData];
            
        public static short Serialize(StreamBuffer outStream, object customObject)
        {
            var bh = (BulletHitEventData) customObject;
            lock (MemBulletHitData)
            {
                var bytes = MemBulletHitData;
                var index = 0;
                Protocol.Serialize(bh.direction.x, bytes, ref index);
                Protocol.Serialize(bh.direction.y, bytes, ref index);
                Protocol.Serialize(bh.viewId, bytes, ref index);
                outStream.Write(bytes, 0, SizeOfBulletHitData);
            }

            return SizeOfBulletHitData;
        }

        public static object Deserialize(StreamBuffer inStream, short length)
        {
            var bh = new BulletHitEventData();
            lock (MemBulletHitData)
            {
                inStream.Read(MemBulletHitData, 0, SizeOfBulletHitData);
                var index = 0;
                Protocol.Deserialize(out bh.direction.x, MemBulletHitData, ref index);
                Protocol.Deserialize(out bh.direction.y, MemBulletHitData, ref index);
                Protocol.Deserialize(out bh.viewId, MemBulletHitData, ref index);
            }

            return bh;
        }
    }
}