using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WoodenPong
{
    public class Paddle
    {
        private readonly Game1 _game;
        private Texture2D _texture;
        private Vector2 _position;
        private readonly bool _isRight;
        public Vector2 Velocity { get; set; }

        public Texture2D Texture => _texture;

        public bool IsRight => _isRight;

        public int Height => _game.WindowsHeight / 6;

        public int Width => Height / 5;
            
        public Rectangle BoundingBox => new Rectangle( (int)_position.X, (int)_position.Y, Width, Height);


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

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("IMGS/Raquette");
            if (_isRight)
            {

                _position = new Vector2(_game.WindowsWidth - _game.WindowsWidth / 25 - _game.WindowsWidth / 60,
                    _game.WindowsHeight / 2 - _texture.Height / 4);
            }
            else
            {
                _position = new Vector2((float)_game.WindowsWidth / 25, _game.WindowsHeight / 2 - _texture.Height / 4);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
        }

        public void Update(double time)
        {
            _position.X += Velocity.X * (float)time;
            _position.Y += Velocity.Y * (float)time;
            if (_position.Y <= 0)
            {
                _position.Y = 0;
            }
            else if (_position.Y > _game.WindowsHeight - Height)
            {
                _position.Y = _game.WindowsHeight - Height;
            }
        }

        public Paddle (Game1 game, bool isRight)
        {
            _game = game;
            _isRight = isRight;
            Velocity = new Vector2(0, 0);
        }
    }
}
