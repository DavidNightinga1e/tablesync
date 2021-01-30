using System;
using ExitGames.Client.Photon;
using UnityEngine;

namespace TableSync
 {
     [Serializable]
     public class BulletSpawnEventData
     {
         public Vector2 position;
         public float rotation;
         
         private const int Size = 3 * 4; // 3 floats
         private static readonly byte[] Mem = new byte[Size];
         
         public static short Serialize(StreamBuffer outStream, object customObject)
         {
             var bsp = (BulletSpawnEventData) customObject;
             lock (Mem)
             {
                 var bytes = Mem;
                 var index = 0;
                 Protocol.Serialize(bsp.position.x, bytes, ref index);
                 Protocol.Serialize(bsp.position.y, bytes, ref index);
                 Protocol.Serialize(bsp.rotation, bytes, ref index);
                 outStream.Write(bytes, 0, Size);
             }
 
             return Size;
         }
 
         public static object Deserialize(StreamBuffer inStream, short length)
         {
             var bsp = new BulletSpawnEventData();
             lock (Mem)
             {
                 inStream.Read(Mem, 0, Size);
                 var index = 0;
                 Protocol.Deserialize(out bsp.position.x, Mem, ref index);
                 Protocol.Deserialize(out bsp.position.y, Mem, ref index);
                 Protocol.Deserialize(out bsp.rotation, Mem, ref index);
             }
 
             return bsp;
         }
     }
 }