using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace FloodFillSorta
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite[,] pixels;
        Random gen = new Random();

        Dictionary<Keys, Color> textureColors = new Dictionary<Keys, Color>
        {
            [Keys.B] = Color.Blue,
            [Keys.R] = Color.Red,
            [Keys.G] = Color.Green
        };

        Color selectedColor; 

        Texture2D PixelTexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixels = new Sprite[20,20];
            PixelTexture = Content.Load<Texture2D>("pixel");
            selectedColor = Color.White;
            /*
            int x = 0;
            int y = 0;
            for (int v = 0; v < pixels.GetLength(0); v++)
            {
                for (int h = 0; h < pixels.GetLength(1); h++)
                {
                    pixels[v, h] = new Sprite(Content.Load<Texture2D>("pixel"), new Vector2(x, y), Color.White)
                    {
                        Scale = new Vector2(0.5f)
                    };
                    x += pixels[v, h].Width;
                    if (gen.Next(2) == 1)
                    {
                        pixels[v, h].Color = Color.Black;
                    }
                }
                y += pixels[v, 0].Height;
                x = 0;
            }*/

            IsMouseVisible = true;
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void CompleteFill(int row, int column, Color startColor, Color fillColor)
        {
            if (row < 0 || row >= pixels.GetLength(0) || column < 0 || column >= pixels.GetLength(1)) return;
            if (pixels[row, column].Color == fillColor || pixels[row, column].Color != startColor) return;

            pixels[row, column].Color = fillColor;
            CompleteFill(row - 1, column, startColor, fillColor);//up
            CompleteFill(row + 1, column, startColor, fillColor);//down
            CompleteFill(row, column - 1, startColor, fillColor);//left
            CompleteFill(row, column + 1, startColor, fillColor);//right

        }
        void IterativeFill(int initialRow, int initialColumn, Texture2D texture, Vector2 scale, float initialChance)
        {
            float chance = initialChance;
            float decay = .90f; 

            Queue<(int y, int x)> queuedPixels = new Queue<(int y, int x)>(); //storing indices 

            queuedPixels.Enqueue((initialRow, initialColumn));

            while (queuedPixels.Count > 0)
            {
                var current = queuedPixels.Dequeue();
                if(chance < gen.NextDouble() * initialChance)
                {
                    chance *= decay;
                    continue;
                }
                if (pixels[current.y,current.x] == null)
                {
                    pixels[current.y, current.x] = new Sprite(texture, new Vector2(current.x * texture.Width * scale.X, current.y * texture.Height * scale.Y), selectedColor);
                }
                else
                {
                    continue;
                }

                if (current.y - 1 > 0)
                {
                    queuedPixels.Enqueue((current.y - 1, current.x));
                }
                if (current.y + 1 < pixels.GetLength(0))
                {
                    queuedPixels.Enqueue((current.y + 1, current.x));
                }
                if (current.x - 1 > 0)
                {
                    queuedPixels.Enqueue((current.y, current.x -1));
                }
                if (current.x + 1 < pixels.GetLength(1))
                {
                    queuedPixels.Enqueue((current.y, current.x + 1));
                }
                chance *= decay;
            }
        }
        void FillWithDecay(int row, int column, Color startColor, Color fillColor)
        {

        }
        Vector2 GetIndex(Vector2 location)
        {
            for (int row = 0; row < pixels.GetLength(0); row++)
            {
                for (int column = 0; column < pixels.GetLength(1); column++)
                {
                    if (pixels[row, column] != null && pixels[row, column].Hitbox.Contains(location))
                    {
                        return new Vector2(row, column);
                    }
                }
            }

            return new Vector2(-1, -1);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            var keysPressed = ks.GetPressedKeys();
            for (int i = 0; i < keysPressed.Length; i++)
            {
                Keys currentKey = keysPressed[i];
                if (textureColors.ContainsKey(currentKey))
                {
                    selectedColor = textureColors[currentKey];
                }
            }

            if (ms.LeftButton == ButtonState.Pressed && IsActive)
            {
                int x = (int)(ms.X/(PixelTexture.Width * 1f));
                int y = (int)(ms.Y / (PixelTexture.Height * 1f));
                if (x < pixels.GetLength(1) && y < pixels.GetLength(0))
                {
                    // CompleteFill((int)index.X, (int)index.Y, Color.White, Color.Chartreuse);
                    IterativeFill(y, x, PixelTexture, new Vector2(1f), 1);
                    
                    
                    //You are not actually using your scale 😒, look at your sprite class constructor
                    //You were calculating it based on 0.5f scale
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int y = 0; y < pixels.GetLength(0); y++)
            {
                for (int x = 0; x < pixels.GetLength(1); x++)
                {
                    if (pixels[y, x] != null)
                    {
                        pixels[y, x].Draw(spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
