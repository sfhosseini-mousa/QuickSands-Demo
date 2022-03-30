using System;

namespace Sands
{
    public class HeroStatChecker
    {
        public int GetHeroHealth(Hero hero)
        {
            int health = 0;

            if (hero.SkinTire < 5)
            {
                if (hero.GetType().Name == "Warrior")
                {
                    health = ArmorDatabase.getArmor(hero.SkinTire - 1).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Mage")
                {
                    health = ArmorDatabase.getArmor(hero.SkinTire + 4).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Ranger")
                {
                    health = ArmorDatabase.getArmor(hero.SkinTire + 9).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Spearman")
                {
                    health = ArmorDatabase.getArmor(hero.SkinTire + 14).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Wizard")
                {
                    health = ArmorDatabase.getArmor(hero.SkinTire + 19).Health + hero.MaxHP;
                }
            }
            else
            {
                if (hero.GetType().Name == "Warrior")
                {
                    health = ArmorDatabase.getArmor(4).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Mage")
                {
                    health = ArmorDatabase.getArmor(9).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Ranger")
                {
                    health = ArmorDatabase.getArmor(14).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Spearman")
                {
                    health = ArmorDatabase.getArmor(19).Health + hero.MaxHP;
                }
                else if (hero.GetType().Name == "Wizard")
                {
                    health = ArmorDatabase.getArmor(24).Health + hero.MaxHP;
                }
            }

            return health;
        }

        public int GetHeroDamage(Hero hero)
        {
            int damage = 0;

            if (hero.SkinTire < 5)
            {
                if (hero.GetType().Name == "Warrior")
                {
                    damage = WeaponDatabase.getWeapon(hero.SkinTire - 1).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Mage")
                {
                    damage = WeaponDatabase.getWeapon(hero.SkinTire + 4).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Ranger")
                {
                    damage = WeaponDatabase.getWeapon(hero.SkinTire + 9).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Spearman")
                {
                    damage = WeaponDatabase.getWeapon(hero.SkinTire + 14).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Wizard")
                {
                    damage = WeaponDatabase.getWeapon(hero.SkinTire + 19).Damage + hero.Damage;
                }
            }
            else
            {
                if (hero.GetType().Name == "Warrior")
                {
                    damage = WeaponDatabase.getWeapon(4).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Mage")
                {
                    damage = WeaponDatabase.getWeapon(9).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Ranger")
                {
                    damage = WeaponDatabase.getWeapon(14).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Spearman")
                {
                    damage = WeaponDatabase.getWeapon(19).Damage + hero.Damage;
                }
                else if (hero.GetType().Name == "Wizard")
                {
                    damage = WeaponDatabase.getWeapon(24).Damage + hero.Damage;
                }
            }

            return damage;
        }
    }
}
