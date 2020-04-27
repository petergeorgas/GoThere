using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
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
        private static UIMenu LocMenu;
        private static UIMenuListItem StationList;
        private static UIMenuListItem CustomList;
        private static UIMenuItem navigateToLocMenu;
        
        private static String current_item;
        private static Keys menuKey;
        private static bool customLocationsEnabled;
        private static ControllerButtons XButton;
       // public static GameConsole console;
        public static List<Destination> customLocsList = new List<Destination>(); // List to add custom locations
        public static void Main()
        {
            handleFileCreation(); // Worry about the settings and such.

            
            current_item = "Bolingbroke"; // The current item of the list of police stations is updated to Bolingbroke, this fixes a bug where upon first loading the plugin, a user 
                                          // Would have to switch to a different location before being able to teleport back to Bolingbroke. 


           
            


            GoMenu = new UIMenu("GoThere", "~b~LET'S MOVE!");
            StationList = new UIMenuListItem("~g~List of Stations: ", "", "Bolingbroke", "Davis", "Downtown Vinewood", "La Mesa", "LSIA", "Mission Row",
                                                            "Paleto Bay", "Rockford Hills", "Sandy Shores", "Vespucci", "Vinewood Hills");

            LocMenu = new UIMenu("Custom Locations", "");

            menu_pool = new MenuPool();

            MenuProcessFiber = new GameFiber(ProcessLoop);


            menu_pool.Add(GoMenu); // Add our menu to the menu pool
            GoMenu.AddItem(StationList); // Add a list of destinations -- maybe we want to hold destination OBJECTS. We shall see. 
            GoMenu.OnItemSelect += OnItemSelect;
            GoMenu.OnListChange += OnListChange;

            if (customLocationsEnabled) // If custom locations are enabled 
            {
               
                navigateToLocMenu = new UIMenuItem("Custom Locations");
                LocMenu = new UIMenu("Custom Locations", "Teleport to custom locations."); // Initialize the locations menu
                menu_pool.Add(LocMenu); // Add the locations menu to the menu pool
                
                GoMenu.AddItem(navigateToLocMenu);  // Add the ui item to the main menu
                GoMenu.BindMenuToItem(LocMenu, navigateToLocMenu);  // Bind the locations menu to the ui item
                LocMenu.ParentMenu = GoMenu;    // Set the parent menu of the locations menu to be the main menu

                //UIMenuItem saveButton = new UIMenuItem("~g~Save current location");



                // Create a button to save current location as a new custom location -- allow for naming as well 
                // For each destination object in the list containing custom locations, create a button that teleports you to said location 
                // 
                RefreshCustomLocationsMenu();

            }
            GoMenu.RefreshIndex(); // Set the index at 0 by using the RefreshIndex method

            

            MenuProcessFiber.Start(); // Start process fiber

            Game.DisplayNotification("~r~[GoThere] \nGoThere ~w~by ~b~Peter Georgas ~w~has loaded successfully!"); // Display a notification above the radar that the plugin loaded successfully
            //console.Print("[GoThere] GoThere by Peter Georgas has loaded successfully!");

            GameFiber.Hibernate(); // Continue with our plugin. Prevent it from being unloaded
        }

        public static void ProcessLoop()
        {
            while (true)
            {
                GameFiber.Yield();
                if ((Game.IsKeyDown(menuKey) || Game.IsControllerButtonDown(XButton)) && !menu_pool.IsAnyMenuOpen()) // Toggle switch for our menu. Going to make this a config file. 
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
        
            if ((sender != GoMenu) || selectedItem == navigateToLocMenu) return; // If the sender is not the main menu or the selected item is the button to go to the custom locations menu, return.

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
                default:
                    break;
            }
        }


        // This method is responsible for creating the buttons for the custom locations menu
        public static void RefreshCustomLocationsMenu()
        {
            
            LocMenu.Clear(); // Clear the menu
          //  LocMenu.RefreshIndex();

            
            UIMenuStringSelector saveButton = new UIMenuStringSelector("~g~Save Current Location", "");
            LocMenu.AddItem(saveButton);


            try
            {
                foreach (Destination current_destination in customLocsList) // For each destination in the list containing all of the destinations
                {
                    Ped playerPed = Game.LocalPlayer.Character;
                    UIMenuItem newButton = new UIMenuItem(current_destination.getName());

                    LocMenu.AddItem(newButton); // Add a new button to the menu

                    LocMenu.OnItemSelect += (sender, selectedItem, index) => // When the button is clicked, teleport the player and notify them. 
                    {
                        if (index == 0) // If the save location button was clicked
                        {
                            Game.DisplaySubtitle("~g~Location has been saved!");
                        }
                        else // If a custom location was clicked
                        {
                            playerPed.Position = customLocsList[index-1].getLocation(); // Set their position and heading to the proper one
                            playerPed.Heading = customLocsList[index-1].getHeading();
                            Game.DisplaySubtitle("~g~Teleported to: " + "~b~" + customLocsList[index - 1].getName() + "~g~!");
                        }
                    };
                }
                
            }
            catch (ArgumentOutOfRangeException AE)
            {
                Game.DisplayNotification("~r~[GoThere]\nSomething went wrong! IndexOutOfRange!");
            }
            finally
            {
                LocMenu.RefreshIndex();
                menu_pool.RefreshIndex();
            }
        }

        // This Method is called right when our program begins, it's gonna take care of all of the reading/writing from our XML file we may need to do when the plugin loads.
        public static void handleFileCreation()
        {
            try // Handle Options.xml creation
            {
                if (!File.Exists("Plugins/GoThere/Options.xml"))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                    using (XmlWriter writer = XmlWriter.Create("Plugins/GoThere/Options.xml", settings)) // If the file key doesn't exist, create it
                    {
                        writer.WriteComment("GoThere uses specific names for keys that can be found at https://bit.ly/2lZm1nt");
                        writer.WriteStartElement("GoThere");
                        writer.WriteElementString("MenuKey", "F4");
                        writer.WriteElementString("ControllerButton", "None");
                        writer.WriteElementString("EnableCustomLocations", "false");
                        writer.WriteEndElement();
                        writer.Flush();

                        menuKey = System.Windows.Forms.Keys.F4; //Set the menu key to F4
                        // Do nothing about the ControllerButton
                        customLocationsEnabled = false; // Disable custom destinations 
                        //console.Print("[GoThere] Options.xml did not exist, so it was created. Default Menu Key is F4!");
                    }
                }
                else // If the file does exist
                {
                    // Right now I'm just going to use XMLDocument. Apparently XMLReader is faster and more memory efficient. 
                    XmlDocument options = new XmlDocument();
                    options.Load("Plugins/GoThere/Options.xml"); // Load the XML document

                    XmlNode menuKeyNode = options.DocumentElement.SelectSingleNode("/GoThere/MenuKey");
                    XmlNode buttonNode = options.DocumentElement.SelectSingleNode("/GoThere/ControllerButton"); //TODO: Add support for modifier keys & controller buttons as well.
                    XmlNode locationNode = options.DocumentElement.SelectSingleNode("/GoThere/EnableCustomLocations");
                    string tempkey = menuKeyNode.InnerText;
                    string tempButton = buttonNode.InnerText;
                    string locationKey = locationNode.InnerText;
                   



                    // Once we have grabbed all of our data from the XMLDocument, we should close it, and collect the garbage.
                    options = null;
                    menuKeyNode = null;
                    locationNode = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers(); // Called for GC to occur Synchronously.

                    // Try to set the menu toggle key
                    try
                    {
                        menuKey = (System.Windows.Forms.Keys)Enum.Parse(typeof(Keys), tempkey); // Set the Menu key to whatever it is in the XML file.
                        XButton = (ControllerButtons)Enum.Parse(typeof(ControllerButtons), tempButton); // Set the Menu controller button to whatever it is in the XML file.
                        //console.Print("[GoThere] Key to open GoThere Menu: ~g~" + tempkey + "~w~/~g~" + tempButton);
                    }
                    catch (ArgumentException AE) // Enum.Parse throws an ArgumentException if the key string is not in the enumeration, so we're going to need to catch it 
                    {
                        menuKey = Keys.F4; // Set the menuKey to F4
                        // Do nothing with the xbox key
                        //console.Print("[GoThere] The value for Menu Key or Menu Button in Options.xml does not exist! You can use F4 to open GoThere!");
                        Game.LogTrivial("The value for Menu Key or Menu Button in Options.xml does not exist! You can use F4 to open GoThere!");
                        Game.DisplayNotification("~r~[GoThere] \n~w~You have entered a value in ~b~Options.xml ~w~for a menu key/menu controller button that does not exist. You can use ~g~F4 ~w~ to open GoThere!");

                    }

                    // Try to enable/disable custom locations
                    try
                    {
                        customLocationsEnabled = Boolean.Parse(locationKey); // Set whether custom locations are enabled according to the value in the XML File.
                        if(customLocationsEnabled)
                        {
                            //console.Print("[GoThere] Custom locations have been enabled!");
                            Game.LogTrivial("Custom locations have been enabled!");
                        }
                        else
                        {
                            //console.Print("[GoThere] Custom locations have been disabled!");
                            Game.LogTrivial("Custom locations have been disabled!");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException || ex is ArgumentNullException)
                        {
                            customLocationsEnabled = false;
                            //console.Print("[GoThere] ERROR! You have entered a value in Options.xml for enabling custom locations that does not exist. Custom locations have been disabled!");
                            Game.LogTrivial("ERROR! You have entered a value in Options.xml for enabling custom locations that does not exist. Custom locations have been disabled!");
                            Game.DisplayNotification("~r~[GoThere] \n~w~You have entered a value in ~b~Options.xml ~w~for enabling custom locations that does not exist. Custom locations have been ~r~disabled~w~!");
                        }
                    }
                }

                if (!File.Exists("Plugins/GoThere/CustomLocations.xml")) // If CustomLocations.xml doesn't exist, create it.
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                    using (XmlWriter writer = XmlWriter.Create("Plugins/GoThere/CustomLocations.xml", settings)) // If the file key doesn't exist, create it
                    {
                        writer.WriteComment("GoThere uses this file to store custom locations! You may add a location manually inside this file, or create one in game.");
                        writer.WriteStartElement("CustomLocations");
                        writer.WriteStartElement("Item");
                        writer.WriteElementString("Name", "SampleLocation");
                        writer.WriteElementString("LocationX", "1853.753");
                        writer.WriteElementString("LocationY", "2586.172");
                        writer.WriteElementString("LocationZ", "45.67202");
                        writer.WriteElementString("LocationHeading", "91.27246");
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.Flush();

                       // console.Print("[GoThere] CustomLocations.xml did not exist, so it was created.");
                    }
                }
                else // If CustomLocations.xml already exists
                {
                    // For each item in the file
                    // Create a new destination object with the given data 
                    // Append to a list 
                    
                    XDocument customLoc = XDocument.Load("Plugins/GoThere/CustomLocations.xml");

                    foreach(XElement element in customLoc.Descendants("Item"))
                    {
                        try
                        {
                            String tempName = (String)element.Element("Name");
                            if (!tempName.Equals("SampleLocation"))  // We don't want to add our sample location, so as long as the name of the location isn't SampleLocation, add it to our menu
                            {
                                float tempX = (float)element.Element("LocationX");
                                float tempY = (float)element.Element("LocationY");
                                float tempZ = (float)element.Element("LocationZ");
                                float tempHead = (float)element.Element("LocationHeading");
                                Vector3 tempVector = new Vector3(tempX, tempY, tempZ); // Cast the values to a float. This may be tricky. Need to make sure casting to float doesn't mess anything up.
                                Destination newDest = new Destination(tempName, tempVector, tempHead);
                                customLocsList.Add(newDest); // Append this new location to the list. 
                                Game.LogTrivial("A custom location named: " + tempName + " has been created and added to the menu!");
                            }
                        }
                        catch(InvalidCastException ICE)
                        {
                           // console.Print("[GoThere] There was a cast problem in CustomLocations.xml. Please re-check your values and try again.");
                        }

                    }
                    
    
                }
            }
            catch (DirectoryNotFoundException DNF) // Exception thrown when the GoThere folder does not reside in the Plugins folder upon loading the plugin. 
            {
                //console.Print("[GoThere] GoThere folder could not be found in GTAV/Plugins folder. Please either create the folder or drag and drop it from the zip archive.");
                Game.LogTrivial("GoThere folder could not be found in GTAV/Plugins folder. Please either create the folder or drag and drop it from the zip archive.");
                Game.DisplayNotification("~r~[GoThere] \n~w~GoThere folder could not be found in ~g~GTAV~w~/~g~Plugins ~w~folder. Please either ~g~create ~w~the folder or ~g~drag and drop ~w~it from the zip archive.");
                Game.UnloadActivePlugin();  //Exit the plugin? 
                // Exit the plugin.
            }
        }
        public static void writeCustomLoc(Destination d)
        {
            XDocument customLoc = XDocument.Load("Plugins/GoThere/CustomLocations.xml");

            var newStuff = new XElement("Item",
                           new XElement("Name", d.getName()),
                           new XElement("LocationX", d.getLocation().X),
                           new XElement("LocationY", d.getLocation().Y),
                           new XElement("LocationZ", d.getLocation().Z),
                           new XElement("LocationHeading", d.getHeading()));
            customLoc.Element("CustomLocations").Add(newStuff);

            customLoc.Save("Plugins/GoThere/CustomLocations.xml");
        }
    }
}
