using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillageGame.Models;


namespace VillageGame.Controllers
{
    public class HomeController : Controller
    {
        public VillageGameEntities2 db = new VillageGameEntities2();
        public Random ran = new Random();

        public bool EndGame()
        {
            if (db.Resources.Find(1).Food < 0 || db.Resources.Find(1).Water < 0)
            {
                return true;
            }
            return false;
        }

        public bool GotRaided()
        {
            int numCastles = (int)db.Buildings.Find(1).Castles;
            int castleProtection = 5 * numCastles;
            int chance = ran.Next(1, 101);

            if(numCastles == 2)
            {
                return false;
            }

            if(chance <= 10 - castleProtection)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuildAction(string type)
        {
            if(type == "house")
            {
                db.Buildings.Find(1).Houses++;
                db.Resources.Find(1).Wood -= 5;
                
                Session["ActionMessage"] = "You built a house!";
            }
            else if(type == "well")
            {
                db.Buildings.Find(1).Wells++;
                db.Resources.Find(1).Wood -= 6;
                Session["ActionMessage"] = "You built a well!";
            }
            else if(type == "farm")
            {
                db.Buildings.Find(1).Farms++;
                db.Resources.Find(1).Wood -= 8;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ResourceAction(string type)
        {

            if (type == "wood")
            {
                int quantityWood = ran.Next(1, 4);
                db.Resources.Find(1).Wood += quantityWood;
                Session["ActionMessage"] = "You found " + quantityWood + " wood!";
            }
            else if(type == "food")
            {
                int quantityFood = ran.Next(1, 4);
                db.Resources.Find(1).Food += quantityFood;
                Session["ActionMessage"] = "You found " + quantityFood + " food!";
            }
            else if(type == "water")
            {
                int quantityWater = ran.Next(1, 5);
                db.Resources.Find(1).Water += quantityWater;
                Session["ActionMessage"] = "You found " + quantityWater + " Water!";
            }
            else if(type == "stone")
            {
                int quantityStone = ran.Next(0, 4);
                db.Resources.Find(1).Stone += quantityStone;
            }
            db.GameDatas.Find(1).Actions--;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        
        public ActionResult GameView()
        {
            return View();
        }

        public ActionResult CreateVillager()
        {
            return View();
        }

        [HttpPost] 
        public ActionResult CreateVillager(string Name)
        {
            Villager villager = new Villager();
            villager.Name = Name;
            db.Villagers.Add(villager);
            Session["ActionMessage"] = "New villager " + villager.Name + " has entered the town!";
            db.GameDatas.Find(1).Actions++;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NextDay()
        {
            int VillagerCount = db.Villagers.ToArray().Length;
            var buildings = db.Buildings.Find(1);
            var Game = db.GameDatas.Find(1);
            var resources = db.Resources.Find(1);
            if(EndGame())
            {
                return RedirectToAction("GameOver");
            }

            if(db.GameDatas.Find(1).Actions > 0)
            {
                Session["ActionMessage"] = "You sill have Actions left!";
                return RedirectToAction("Index");
            }

            Session["ActionMessage"] = "";
            resources.Food -= VillagerCount - buildings.Farms;
            resources.Water -= VillagerCount - buildings.Wells;
            Game.Actions = VillagerCount;
            Game.Days++;
            db.SaveChanges();
            if (GotRaided())
            {
                Session["Raided"] = true;
                return RedirectToAction("Raided");
            }
            if (EndGame())
            {
                return RedirectToAction("GameOver");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //make an are you sure message page at some point, for now it starts a new game
        public ActionResult YouSure(string type)
        {
            List<Villager> villagers = db.Villagers.ToList();
            db.Resources.Find(1).Food = 6;
            db.Resources.Find(1).Water = 6;
            db.Resources.Find(1).Wood = 0;
            db.Buildings.Find(1).Wells = 0;
            db.Buildings.Find(1).Houses = 1;
            db.Buildings.Find(1).Farms = 0;
            db.Buildings.Find(1).Castles = 0;
            db.GameDatas.Find(1).Days = 0;
            db.GameDatas.Find(1).Actions = 0;

            for(int i = 0; i < villagers.Count; i++)
            {
                db.Villagers.Remove(villagers[i]);
            }
            db.SaveChanges();


            return View();
        }

        
        public ActionResult Raided()
        {
            int foodRaided = ran.Next(1, 4);
            int waterRaided = ran.Next(1, 4);
            int villagerCount = db.Villagers.ToArray().Length;
            List<Villager> villagers = db.Villagers.ToList();
            int VillagerIndex = ran.Next(1, villagerCount);
            var resource = db.Resources.Find(1);
            Villager unluckyGuy = villagers[VillagerIndex];
            string nameOfTheDeceased = unluckyGuy.Name;
            if ((bool)Session["Raided"] == true)
            {
                if (resource.Food < foodRaided)
                {
                    resource.Food = 0;
                }
                else
                {
                    db.Resources.Find(1).Food -= foodRaided;
                }

                if (resource.Water < waterRaided)
                {
                    resource.Water = 0;
                }
                else
                {
                    db.Resources.Find(1).Water -= waterRaided;
                }

                Session["LostFood"] = "You've lost " + foodRaided + " food";
                Session["LostWater"] = "You've lost " + waterRaided + " water";
                if()
                Session["LostVillager"] = nameOfTheDeceased + "has died and their house was burned to the ground";
                db.Buildings.Find(1).Houses--;

                db.Villagers.Remove(unluckyGuy);
                db.SaveChanges();
                Session["Raided"] = false;
            }
            else
            {
                ViewBag.NotRaided = "You're trying to get raided? Who's side are you on?";
            }

            return View();
        }

        public ActionResult GameOver()
        {
            return View();
        }

    }
}