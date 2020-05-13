using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Direct.Shared;
using Direct.Interface;

namespace ManageRobot
{
    [DirectSealed]
    [DirectDom("Robotic Management")]
    [ParameterType(false)]
    public class Main
    {
        public static readonly IDirectLog logArchitect = DirectLogManager.GetLogger(Loggers.LibraryObjects);

        [DirectDom("Set Context")]
        [DirectDomMethod("Set Context to {context} for {solution} with {load on startup}")]
        [MethodDescriptionAttribute("Sets the context and load on startup for an already assigned solution for the users team")]
        public static string SetCntx(string cntx, string sol, bool ldstrp)
        {
            return Helpers.SetRobot.setContext(cntx, sol, ldstrp);
        }
    }
}
