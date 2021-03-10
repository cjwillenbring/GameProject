using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameArchitectureExample.Collisions;

namespace GameArchitectureExample.GamePlay
{
    public static class PlatformBuilder
    {
        public static void GeneratePlatforms(List<PlatformSprite> platformList)
        {
            // Middle Platform
            platformList.Add(new PlatformSprite(new Vector2(334, 300)));
            platformList.Add(new PlatformSprite(new Vector2(380, 300)));
            platformList.Add(new PlatformSprite(new Vector2(426, 300)));
            platformList.Add(new PlatformSprite(new Vector2(472, 300)));

            // Left Platform
            platformList.Add(new PlatformSprite(new Vector2(334 - 275, 250)));
            platformList.Add(new PlatformSprite(new Vector2(380 - 275, 250)));
            platformList.Add(new PlatformSprite(new Vector2(426 - 275, 250)));
            platformList.Add(new PlatformSprite(new Vector2(472 - 275, 250)));

            // Right Platform
            platformList.Add(new PlatformSprite(new Vector2(334 + 250, 250)));
            platformList.Add(new PlatformSprite(new Vector2(380 + 250, 250)));
            platformList.Add(new PlatformSprite(new Vector2(426 + 250, 250)));
            platformList.Add(new PlatformSprite(new Vector2(472 + 250, 250)));
        }
    }
}
