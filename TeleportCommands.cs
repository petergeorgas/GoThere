using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoThere.Util;

namespace GoThere
{
    
    public static class TeleportCommands
    {
        static Ped playerPed = Game.LocalPlayer.Character; //Current Character Ped Object
        //It may be beneficial, resourcefully, to create our Destination objects up here

            
        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Bolingbroke Penitentiary.")]
        public static void Command_TeleportToBolingbroke()
        {
            try
            {
                Destination Bolingbroke = new Destination("Bolingbroke", new Vector3(1853.753f, 2586.172f, 45.67202f), 91.27246f); //Destination object for Bolingbroke
                playerPed.Position = Bolingbroke.getLocation(); // Set the player's current position to Bolingbroke
                playerPed.Heading = Bolingbroke.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Bolingbroke~g~!");
            }
            catch(Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Davis Police Station.")]
        public static void Command_TeleportToDavis()
        {
            try
            {
                Destination Davis = new Destination("Davis", new Vector3(357.5169f, -1581.96f, 29.29196f), 225.6059f);
                playerPed.Position = Davis.getLocation(); // Set the player's current position to Davis
                playerPed.Heading = Davis.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Davis~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Downtown Vinewood Police Station.")]
        public static void Command_TeleportToDowntownVinewood()
        {
            try
            {
                Destination Vinewood = new Destination("Downtown Vinewood", new Vector3(642.051f, 0.6869946f, 82.78673f), 66.49108f);
                playerPed.Position = Vinewood.getLocation(); // Set the player's current position to Downtown Vinewood
                playerPed.Heading = Vinewood.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Downtown Vinewood~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to La Mesa Police Station.")]
        public static void Command_TeleportToLaMesa()
        {
            try
            {
                Destination LaMesa = new Destination("La Mesa", new Vector3(816.6286f, -1290.104f, 26.28568f), 267.0676f);
                playerPed.Position = LaMesa.getLocation(); // Set the player's current position to La Mesa
                playerPed.Heading = LaMesa.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~La Mesa~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Los Santos International Airport Police Station.")]
        public static void Command_TeleportToLSIA()
        {
            try
            {
                Destination LSIA = new Destination("LSIA", new Vector3(-860.5375f, -2411.062f, 13.94444f), 61.23682f);
                playerPed.Position = LSIA.getLocation(); // Set the player's current position to LSIA
                playerPed.Heading = LSIA.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Los Santos International Airport~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Mission Row Police Station.")]
        public static void Command_TeleportToMissionRow()
        {
            try
            {
                Destination MissionRow = new Destination("Mission Row", new Vector3(430.1666f, -981.8361f, 30.71045f), 269.0408f);
                playerPed.Position = MissionRow.getLocation(); // Set the player's current position to Mission Row
                playerPed.Heading = MissionRow.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Mission Row~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Paleto Bay Police Station.")]
        public static void Command_TeleportToPaleto()
        {
            try
            {
                Destination PaletoBay = new Destination("Paleto Bay", new Vector3(-439.2147f, 6020.684f, 31.49011f), 132.1379f);
                playerPed.Position = PaletoBay.getLocation(); // Set the player's current position to Paleto Bay
                playerPed.Heading = PaletoBay.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Paleto Bay~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Rockford Hills Police Station.")]
        public static void Command_TeleportToRockford()
        {
            try
            {
                Destination Rockford = new Destination("Rockford", new Vector3(-559.9272f, -135.6659f, 38.17828f), 20.00237f);
                playerPed.Position = Rockford.getLocation(); // Set the player's current position to Rockford Hills
                playerPed.Heading = Rockford.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Rockford Hills~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Sandy Shores Police Station.")]
        public static void Command_TeleportToSandyShores()
        {
            try
            { 
            Destination Sandy = new Destination("Sandy", new Vector3(1859.35f, 3676.741f, 33.64919f), 29.02762f);
            playerPed.Position = Sandy.getLocation(); // Set the player's current position to Sandy Shores
            playerPed.Heading = Sandy.heading;        // Set the player's current heading to straght on
            Game.DisplaySubtitle("~g~Teleported to: " + "~b~Sandy Shores~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Vespucci Police Station.")]
        public static void Command_TeleportToVespucci()
        {
            try
            {
                Destination Vespucci = new Destination("Vespucci", new Vector3(-1114.111f, -849.8613f, 19.31662f), 307.3961f);
                playerPed.Position = Vespucci.getLocation(); // Set the player's current position to Vespucci
                playerPed.Heading = Vespucci.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Vespucci~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        [Rage.Attributes.ConsoleCommand(Description = "Teleports the player to Vinewood Hills Police Station.")]
        public static void Command_TeleportToVinewoodHills()
        {
            try
            {
                Destination VHills = new Destination("VHills", new Vector3(379.1551f, 790.9927f, 190.4106f), 357.8441f);
                playerPed.Position = VHills.getLocation(); // Set the player's current position to Vinewood Hills
                playerPed.Heading = VHills.heading;        // Set the player's current heading to straght on
                Game.DisplaySubtitle("~g~Teleported to: " + "~b~Vinewood Hills~g~!");
            }
            catch (Rage.Exceptions.InvalidHandleableException IHE)
            {
                Game.DisplaySubtitle("~r~Unable to teleport!");
            }
        }

        //A debugging command to get the player's current location
        [Rage.Attributes.ConsoleCommand]
        public static void Command_GetLocation()
        {  
            Vector3 curentLocation = playerPed.Position;
            float heading = playerPed.Heading;
            Game.DisplaySubtitle("~b~Current Location: ~w~" + curentLocation + " ~r~Current Heading: ~w~" + heading);
        } 
    }
}
