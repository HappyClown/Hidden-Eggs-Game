using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

    public class VertexColorCycler : MonoBehaviour
    {
        public float fadeInDur = 1.0f;
        public float timeBetweenChars = 0.5f;

        private TMP_Text m_TextComponent;

        void Awake()
        {
            m_TextComponent = GetComponent<TMP_Text>();
        }


        void Start()
        {
            StartCoroutine(AnimateVertexColors());
        }


        /// <summary>
        /// Method to animate vertex colors of a TMP Text object.
        /// </summary>
        /// <returns></returns>
        IEnumerator AnimateVertexColors()
        {
            TMP_TextInfo textInfo = m_TextComponent.textInfo;
            int currentCharacter = 0;

            Color32[] newVertexColors;
            Color32 c0 = m_TextComponent.color;

            while (currentCharacter < textInfo.characterCount)
            {
                int characterCount = textInfo.characterCount;

                // Get the index of the material used by the current character.
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

                // Get the vertex colors of the mesh used by this text element (character or sprite).
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Get the index of the first vertex used by this text element.
                int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

                StartCoroutine(FadeInCharacter(materialIndex, vertexIndex, currentCharacter));
                Debug.Log(currentCharacter);

                // // Only change the vertex color if the text element is visible.
                // if (textInfo.characterInfo[currentCharacter].isVisible)
                // {
                //     c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

                //     newVertexColors[vertexIndex + 0] = c0;
                //     newVertexColors[vertexIndex + 1] = c0;
                //     newVertexColors[vertexIndex + 2] = c0;
                //     newVertexColors[vertexIndex + 3] = c0;

                //     // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                //     m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                //     // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                //     // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
                // }

                currentCharacter += 1;

                yield return new WaitForSeconds(timeBetweenChars);
            }
        }

        IEnumerator FadeInCharacter(int materialIndex, int vertexIndex, int currentCharacter) {
            TMP_TextInfo textInfo = m_TextComponent.textInfo;

            Color32[] newVertexColors;
            Color32 c0 = m_TextComponent.color;

            // Get the vertex colors of the mesh used by this text element (character or sprite).
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            float alpha = 0f;
            float fadeToOne = 0f;
            while (alpha < 255)
            {
                fadeToOne += Time.deltaTime / fadeInDur;
                alpha = fadeToOne * 255;
                if (alpha > 255) {
                    alpha = 255;
                }
                //Debug.Log(alpha);

                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    c0 = new Color32(255, 255, 255, (byte)alpha);

                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;

                    // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    // This last process could be done to only update the vertex data that has changed as opposed to all of the vertex data but it would require extra steps and knowing what type of renderer is used.
                    // These extra steps would be a performance optimization but it is unlikely that such optimization will be necessary.
                }
                yield return null;
            }
            c0 = new Color32(255, 255, 255, 255);

            newVertexColors[vertexIndex + 0] = c0;
            newVertexColors[vertexIndex + 1] = c0;
            newVertexColors[vertexIndex + 2] = c0;
            newVertexColors[vertexIndex + 3] = c0;

            // New function which pushes (all) updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
            m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return null;
        }

    }
}
