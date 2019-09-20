using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

[assembly: Rage.Attributes.Plugin("GoThere", Description = "GoThere lets users easily teleport around San Andreas to all police stations.", Author = "Peter Georgas")]
namespace GoThere
{
    public static class GoThere
    {
        private static GameFiber MenuProcessFiber;
        private static MenuPool menu_pool;
        private static UIMenu GoMenu;
        private static UIMenuListItem StationList;
        private static String current_item;
        public static void Main()
        {

            // Display a notification in the console that the plugin loaded successfully
            Game.DisplayNotification("~r~GoThere ~w~by ~b~Peter Georgas ~w~has loaded successfully!"); // Display a notification above the radar that the plugin loaded successfully


            GoMenu = new UIMenu("GoThere", "~b~LET'S MOVE!");
            StationList = new UIMenuListItem("~g~List of Stations: ", "", "Bolingbroke", "Davis", "Downtown Vinewood", "La Mesa", "LSIA", "Mission Row",
                                                            "Paleto Bay", "Rockford Hills", "Sandy Shores", "Vespucci", "Vinewood Hills");
            menu_pool = new MenuPool();

            MenuProcessFiber = new GameFiber(ProcessLoop);

            menu_pool.Add(GoMenu); // Add our menu to the menu pool
            GoMenu.AddItem(StationList); // Add a list of destinations -- maybe we want to hold destination OBJECTS. We shall see. 
            GoMenu.AddItem(new UIMenuItem("~y~Teleport")); // Add a button that will ultimately teleport you.
            GoMenu.RefreshIndex(); // Set the index at 0 by using the RefreshIndex method

            GoMenu.OnItemSelect += OnItemSelect;
            GoMenu.OnListChange += OnListChange;

            MenuProcessFiber.Start(); // Start process fiber

            GameFiber.Hibernate(); //Continue with our plugin. Prevent it from being unloaded
        }

        public static void ProcessLoop()
        {
            while (true)
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(System.Windows.Forms.Keys.F4) && !menu_pool.IsAnyMenuOpen()) // Toggle switch for our menu. Going to make this a config file. 
                {
                    GoMenu.Visible = !GoMenu.Visible;
                }
                menu_pool.ProcessMenus();
            }
        }

        public static void OnListChange(UIMenu sender, UIMenuListItem list, int index)
        {
            if (sender != GoMenu || list != StationList) return; // We only want to detect changes from our menu.
           
            current_item = list.Collection[index].Value.ToString();
           //Game.DisplaySubtitle("~g~Current Item: " + current_item);

        }

        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index) //Overridden OnItemSelect method
        {
            //"Bolingbroke", "Davis", "Downtown Vinewood", "La Mesa", "LSIA", "Mission Row",
            // "Paleto Bay", "Rockford Hills", "Sandy Shores", "Vespucci", "Vinewood Hills"
            if (sender != GoMenu) return; // If there is no change from our menu, just return. 

            switch(current_item)
            {
                case "Bolingbroke":
                    TeleportCommands.Command_TeleportToBolingbroke();
                    break;
                case "Davis":
                    TeleportCommands.Command_TeleportToDavis();
                    break;
                case "Downtown Vinewood":
                    TeleportCommands.Command_TeleportToDowntownVinewood();
                    break;
                case "La Mesa":
                    TeleportCommands.Command_TeleportToLaMesa();
                    break;
                case "LSIA":
                    TeleportCommands.Command_TeleportToLSIA();
                    break;
                case "Mission Row":
                    TeleportCommands.Command_TeleportToMissionRow();
                    break;
                case "Paleto Bay":
                    TeleportCommands.Command_TeleportToPaleto();
                    break;
                case "Rockford Hills":
                    TeleportCommands.Command_TeleportToRockford();
                    break;
                case "Sandy Shores":
                    TeleportCommands.Command_TeleportToSandyShores();
                    break;
                case "Vespucci":
                    TeleportCommands.Command_TeleportToVespucci();
                    break;
                case "Vinewood Hills":
                    TeleportCommands.Command_TeleportToVinewoodHills();
                    break;
            }
        }
    }


}
