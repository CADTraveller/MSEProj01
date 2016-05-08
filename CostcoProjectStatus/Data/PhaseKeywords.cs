using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;

namespace Data
{
    public static class PhaseKeywords
    {
        public static Dictionary<Phases, List<string>> ExactKeywords
        {
            get
            {
                return new Dictionary<Phases, List<string>>
                {
                    {Phases.Build_Test, new List<string>() },
                    {Phases.Deploy,  new List<string>() { "udeploy" } },
                    {Phases.Macro_Design, new List<string>() },
                    {Phases.Micro_Design, new List<string>() },
                    {Phases.Solution_Outline, new List<string>() },
                    {Phases.Start_Up, new List<string>() },
                    {Phases.Transition_Close, new List<string>() }
                };
            }
        }

        public static Dictionary<Phases, List<string>> FuzzyKeywords
        {
            get
            {
                return new Dictionary<Phases, List<string>>
                {
                    {Phases.Build_Test, new List<string>() { "build", "test", "testing" } },
                    {Phases.Deploy,  new List<string>() { "deployment" } },
                    {Phases.Macro_Design, new List<string>() {"macro", "high level", "design" } },
                    {Phases.Micro_Design, new List<string>() {"micro", "detail", "design" } },
                    {Phases.Solution_Outline, new List<string>() { "outline", "sketch"} },
                    {Phases.Start_Up, new List<string>() {"start up" } },
                    {Phases.Transition_Close, new List<string>() {"close", "transition" } }
                };
            }
        }

        public static Phases GuessPhase(List<StatusUpdate> updates)
        {


            return Phases.Not_Assigned;
        }
    }
}
