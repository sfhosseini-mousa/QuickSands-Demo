using UnityEngine.UI;
using UnityEngine;
using Spine.Unity;

namespace Sands
{
    class HeroPopUpCreator : MonoBehaviour
    {
        [SerializeField] GameObject heroPopUpPrefab;
        ObjectFader objectFader = new ObjectFader();

        public void CreateHeroPopUp(ref GameObject instantiatedHeroPopUp, string text, Transform pos, int skinTier)
        {
            instantiatedHeroPopUp = Instantiate(heroPopUpPrefab, pos);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = text;

            GameObject hero = (GameObject)Resources.Load(HeroPartyDB.getHero(0).GetType().Name, typeof(GameObject));
            hero.GetComponent<Hero>().SkinTire = skinTier;
            hero.GetComponent<Hero>().setSkin(hero);
            GameObject instantiatedHero = Instantiate(hero, instantiatedHeroPopUp.transform.GetChild(1));
            instantiatedHero.transform.localScale = new Vector3(200f, 200f, 0f);

            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            instantiatedHero.GetComponent<SkeletonMecanim>().skeleton.A = 0f;
            StartCoroutine(objectFader.ApearSpineSkeletonMecenim(instantiatedHero));
        }
    }
}
