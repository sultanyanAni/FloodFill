using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FloodFillSorta
{
    public class Sprite
    {
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public int Y { get { return (int)Position.Y; } set { Position = new Vector2(Position.X, value); }  }
        public int X { get { return (int)Position.X; } set { Position = new Vector2(value, Position.Y); } }
        public int Width => (int)(Image.Width * Scale.X);
        public int Height => (int)(Image.Height * Scale.Y);
        public Color Color { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public virtual Rectangle? SourceRectangle { get; }
        public Vector2 Scale { get; set; }

        public Rectangle Hitbox => new Rectangle(X, Y, Width, Height);
       // public Vector2 Origin => new Vector2(Position.X + Image.Width / 2, Position.Y + Image.Height / 2);
        public Sprite(Texture2D image, Vector2 position, Color color)             
        {
            Image = image;
            Position = position;
            Color = color;
            Scale = Vector2.One;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, SourceRectangle, Color, 0, Vector2.Zero, Scale, SpriteEffects, 0);
        }
    }
}
