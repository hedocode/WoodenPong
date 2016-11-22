using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WoodenPong
{
    class Sprite
    {
        private readonly Game1 _game;
        private Vector2 _position;
        private readonly Texture2D _texture;
        public Vector2 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }
        public Vector2 Velocity { get; set; }

        public Sprite(Game1 game, Texture2D texture, Vector2 pos, Vector2 vel)
        {
            _game = game;
            _texture = texture;
            Position = pos;
            Velocity = vel;
        }

        public void Update(double time)
        {
            _position.X += Velocity.X * (float)time;
            _position.Y += Velocity.Y * (float)time;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, _game.WindowsWidth, _game.WindowsHeight), Color.White);
        }
    }
}
