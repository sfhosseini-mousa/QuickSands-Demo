using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Spine.Unity;

namespace Sands
{
    class ObjectFader
    { 
        public IEnumerator FlashSpineSkeletonMecenim(GameObject gameObject)
        {
            Spine.Skeleton skeleton = gameObject.GetComponent<SkeletonMecanim>().skeleton;

            while (true)
            {
                while (skeleton.A < 1f)
                {
                    skeleton.A += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                while (skeleton.A > 0.5f)
                {
                    skeleton.A -= 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        public IEnumerator FlashSpineSkeletonAnimation(GameObject gameObject)
        {
            Spine.Skeleton skeleton = gameObject.GetComponent<SkeletonAnimation>().skeleton;

            while (true)
            {
                while (skeleton.A < 1f)
                {
                    skeleton.A += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                while (skeleton.A > 0.5f)
                {
                    skeleton.A -= 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        public IEnumerator FlashImage(GameObject gameObject)
        {
            Image image = gameObject.GetComponent<Image>();
            Color color = image.color;

            while (true)
            {
                while (image.color.a < 1f)
                {
                    color.a += 0.01f;
                    image.color = color;
                    yield return new WaitForSeconds(0.01f);
                }
                while (image.color.a > 0.5f)
                {
                    color.a -= 0.01f;
                    image.color = color;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        public IEnumerator AppearText(GameObject gameObject)
        {
            Text text = gameObject.GetComponent<Text>();
            Color color = text.color;

            while (text.color.a < 1)
            {
                color.a += 0.04f;
                text.color = color;
                yield return new WaitForSeconds(0.02f);
            }
        }

        public IEnumerator DisappearText(GameObject gameObject)
        {
            Text text = gameObject.GetComponent<Text>();
            Color color = text.color;

            while (text.color.a > 0)
            {
                color.a -= 0.04f;
                text.color = color;
                yield return new WaitForSeconds(0.02f);
            }
        }

        public IEnumerator ApearCanvasGroup(GameObject gameObject)
        {
            CanvasGroup canvasGroup= gameObject.GetComponent<CanvasGroup>();

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += 0.1f;
                yield return new WaitForSeconds(0.02f);
            }
        }

        public IEnumerator DisappearCanvasGroup(GameObject gameObject)
        {
            CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 0.1f;
                yield return new WaitForSeconds(0.02f);
            }
        }

        public IEnumerator ApearSpineSkeletonMecenim(GameObject gameObject)
        {
            Spine.Skeleton skeleton = gameObject.GetComponent<SkeletonMecanim>().skeleton;

            while (skeleton.A < 0.9f)
            {
                skeleton.A += 0.04f;
                yield return new WaitForSeconds(0.02f);
            }
             
            skeleton.A = 1f;
        }

        public IEnumerator DisapearSpineSkeletonMecenim(GameObject gameObject)
        {
            Spine.Skeleton skeleton = gameObject.GetComponent<SkeletonMecanim>().skeleton;

            while (skeleton.A > 0.1f)
            {
                skeleton.A -= 0.04f;
                yield return new WaitForSeconds(0.02f);
            }

            skeleton.A = 0f;
        }
    }
}
