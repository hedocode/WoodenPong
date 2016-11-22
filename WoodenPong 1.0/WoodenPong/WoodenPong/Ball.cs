using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WoodenPong
{
    public class Ball
    {
        private Game1 _game;
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        public int Maxvelocity => _game.WindowsHeight;

        public bool HasHitAntibug;

        public int Width => _game.WindowsWidth / 35;
        public int Height => _game.WindowsWidth / 35;

        public Rectangle BoundingBox => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

        public bool IsAttached { get; private set; }
        private Paddle _paddleAttached;

        public Texture2D Texture => _texture;

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                if (value.X <= Maxvelocity && value.Y <= Maxvelocity && value.X >= -Maxvelocity && value.Y >= -Maxvelocity)
                {
                    _velocity = value;
                }

                else if(value.X > Maxvelocity && value.Y > Maxvelocity)
                {
                    _velocity = new Vector2(Maxvelocity, Maxvelocity);
                }
                else if (value.X < -Maxvelocity && value.Y < -Maxvelocity)
                {
                    _velocity = new Vector2(-Maxvelocity, -Maxvelocity);
                }

                else if(value.X > Maxvelocity)
                {
                    _velocity.X = Maxvelocity;
                }
                else if (value.X < -Maxvelocity)
                {
                    _velocity.X = -Maxvelocity;
                }
                else if (value.Y > Maxvelocity)
                {
                    _velocity.Y = Maxvelocity;
                }
                else
                {
                    _velocity.Y = -Maxvelocity;
                }
            }
        }

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

        public void AttachTo(Paddle paddle)
        {
            if (paddle.IsRight)
            {
                Velocity = new Vector2(0, 0);
                _position = new Vector2(paddle.Position.X - Width, paddle.Position.Y + paddle.Height/2f );
                IsAttached = true;
                _paddleAttached = paddle;
                HasHitAntibug = false;
            }
            else
            {
                Velocity = new Vector2(0, 0);
                _position = new Vector2(paddle.Position.X + paddle.Width, paddle.Position.Y + paddle.Height/2f - Height );
                IsAttached = true;
                _paddleAttached = paddle;
            }
        }

        public bool IsAttachedToPaddle(Paddle paddle)
        {
            if (IsAttached)
            {
                if (_paddleAttached == paddle)
                {
                    return true;
                }
            }
            return false;
        }

        public void Shoot(Paddle p)
        {
            
            if (_paddleAttached.IsRight)
            {
                if (_position.X <= p.Position.X && _position.Y > p.Position.Y && _position.Y < p. Position.Y + p.Height)
                {
                    Velocity = new Vector2(-_game.WindowsHeight/2f, p.Velocity.Y);
                    IsAttached = false;
                    _paddleAttached = null;
                }
            }
            else
            {
                if (_position.X >= p.Position.X + p.Width && _position.Y > p.Position.Y && _position.Y < p.Position.Y + p.Height)
                {
                    Velocity = new Vector2(_game.WindowsHeight/2f, p.Velocity.Y);
                    IsAttached = false;
                    _paddleAttached = null;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
        }

        public void Update(double time)
        {
            _position.X += _velocity.X*(float)time;
            _position.Y += _velocity.Y * (float)time;
            if (_position.Y < 0 || _position.Y > _game.WindowsHeight - Height)
            {
                if (_position.Y < 0)
                    _position.Y = 0;
                else
                    _position.Y = _game.WindowsHeight - Height;

                _velocity.Y = -_velocity.Y;

                if (_game.SoundIsOn)
                {
                    if (_position.X < _game.WindowsWidth / 4f)
                    {
                        _game.BallHitLeft.Play();
                    }
                    else if (_position.X > 3 * _game.WindowsWidth / 4f)
                    {
                        _game.BallHitRight.Play();
                    }
                    else
                    {
                        _game.BallHitUpDown.Play();
                    }
                }
            }

            if (_position.X < 0)
            {
                AttachTo(_game.R2);
                _game.Scorep2++;
            }
            else if(_position.X > _game.WindowsWidth)
            {
                AttachTo(_game.R1);
                _game.Scorep1++;
            }
        }

        public Ball(Game1 game, Paddle p1, Texture2D ballTexture)
        {
            _game = game;
            _texture = ballTexture;
            AttachTo(p1);
        }

        public Ball(Game1 game, int x, int y, Texture2D ballTexture, Vector2 velocity)
        {
            _game = game;
            _texture = ballTexture;
            _position = new Vector2(x, y);
            Velocity = velocity;
            HasHitAntibug = false;
        }
        
    }
}
