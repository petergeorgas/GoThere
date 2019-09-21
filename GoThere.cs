using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

[assembly: Rage.Attributes.Plugin("GoThere", Description = "GoThere lets users easily teleport around San Andreas to all police stations.", Author = "Peter Georgas")]

//TODO: Prevent GoThere from being loaded multiple times.
namespace GoThere
{
    public static class GoThere
    {
        private static GameFiber MenuProcessFiber;
        private static MenuPool menu_pool;
        private static UIMenu GoMenu;
        private static UIMenuListItem StationList;
        private static String current_item;
        private static System.Windows.Forms.Keys menuKey;
        public static void Main()
        {
            handleFileCreation(); // Worry about the settings and such.

            current_item = "Bolingbroke"; // The current item of the list of police stations is updated to Bolingbroke, this fixes a bug where upon first loading the plugin, a user 
                                          // Would have to switch to a different location before being able to teleport back to Bolingbroke. 


            // Display a notification in the console that the plugin loaded successfully
            Game.DisplayNotification("~r~[GoThere] \nGoThere ~w~by ~b~Peter Georgas ~w~has loaded successfully!"); // Display a notification above the radar that the plugin loaded successfully


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

            GameFiber.Hibernate(); // Continue with our plugin. Prevent it from being unloaded
        }

        public static void ProcessLoop()
        {
            while (true)
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(menuKey) && !menu_pool.IsAnyMenuOpen()) // Toggle switch for our menu. Going to make this a config file. 
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


        public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index) // Overridden OnItemSelect method
        {
            if (sender != GoMenu) return; // If there is no change from our menu, just return. 

            switch (current_item)
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


        // This Method is called right when our program begins, it's gonna take care of all of the reading/writing from our XML file we may need to do when the plugin loads.
        public static void handleFileCreation()
        {
            try
            {
                if (!File.Exists("Plugins/GoThere/Options.xml"))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                    using (XmlWriter writer = XmlWriter.Create("Plugins/GoThere/Options.xml", settings)) // If the file key doesn't exist, create it
                    {

                        
                        //This is kinda fucked up right now, I will have to look into learning how to make the XML Fields Cascade.
                        writer.WriteStartElement("GoThere");
                        writer.WriteElementString("MenuKey", "F4");
                        writer.WriteEndElement();
                        writer.Flush();

                        menuKey = System.Windows.Forms.Keys.F4; //Set the menu key to F4
                        Console.WriteLine("[GoThere] Options.xml did not exist, so it was created. Default Menu Key is F4!");
                    }
                }
                else // If the file does exist
                {
                    // Right now I'm just going to use XMLDocument. Apparently XMLReader is faster and more memory efficient. 
                    XmlDocument options = new XmlDocument();
                    options.Load("Plugins/GoThere/Options.xml"); // Load the XML document

                    XmlNode menuKeyNode = options.DocumentElement.SelectSingleNode("/GoThere/MenuKey");
                    string tempkey = menuKeyNode.InnerText;


                    // Once we have grabbed all of our data from the XMLDocument, we should close it, and collect the garbage.
                    options = null;
                    menuKeyNode = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers(); // Called for GC to occur Synchronously.

                    // Try to set the menu toggle key
                    try
                    {
                        menuKey = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), tempkey); // Set the Menu key to whatever it is in the XML file.
                        Console.WriteLine("[GoThere] Key to open GoThere Menu: " + tempkey);
                    }
                    catch (ArgumentException AE) // Enum.Parse throws an ArgumentException if the key string is not in the enumeration, so we're going to need to catch it 
                    {
                        menuKey = System.Windows.Forms.Keys.F4; // Set the menuKey to F4
                        Console.WriteLine("[GoThere] The value for Menu Key in Options.xml does not exist! You can use F4 to open GoThere!");
                        Game.DisplayNotification("~r~[GoThere] \n~w~You have entered a value in ~b~Options.xml ~w~for a menu key that does not exist. You can use ~g~F4 ~w~ to open GoThere!");

                    }
                }
            }
            catch (DirectoryNotFoundException DNF) // Exception thrown when the GoThere folder does not reside in the Plugins folder upon loading the plugin. 
            {
                Game.DisplayNotification("~r~[GoThere] \n~w~GoThere folder could not be found in ~g~GTAV~w~/~g~Plugins ~w~folder. Please either ~g~create ~w~the folder or ~g~drag and drop ~w~it from the zip archive.");
                // Exit the plugin.
                // If we don't exit, shit hits the fan because the plugin will be loaded twice, and it will attempt to create multiple console commands. 
            }
        }
    }
}
