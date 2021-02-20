using LiveControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveClientForBlender
{
    public class ObjectController : IObjectController
    {
        private BlenderRemoteScriptServer server = new BlenderRemoteScriptServer();

        public void Start()
        {
            server.Start();
        }

        public void MoveObjectAddPosition(double x, double y, double z, string objectName)
        {
            var script = $@"ob = bpy.data.objects['{objectName}']
ob.location = ob.location + mathutils.Vector((.1, .0, .0))";

            var response = server.RunScript(script);
        }
    }
}
