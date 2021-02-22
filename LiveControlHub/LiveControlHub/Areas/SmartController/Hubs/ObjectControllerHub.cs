using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace LiveControlHub.Areas.SmartController.Hubs
{
    public class ObjectControllerHub : Hub
    {
        private static Vector3 objectPosition = new Vector3();
        private static Vector3 objectRotation = new Vector3();
        public void SaveObjectPosition(string objectName)
        {
            objectPosition = LiveControlHub.Program.ObjectController.GetObjectPosition(objectName);
        }

        public void MoveObjectPositionXYZ(float x, float y, float z, string objectName)
        {
            var scale = 0.01f;
            
            var dx = -y * scale + objectPosition.X;
            var dy = -x * scale + objectPosition.Y;
            var dz = z * scale + objectPosition.Z;

            LiveControlHub.Program.ObjectController.MoveObjectSetPosition(dx, dy, dz, objectName);
            return;
        }

        public void ResetObjectRotation(bool x, bool y, bool z, string objectName)
        {
            var r = LiveControlHub.Program.ObjectController.GetObjectRotation(objectName);

            if(x)
            {
                LiveControlHub.Program.ObjectController.RotateObjectSetRotation(0, r.Y, r.Z, objectName);
            }

            if (y)
            {
                LiveControlHub.Program.ObjectController.RotateObjectSetRotation(r.X, 0, r.Z, objectName);
            }

            if (z)
            {
                LiveControlHub.Program.ObjectController.RotateObjectSetRotation(r.X, r.Y, 0, objectName);
            }
        }
        public void SaveObjectRotation(string objectName)
        {
            objectRotation = LiveControlHub.Program.ObjectController.GetObjectRotation(objectName);
        }

        public void RotateObjectXYZ(float x, float y, float z, float scale, string objectName)
        {
            var sscale = 0.06f * scale;

            var dx = x * sscale;
            var dy = y * sscale;
            var dz = z * sscale;

            objectRotation.X -= dx;
            objectRotation.Y -= dy;
            objectRotation.Z -= dz;

            var xr = objectRotation.X;
            var yr = objectRotation.Y;
            var zr = objectRotation.Z;

           LiveControlHub.Program.ObjectController.RotateObjectSetRotation(xr, yr, zr, objectName);
            return;
        }
    }
}
