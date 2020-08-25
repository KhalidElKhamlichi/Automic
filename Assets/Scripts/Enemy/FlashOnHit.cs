using System.Collections;
using Automic.Common;
using UnityEngine;

namespace Automic {
    [RequireComponent(typeof(CollisionManager))]
    public class FlashOnHit : MonoBehaviour {
    
        private MeshRenderer renderer;
        private float counter;
        private Color original;
        private bool flashing;
        void Start() {
            GetComponent<CollisionManager>().onHit(flash);
            renderer = GetComponent<MeshRenderer>();
            original = renderer.material.GetColor("_BaseColor");
        }

        private void flash(Collision obj) {
            counter = 0;
            renderer.material.SetColor("_BaseColor", Color.white);
            StopCoroutine("resetColor");
            StartCoroutine("resetColor", .1f);
        }

        private IEnumerator resetColor(float step) {
            while (counter < 1) {
                renderer.material.SetColor("_BaseColor", Color.Lerp(Color.white, original, counter));
                counter += step;
                yield return new WaitForSeconds(step);
            }
        }
    }
}
