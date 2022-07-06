using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteToImage : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_sprites;
    [SerializeField]
    UITexture m_texture;

    // Start is called before the first frame update
    void Start()
    {
        m_sprites = Resources.LoadAll<Sprite>("Images/Fonts");

        for(int i =0; i<m_sprites.Length;i++)
        {
            var sprite = m_sprites[i];
            //mipChain : 원본을 거리에 따라 2의 제곱비율로 줄여서 로드, 2D에선 잘 안쓰지만, 3D에선 자주씀
            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);

            /*for(int y = 0; y<texture.height; y++)
            {
                for(int x =0; x<texture.width; x++)
                {
                    var color = sprite.texture.GetPixel((int)sprite.rect.x + x, (int)sprite.rect.y + y);
                    texture.SetPixel(x, y, color);
                }
            }*/
            //위와 같은 방식으로 작동하는 함수 SetPixels
            texture.SetPixels(sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height));
            texture.Apply();
            /*m_texture.mainTexture = texture;
            m_texture.MakePixelPerfect();*/
            string path = string.Empty;
            //Application.dataPath : Assets 폴더까지 자동으로 입력
            //Application.persistentDataPath : 프로젝트, 빌드파일이 설치된 경로
#if UNITY_EDITOR
            path = Application.dataPath;
#else
            path = Application.persistentDataPath;
#endif
            //파일로 저장하기
            var image = texture.EncodeToPNG();
            File.WriteAllBytes(string.Format("{0}/Fonts/image_{1:000}.png", path, i + 1), image);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
