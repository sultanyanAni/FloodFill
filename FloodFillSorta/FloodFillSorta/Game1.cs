using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
            pixels = new Sprite[10, 10];
            int x = 0;
            int y = 0;
            for (int v = 0; v < pixels.GetLength(0); v++)
            {
                for (int h = 0; h < pixels.GetLength(1); h++)
                {
                    pixels[v, h] = new Sprite(Content.Load<Texture2D>("pixel"), new Vector2(x,y), Color.White)
                    {
                        Scale = new Vector2(0.5f)
                    };
                    x += pixels[v, h].Width;
                  if(gen.Next(2) == 1)
                    {
                        pixels[v, h].Color = Color.Black;
                    }
                }
                y += pixels[v, 0].Height;
                x = 0;
            }
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
            if (pixels[row,column].Color == fillColor || pixels[row, column].Color != startColor) return;

            pixels[row, column].Color = fillColor;
            CompleteFill(row - 1, column, startColor, fillColor);//up
            CompleteFill(row + 1, column, startColor, fillColor);//down
            CompleteFill(row, column-1, startColor, fillColor);//left
            CompleteFill(row, column+1, startColor, fillColor);//right

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
                   if(pixels[row,column].Hitbox.Contains(location))
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

            if(ms.LeftButton == ButtonState.Pressed)
            {
                var index = GetIndex(new Vector2(ms.X, ms.Y));
               if (index.X > -1)
                {
                    CompleteFill((int)index.X, (int)index.Y, Color.White, Color.Chartreuse);
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
                    pixels[y, x].Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
